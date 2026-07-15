import { describe, expect, it } from 'vitest'
import { toScenePosition } from './prototypeCoordinates'

describe('toScenePosition', () => {
  it('maps normalized prototype coordinates into visual scene space', () => {
    expect(toScenePosition([0.25, -0.5])).toEqual([2.5, -5])
  })
})
