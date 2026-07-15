import type { Stage00TerrainPrototype, TerrainPrototypeFeature } from '../types'
import { toScenePosition } from './prototypeCoordinates'

interface Stage00TerrainProps {
  data: Stage00TerrainPrototype
}

function MountainMass({ feature }: { feature: TerrainPrototypeFeature }) {
  const [x, z] = toScenePosition(feature.position)
  const width = feature.relativeScale[0] * 10
  const depth = feature.relativeScale[1] * 9
  const height = feature.relativeHeight * 3.2

  return (
    <mesh
      castShadow
      receiveShadow
      position={[x, height * 0.25 - 0.2, z]}
      rotation={[0, -0.22, 0]}
      scale={[width, height, depth]}
    >
      <sphereGeometry args={[0.5, 24, 12]} />
      <meshStandardMaterial color="#526b56" roughness={0.94} />
    </mesh>
  )
}

function ValleyFloor({ feature }: { feature: TerrainPrototypeFeature }) {
  const [x, z] = toScenePosition(feature.position)
  const width = feature.relativeScale[0] * 11
  const depth = feature.relativeScale[1] * 10

  return (
    <mesh
      receiveShadow
      position={[x, -0.06, z]}
      rotation={[-Math.PI / 2, 0, 0]}
      scale={[width, depth, 1]}
    >
      <circleGeometry args={[0.5, 48]} />
      <meshStandardMaterial color="#899071" roughness={1} />
    </mesh>
  )
}

export function Stage00Terrain({ data }: Stage00TerrainProps) {
  return (
    <group name={data.id}>
      <mesh receiveShadow rotation={[-Math.PI / 2, 0, 0]} position={[0, -0.1, 0]}>
        <circleGeometry args={[8, 64]} />
        <meshStandardMaterial color="#747b60" roughness={1} />
      </mesh>

      {data.features.map((feature) =>
        feature.type === 'mountain_mass' ? (
          <MountainMass key={feature.id} feature={feature} />
        ) : (
          <ValleyFloor key={feature.id} feature={feature} />
        ),
      )}
    </group>
  )
}
