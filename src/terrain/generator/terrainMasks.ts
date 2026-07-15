import type { PrototypePoint2D, TerrainLandform } from '../../types'

export function sampleLandformMask(
  point: PrototypePoint2D,
  landform: TerrainLandform,
): number {
  const dx = (point[0] - landform.position[0]) / landform.radius[0]
  const dz = (point[1] - landform.position[1]) / landform.radius[1]
  const distanceSquared = dx * dx + dz * dz
  return Math.exp(-Math.pow(distanceSquared, landform.transition))
}

function distanceToSegment(
  point: PrototypePoint2D,
  start: PrototypePoint2D,
  end: PrototypePoint2D,
): number {
  const segmentX = end[0] - start[0]
  const segmentZ = end[1] - start[1]
  const lengthSquared = segmentX * segmentX + segmentZ * segmentZ

  if (lengthSquared === 0) {
    return Math.hypot(point[0] - start[0], point[1] - start[1])
  }

  const amount = Math.max(
    0,
    Math.min(
      1,
      ((point[0] - start[0]) * segmentX +
        (point[1] - start[1]) * segmentZ) /
        lengthSquared,
    ),
  )
  const nearestX = start[0] + segmentX * amount
  const nearestZ = start[1] + segmentZ * amount
  return Math.hypot(point[0] - nearestX, point[1] - nearestZ)
}

export function distanceToPath(
  point: PrototypePoint2D,
  path: PrototypePoint2D[],
): number {
  let minimum = Number.POSITIVE_INFINITY

  for (let index = 0; index < path.length - 1; index += 1) {
    const start = path[index]
    const end = path[index + 1]
    if (start && end) {
      minimum = Math.min(minimum, distanceToSegment(point, start, end))
    }
  }

  return minimum
}
