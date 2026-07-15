import { BufferAttribute, BufferGeometry, Color } from 'three'
import type { TerrainSurfaceZone } from '../../types'
import type { GeneratedTerrain } from '../providers'

function findSurfaceColor(
  zones: TerrainSurfaceZone[],
  type: TerrainSurfaceZone['type'],
): Color {
  const zone = zones.find((candidate) => candidate.type === type)
  if (!zone) {
    throw new Error(`Missing terrain surface zone: ${type}`)
  }
  return new Color(zone.color)
}

export function createTerrainGeometry(
  terrain: GeneratedTerrain,
  zones: TerrainSurfaceZone[],
): BufferGeometry {
  const vertexCount = terrain.resolution * terrain.resolution
  const positions = new Float32Array(vertexCount * 3)
  const colors = new Float32Array(vertexCount * 3)
  const indices: number[] = []
  const heightRange = Math.max(terrain.maximumHeight - terrain.minimumHeight, 0.001)
  const hillColor = findSurfaceColor(zones, 'hill')
  const foothillColor = findSurfaceColor(zones, 'foothill')
  const valleyColor = findSurfaceColor(zones, 'valley')
  const riparianColor = findSurfaceColor(zones, 'riparian')

  for (let zIndex = 0; zIndex < terrain.resolution; zIndex += 1) {
    for (let xIndex = 0; xIndex < terrain.resolution; xIndex += 1) {
      const index = zIndex * terrain.resolution + xIndex
      const normalizedHeight =
        ((terrain.heights[index] ?? 0) - terrain.minimumHeight) / heightRange
      const baseColor =
        normalizedHeight > 0.62
          ? hillColor.clone()
          : normalizedHeight > 0.3
            ? foothillColor.clone()
            : valleyColor.clone()
      baseColor.lerp(riparianColor, (terrain.riparianWeights[index] ?? 0) * 0.82)

      positions[index * 3] =
        (xIndex / (terrain.resolution - 1) - 0.5) * terrain.width
      positions[index * 3 + 1] = terrain.heights[index] ?? 0
      positions[index * 3 + 2] =
        (zIndex / (terrain.resolution - 1) - 0.5) * terrain.depth
      colors[index * 3] = baseColor.r
      colors[index * 3 + 1] = baseColor.g
      colors[index * 3 + 2] = baseColor.b
    }
  }

  for (let zIndex = 0; zIndex < terrain.resolution - 1; zIndex += 1) {
    for (let xIndex = 0; xIndex < terrain.resolution - 1; xIndex += 1) {
      const topLeft = zIndex * terrain.resolution + xIndex
      const topRight = topLeft + 1
      const bottomLeft = topLeft + terrain.resolution
      const bottomRight = bottomLeft + 1
      indices.push(topLeft, bottomLeft, topRight, topRight, bottomLeft, bottomRight)
    }
  }

  const geometry = new BufferGeometry()
  geometry.setAttribute('position', new BufferAttribute(positions, 3))
  geometry.setAttribute('color', new BufferAttribute(colors, 3))
  geometry.setIndex(indices)
  geometry.computeVertexNormals()
  geometry.computeBoundingSphere()
  return geometry
}
