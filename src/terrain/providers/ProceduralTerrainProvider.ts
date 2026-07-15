import type { Stage00TerrainPrototype } from '../../types'
import { generateHeightField } from '../generator/generateHeightField'
import type {
  GeneratedTerrain,
  RiverTerrainModifier,
  TerrainProvider,
} from './TerrainProvider'

export class ProceduralTerrainProvider implements TerrainProvider {
  generate(
    data: Stage00TerrainPrototype,
    modifiers: RiverTerrainModifier[] = [],
  ): GeneratedTerrain {
    return generateHeightField(data, modifiers)
  }
}
