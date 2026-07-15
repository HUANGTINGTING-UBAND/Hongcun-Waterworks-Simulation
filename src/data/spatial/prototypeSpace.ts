import type { PrototypePoint2D } from '../../types'

export function normalizedToScene(
  point: PrototypePoint2D,
  width: number,
  depth: number,
): [x: number, z: number] {
  return [point[0] * width * 0.5, point[1] * depth * 0.5]
}
