import { describe, expect, it } from 'vitest'
import { z } from 'zod'
import { DataLoadError } from './DataLoadError'
import { loadStage00Data } from './loadStage00Data'
import { parseData } from './parseData'

describe('loadStage00Data', () => {
  it('loads and validates the timeline, terrain, and water datasets', () => {
    const data = loadStage00Data()

    expect(data.stage.id).toBe('stage_00_natural_hongcun')
    expect(data.terrain.stage).toBe(data.stage.id)
    expect(data.water.stage).toBe(data.stage.id)
    expect(data.stage.confidence).toBe('D')
    expect(data.terrain.confidence).toBe('D')
    expect(data.water.confidence).toBe('D')
    expect(data.stage.gameAdjustment).toBe(true)
    expect(data.terrain.gameAdjustment).toBe(true)
    expect(data.water.gameAdjustment).toBe(true)
  })

  it('reports the dataset and invalid field when validation fails', () => {
    expect(() =>
      parseData('example.json', { confidence: 'unknown' }, z.object({
        confidence: z.literal('D'),
      })),
    ).toThrow(DataLoadError)

    expect(() =>
      parseData('example.json', { confidence: 'unknown' }, z.object({
        confidence: z.literal('D'),
      })),
    ).toThrow(/example\.json.*confidence/)
  })
})
