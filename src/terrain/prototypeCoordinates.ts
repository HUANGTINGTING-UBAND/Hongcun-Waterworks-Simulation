import type { PrototypePoint2D } from '../types'

const PROTOTYPE_SCENE_SPAN = 10

export function toScenePosition(point: PrototypePoint2D): [x: number, z: number] {
  return [point[0] * PROTOTYPE_SCENE_SPAN, point[1] * PROTOTYPE_SCENE_SPAN]
}
