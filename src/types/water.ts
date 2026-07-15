import type {
  HistoricalMetadata,
  PrototypePoint2D,
} from './historicalData'

export interface VisualRiverCorridor extends HistoricalMetadata {
  id: string
  type: 'visual_river_corridor'
  surfaceWidth: number
  bedWidth: number
  bankWidth: number
  bedDepth: number
  surfaceOffset: number
}

export interface NaturalWatercoursePrototype extends HistoricalMetadata {
  id: string
  type: 'natural_stream'
  flowModel: 'visual_direction_only'
  color: string
  path: PrototypePoint2D[]
  corridor: VisualRiverCorridor
}

export interface Stage00WaterPrototype extends HistoricalMetadata {
  id: string
  type: 'natural_water_prototype'
  stage: 'stage_00_natural_hongcun'
  coordinateSystem: 'prototype_normalized_local'
  disclaimer: string
  watercourses: NaturalWatercoursePrototype[]
}
