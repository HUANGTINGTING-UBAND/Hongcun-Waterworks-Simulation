import stageJson from '../../data/timeline/stage_00.json'
import terrainJson from '../../data/terrain/terrain_stage00.json'
import waterJson from '../../data/water/water_stage00.json'
import type {
  HistoricalStage,
  Stage00TerrainPrototype,
  Stage00WaterPrototype,
} from '../types'
import { parseData } from './parseData'
import {
  historicalStageSchema,
  stage00TerrainSchema,
  stage00WaterSchema,
} from './schemas'

export interface Stage00Data {
  stage: HistoricalStage
  terrain: Stage00TerrainPrototype
  water: Stage00WaterPrototype
}

export function loadStage00Data(): Stage00Data {
  return {
    stage: parseData('data/timeline/stage_00.json', stageJson, historicalStageSchema),
    terrain: parseData(
      'data/terrain/terrain_stage00.json',
      terrainJson,
      stage00TerrainSchema,
    ),
    water: parseData(
      'data/water/water_stage00.json',
      waterJson,
      stage00WaterSchema,
    ),
  }
}
