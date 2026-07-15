export type HistoricalConfidence = 'A' | 'B' | 'C' | 'D'

export interface HistoricalMetadata {
  source: string
  confidence: HistoricalConfidence
  gameAdjustment: boolean
}

export type PrototypePoint2D = readonly [x: number, z: number]

export type PrototypeScale2D = readonly [x: number, z: number]
