import type {
  HistoricalMetadata,
  PrototypePoint2D,
  PrototypeScale2D,
} from './historicalData'

export interface PrototypeSpace extends HistoricalMetadata {
  id: string
  type: 'normalized_visual_space'
  extent: PrototypeScale2D
  sceneSpan: number
  heightMode: 'relative_visual_units'
}

export type NoiseLayerType =
  | 'large_scale_noise'
  | 'medium_scale_noise'
  | 'small_scale_noise'

export interface TerrainNoiseLayer extends HistoricalMetadata {
  id: string
  type: NoiseLayerType
  frequency: number
  amplitude: number
}

export interface TerrainHeightFieldConfig extends HistoricalMetadata {
  id: string
  type: 'procedural_height_field'
  gridResolution: number
  seed: number
  baseLevel: number
  visualHeightScale: number
  noiseLayers: TerrainNoiseLayer[]
}

export type TerrainLandformType =
  | 'distant_hills'
  | 'foothill_slope'
  | 'valley_plain'

export interface TerrainLandform extends HistoricalMetadata {
  id: string
  type: TerrainLandformType
  position: PrototypePoint2D
  radius: PrototypeScale2D
  relativeHeight: number
  transition: number
}

export type TerrainSurfaceType = 'hill' | 'foothill' | 'valley' | 'riparian'

export interface TerrainSurfaceZone extends HistoricalMetadata {
  id: string
  type: TerrainSurfaceType
  color: string
}

export interface Stage00TerrainPrototype extends HistoricalMetadata {
  id: string
  type: 'terrain_prototype'
  stage: 'stage_00_natural_hongcun'
  coordinateSystem: 'prototype_normalized_local'
  disclaimer: string
  space: PrototypeSpace
  heightField: TerrainHeightFieldConfig
  landforms: TerrainLandform[]
  surfaceZones: TerrainSurfaceZone[]
}
