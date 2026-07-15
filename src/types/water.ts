import type {
  HistoricalMetadata,
  PrototypePoint2D,
} from './historicalData'

export interface NaturalWatercoursePrototype extends HistoricalMetadata {
  id: string
  type: 'natural_stream'
  flowModel: 'visual_direction_only'
  path: PrototypePoint2D[]
}

export interface Stage00WaterPrototype extends HistoricalMetadata {
  id: string
  type: 'natural_water_prototype'
  stage: 'stage_00_natural_hongcun'
  coordinateSystem: 'prototype_normalized_local'
  disclaimer: string
  watercourses: NaturalWatercoursePrototype[]
}
