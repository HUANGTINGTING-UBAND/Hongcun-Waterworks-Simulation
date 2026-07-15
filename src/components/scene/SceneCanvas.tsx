import { Canvas } from '@react-three/fiber'
import type { Stage00Data } from '../../data'
import { CameraController } from './CameraController'
import { SceneEnvironment } from './Environment'
import { SceneLighting } from './Lighting'
import { NaturalLandscape } from './NaturalLandscape'

interface SceneCanvasProps {
  stage00: Stage00Data
}

export function SceneCanvas({ stage00 }: SceneCanvasProps) {
  return (
    <Canvas
      camera={{
        fov: 42,
        near: 0.1,
        far: 100,
        position: [8, 11.3, 8],
      }}
      dpr={[1, 2]}
      gl={{ antialias: true }}
      shadows
    >
      <SceneEnvironment />
      <SceneLighting />
      <NaturalLandscape stage00={stage00} />
      <CameraController />
    </Canvas>
  )
}
