import { describe, expect, it } from 'vitest'
import { loadStage00Data } from '../../data'
import { generateHeightField } from '../../terrain'
import { createRiverCorridor } from '../generator/createRiverCorridor'
import { createWaterGeometry } from './createWaterGeometry'

describe('createWaterGeometry', () => {
  it('creates a static river ribbon that follows the generated terrain', () => {
    const stage00 = loadStage00Data()
    const watercourse = stage00.water.watercourses[0]!
    const terrain = generateHeightField(stage00.terrain, [
      createRiverCorridor(watercourse),
    ])
    const geometry = createWaterGeometry(watercourse, terrain)
    const positions = geometry.getAttribute('position')

    expect(positions.count).toBeGreaterThan(200)
    expect(geometry.index?.count).toBeGreaterThan(600)
    expect(positions.getY(0)).toBeGreaterThan(terrain.minimumHeight)
    expect(positions.getY(0)).toBeCloseTo(
      terrain.sampleHeight(watercourse.path[0]!) +
        (watercourse.corridor.bedDepth * 0.72 +
          watercourse.corridor.surfaceOffset) *
          terrain.visualHeightScale,
      5,
    )

    geometry.dispose()
  })
})
