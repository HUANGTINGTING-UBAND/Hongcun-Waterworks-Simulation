import { describe, expect, it } from 'vitest'
import { loadStage00Data } from '../../data'
import { createRiverCorridor } from '../../water'
import { generateHeightField } from './generateHeightField'

const stage00 = loadStage00Data()
const corridor = createRiverCorridor(stage00.water.watercourses[0]!)

describe('generateHeightField', () => {
  it('generates deterministic terrain for the same D-level prototype data', () => {
    const first = generateHeightField(stage00.terrain, [corridor])
    const second = generateHeightField(stage00.terrain, [corridor])

    expect(first.heights).toEqual(second.heights)
    expect(first.resolution).toBe(stage00.terrain.heightField.gridResolution)
    expect(first.heights).toHaveLength(first.resolution * first.resolution)
    expect(first.width).toBe(stage00.terrain.space.sceneSpan)
    expect(first.visualHeightScale).toBe(
      stage00.terrain.heightField.visualHeightScale,
    )
    expect(Array.from(first.heights).every(Number.isFinite)).toBe(true)
  })

  it('creates a continuous mountain, foothill, valley height relationship', () => {
    const terrain = generateHeightField(stage00.terrain, [corridor])
    const mountainHeight = terrain.sampleHeight([-0.42, 0.3])
    const foothillHeight = terrain.sampleHeight([-0.2, 0.05])
    const valleyHeight = terrain.sampleHeight([0.3, -0.18])

    expect(mountainHeight).toBeGreaterThan(foothillHeight)
    expect(foothillHeight).toBeGreaterThan(valleyHeight)
  })

  it('carves the visual river corridor below the unmodified terrain', () => {
    const unmodified = generateHeightField(stage00.terrain)
    const modified = generateHeightField(stage00.terrain, [corridor])
    const riverPoint = stage00.water.watercourses[0]!.path[3]!

    expect(modified.sampleHeight(riverPoint)).toBeLessThan(
      unmodified.sampleHeight(riverPoint),
    )
  })
})
