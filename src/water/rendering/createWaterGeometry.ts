import {
  BufferAttribute,
  BufferGeometry,
  CatmullRomCurve3,
  Vector3,
} from 'three'
import { normalizedToScene } from '../../data/spatial'
import type { NaturalWatercoursePrototype } from '../../types'
import type { GeneratedTerrain } from '../../terrain'

const PATH_SEGMENTS = 120

export function createWaterGeometry(
  watercourse: NaturalWatercoursePrototype,
  terrain: GeneratedTerrain,
): BufferGeometry {
  const curve = new CatmullRomCurve3(
    watercourse.path.map(([x, z]) => new Vector3(x, 0, z)),
  )
  const positions = new Float32Array((PATH_SEGMENTS + 1) * 2 * 3)
  const indices: number[] = []

  for (let index = 0; index <= PATH_SEGMENTS; index += 1) {
    const amount = index / PATH_SEGMENTS
    const point = curve.getPoint(amount)
    const tangent = curve.getTangent(amount).normalize()
    const perpendicularX = -tangent.z
    const perpendicularZ = tangent.x
    const halfWidth = watercourse.corridor.surfaceWidth * 0.5
    const left: [number, number] = [
      point.x + perpendicularX * halfWidth,
      point.z + perpendicularZ * halfWidth,
    ]
    const right: [number, number] = [
      point.x - perpendicularX * halfWidth,
      point.z - perpendicularZ * halfWidth,
    ]
    const centerHeight =
      terrain.sampleHeight([point.x, point.z]) +
      (watercourse.corridor.bedDepth * 0.72 +
        watercourse.corridor.surfaceOffset) *
        terrain.visualHeightScale
    const [leftX, leftZ] = normalizedToScene(left, terrain.width, terrain.depth)
    const [rightX, rightZ] = normalizedToScene(right, terrain.width, terrain.depth)
    const leftOffset = index * 6
    const rightOffset = leftOffset + 3

    positions.set([leftX, centerHeight, leftZ], leftOffset)
    positions.set([rightX, centerHeight, rightZ], rightOffset)

    if (index < PATH_SEGMENTS) {
      const nextLeft = (index + 1) * 2
      const currentLeft = index * 2
      indices.push(
        currentLeft,
        currentLeft + 1,
        nextLeft,
        currentLeft + 1,
        nextLeft + 1,
        nextLeft,
      )
    }
  }

  const geometry = new BufferGeometry()
  geometry.setAttribute('position', new BufferAttribute(positions, 3))
  geometry.setIndex(indices)
  geometry.computeVertexNormals()
  geometry.computeBoundingSphere()
  return geometry
}
