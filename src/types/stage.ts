import type { HistoricalMetadata } from './historicalData'

export interface StageSystems {
  water_system: boolean
  moon_pond: boolean
  south_lake: boolean
}

export interface HistoricalStage extends HistoricalMetadata {
  id: string
  name: string
  type: 'historical_stage'
  description: string
  features: string[]
  systems: StageSystems
  player_goal: string
}
