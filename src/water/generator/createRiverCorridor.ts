import type { NaturalWatercoursePrototype } from '../../types'
import type { RiverTerrainModifier } from '../../terrain'

export function createRiverCorridor(
  watercourse: NaturalWatercoursePrototype,
): RiverTerrainModifier {
  return {
    id: watercourse.corridor.id,
    path: watercourse.path,
    bedWidth: watercourse.corridor.bedWidth,
    bankWidth: watercourse.corridor.bankWidth,
    bedDepth: watercourse.corridor.bedDepth,
  }
}
