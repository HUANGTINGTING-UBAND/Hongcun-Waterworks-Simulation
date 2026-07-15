import type {
  HistoricalMetadata,
  PrototypePoint2D,
  PrototypeScale2D,
} from './historicalData'

export type TerrainPrototypeFeatureType = 'mountain_mass' | 'valley'

export interface TerrainPrototypeFeature extends HistoricalMetadata {
  id: string
  type: TerrainPrototypeFeatureType
  shape: string
  position: PrototypePoint2D
  relativeHeight: number
  relativeScale: PrototypeScale2D
}

export interface Stage00TerrainPrototype extends HistoricalMetadata {
  id: string
  type: 'terrain_prototype'
  stage: 'stage_00_natural_hongcun'
  coordinateSystem: 'prototype_normalized_local'
  disclaimer: string
  features: TerrainPrototypeFeature[]
}
