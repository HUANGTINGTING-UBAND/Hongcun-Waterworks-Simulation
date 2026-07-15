import { describe, expect, it } from 'vitest'
import { loadStage00Data } from '../../data'
import { createRiverCorridor } from '../../water'
import { generateHeightField } from '../generator/generateHeightField'
import { createTerrainGeometry } from './createTerrainGeometry'

describe('createTerrainGeometry', () => {
  it('creates indexed positions, colors, and normals for the height field', () => {
    const stage00 = loadStage00Data()
    const terrain = generateHeightField(stage00.terrain, [
      createRiverCorridor(stage00.water.watercourses[0]!),
    ])
    const geometry = createTerrainGeometry(
      terrain,
      stage00.terrain.surfaceZones,
    )

    expect(geometry.getAttribute('position').count).toBe(
      terrain.resolution * terrain.resolution,
    )
    expect(geometry.getAttribute('color').count).toBe(
      terrain.resolution * terrain.resolution,
    )
    expect(geometry.getAttribute('normal').count).toBe(
      terrain.resolution * terrain.resolution,
    )
    expect(geometry.index?.count).toBe(
      (terrain.resolution - 1) * (terrain.resolution - 1) * 6,
    )

    geometry.dispose()
  })
})
