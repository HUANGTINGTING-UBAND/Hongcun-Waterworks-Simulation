import { useMemo } from 'react'
import type { Stage00Data } from '../../data'
import { ProceduralTerrainProvider, TerrainRenderer } from '../../terrain'
import { createRiverCorridor, WaterRenderer } from '../../water'

const terrainProvider = new ProceduralTerrainProvider()

interface NaturalLandscapeProps {
  stage00: Stage00Data
}

export function NaturalLandscape({ stage00 }: NaturalLandscapeProps) {
  const riverModifiers = useMemo(
    () => stage00.water.watercourses.map(createRiverCorridor),
    [stage00.water.watercourses],
  )
  const terrain = useMemo(
    () => terrainProvider.generate(stage00.terrain, riverModifiers),
    [riverModifiers, stage00.terrain],
  )

  return (
    <group name="stage00-natural-landscape-prototype">
      <TerrainRenderer
        surfaceZones={stage00.terrain.surfaceZones}
        terrain={terrain}
      />
      <WaterRenderer data={stage00.water} terrain={terrain} />
    </group>
  )
}
