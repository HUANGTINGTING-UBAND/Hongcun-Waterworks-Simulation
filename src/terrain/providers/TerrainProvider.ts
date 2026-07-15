import type { PrototypePoint2D, Stage00TerrainPrototype } from '../../types'

export interface RiverTerrainModifier {
  id: string
  path: PrototypePoint2D[]
  bedWidth: number
  bankWidth: number
  bedDepth: number
}

export interface GeneratedTerrain {
  id: string
  resolution: number
  width: number
  depth: number
  visualHeightScale: number
  heights: Float32Array
  riparianWeights: Float32Array
  minimumHeight: number
  maximumHeight: number
  sampleHeight(point: PrototypePoint2D): number
}

export interface TerrainProvider {
  generate(
    data: Stage00TerrainPrototype,
    modifiers?: RiverTerrainModifier[],
  ): GeneratedTerrain
}
