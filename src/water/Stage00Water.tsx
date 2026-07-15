import { CatmullRomCurve3, Vector3 } from 'three'
import type { NaturalWatercoursePrototype, Stage00WaterPrototype } from '../types'
import { toScenePosition } from '../terrain'

interface Stage00WaterProps {
  data: Stage00WaterPrototype
}

function NaturalStream({ watercourse }: { watercourse: NaturalWatercoursePrototype }) {
  const curve = new CatmullRomCurve3(
    watercourse.path.map((point) => {
      const [x, z] = toScenePosition(point)
      return new Vector3(x, 0.04, z)
    }),
  )

  return (
    <mesh name={watercourse.id} receiveShadow>
      <tubeGeometry args={[curve, 80, 0.13, 8, false]} />
      <meshStandardMaterial
        color="#5c918e"
        metalness={0.08}
        roughness={0.3}
        transparent
        opacity={0.9}
      />
    </mesh>
  )
}

export function Stage00Water({ data }: Stage00WaterProps) {
  return (
    <group name={data.id}>
      {data.watercourses.map((watercourse) => (
        <NaturalStream key={watercourse.id} watercourse={watercourse} />
      ))}
    </group>
  )
}
