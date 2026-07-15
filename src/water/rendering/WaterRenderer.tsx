import { useEffect, useMemo } from 'react'
import { DoubleSide } from 'three'
import type { Stage00WaterPrototype } from '../../types'
import type { GeneratedTerrain } from '../../terrain'
import { createWaterGeometry } from './createWaterGeometry'

interface WatercourseRendererProps {
  terrain: GeneratedTerrain
  watercourse: Stage00WaterPrototype['watercourses'][number]
}

function WatercourseRenderer({ terrain, watercourse }: WatercourseRendererProps) {
  const geometry = useMemo(
    () => createWaterGeometry(watercourse, terrain),
    [terrain, watercourse],
  )
  useEffect(() => () => geometry.dispose(), [geometry])

  return (
    <mesh geometry={geometry} name={watercourse.id} receiveShadow>
      <meshStandardMaterial
        color={watercourse.color}
        metalness={0.08}
        opacity={0.88}
        roughness={0.24}
        side={DoubleSide}
        transparent
      />
    </mesh>
  )
}

interface WaterRendererProps {
  data: Stage00WaterPrototype
  terrain: GeneratedTerrain
}

export function WaterRenderer({ data, terrain }: WaterRendererProps) {
  return (
    <group name={data.id}>
      {data.watercourses.map((watercourse) => (
        <WatercourseRenderer
          key={watercourse.id}
          terrain={terrain}
          watercourse={watercourse}
        />
      ))}
    </group>
  )
}
