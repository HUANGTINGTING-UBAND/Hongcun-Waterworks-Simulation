import type { PrototypePoint2D, Stage00TerrainPrototype } from '../../types'
import type {
  GeneratedTerrain,
  RiverTerrainModifier,
} from '../providers/TerrainProvider'
import { clamp, lerp, smoothstep } from './math'
import { sampleMultiScaleNoise } from './multiScaleNoise'
import { distanceToPath, sampleLandformMask } from './terrainMasks'

function applyLandforms(
  initialHeight: number,
  point: PrototypePoint2D,
  data: Stage00TerrainPrototype,
): number {
  let height = initialHeight

  for (const landform of data.landforms) {
    const mask = sampleLandformMask(point, landform)
    if (landform.type === 'valley_plain') {
      height = lerp(height, landform.relativeHeight, mask * 0.86)
    } else {
      const strength = landform.type === 'distant_hills' ? 0.92 : 0.48
      height += landform.relativeHeight * mask * strength
    }
  }

  return height
}

function applyRiverModifiers(
  initialHeight: number,
  point: PrototypePoint2D,
  modifiers: RiverTerrainModifier[],
): { height: number; riparianWeight: number } {
  let height = initialHeight
  let riparianWeight = 0

  for (const modifier of modifiers) {
    const distance = distanceToPath(point, modifier.path)
    const bankWeight = 1 - smoothstep(modifier.bedWidth, modifier.bankWidth, distance)
    const bedWeight = 1 - smoothstep(0, modifier.bedWidth, distance)
    height -= modifier.bedDepth * (bedWeight + bankWeight * 0.22)
    riparianWeight = Math.max(
      riparianWeight,
      1 - smoothstep(modifier.bedWidth, modifier.bankWidth, distance),
    )
  }

  return { height, riparianWeight }
}

function createHeightSampler(
  heights: Float32Array,
  resolution: number,
): (point: PrototypePoint2D) => number {
  return ([x, z]) => {
    const gridX = clamp((x + 1) * 0.5 * (resolution - 1), 0, resolution - 1)
    const gridZ = clamp((z + 1) * 0.5 * (resolution - 1), 0, resolution - 1)
    const x0 = Math.floor(gridX)
    const z0 = Math.floor(gridZ)
    const x1 = Math.min(x0 + 1, resolution - 1)
    const z1 = Math.min(z0 + 1, resolution - 1)
    const tx = gridX - x0
    const tz = gridZ - z0
    const top = lerp(
      heights[z0 * resolution + x0] ?? 0,
      heights[z0 * resolution + x1] ?? 0,
      tx,
    )
    const bottom = lerp(
      heights[z1 * resolution + x0] ?? 0,
      heights[z1 * resolution + x1] ?? 0,
      tx,
    )
    return lerp(top, bottom, tz)
  }
}

export function generateHeightField(
  data: Stage00TerrainPrototype,
  modifiers: RiverTerrainModifier[] = [],
): GeneratedTerrain {
  const resolution = data.heightField.gridResolution
  const heights = new Float32Array(resolution * resolution)
  const riparianWeights = new Float32Array(resolution * resolution)
  let minimumHeight = Number.POSITIVE_INFINITY
  let maximumHeight = Number.NEGATIVE_INFINITY

  for (let zIndex = 0; zIndex < resolution; zIndex += 1) {
    for (let xIndex = 0; xIndex < resolution; xIndex += 1) {
      const point: PrototypePoint2D = [
        (xIndex / (resolution - 1)) * 2 - 1,
        (zIndex / (resolution - 1)) * 2 - 1,
      ]
      const noise = sampleMultiScaleNoise(
        point[0],
        point[1],
        data.heightField.seed,
        data.heightField.noiseLayers,
      )
      const landformHeight = applyLandforms(
        data.heightField.baseLevel + noise,
        point,
        data,
      )
      const riverResult = applyRiverModifiers(landformHeight, point, modifiers)
      const height = riverResult.height * data.heightField.visualHeightScale
      const index = zIndex * resolution + xIndex
      heights[index] = height
      riparianWeights[index] = riverResult.riparianWeight
      minimumHeight = Math.min(minimumHeight, height)
      maximumHeight = Math.max(maximumHeight, height)
    }
  }

  return {
    id: data.id,
    resolution,
    width: data.space.sceneSpan * data.space.extent[0],
    depth: data.space.sceneSpan * data.space.extent[1],
    visualHeightScale: data.heightField.visualHeightScale,
    heights,
    riparianWeights,
    minimumHeight,
    maximumHeight,
    sampleHeight: createHeightSampler(heights, resolution),
  }
}
