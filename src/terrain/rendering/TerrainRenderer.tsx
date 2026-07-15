import { useEffect, useMemo } from 'react'
import type { TerrainSurfaceZone } from '../../types'
import type { GeneratedTerrain } from '../providers'
import { createTerrainGeometry } from './createTerrainGeometry'

interface TerrainRendererProps {
  terrain: GeneratedTerrain
  surfaceZones: TerrainSurfaceZone[]
}

export function TerrainRenderer({ terrain, surfaceZones }: TerrainRendererProps) {
  const geometry = useMemo(
    () => createTerrainGeometry(terrain, surfaceZones),
    [terrain, surfaceZones],
  )

  useEffect(() => () => geometry.dispose(), [geometry])

  return (
    <mesh
      castShadow
      receiveShadow
      geometry={geometry}
      name={terrain.id}
    >
      <meshStandardMaterial vertexColors roughness={0.96} metalness={0} />
    </mesh>
  )
}
