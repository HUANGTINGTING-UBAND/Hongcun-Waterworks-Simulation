import { useFrame, useThree } from '@react-three/fiber'
import { useEffect, useRef } from 'react'
import { OrbitControls } from 'three/addons/controls/OrbitControls.js'

export function CameraController() {
  const { camera, gl } = useThree()
  const controlsRef = useRef<OrbitControls | null>(null)

  useEffect(() => {
    const controls = new OrbitControls(camera, gl.domElement)
    controls.enableDamping = true
    controls.dampingFactor = 0.07
    controls.enablePan = false
    controls.minDistance = 7
    controls.maxDistance = 24
    controls.minPolarAngle = Math.PI / 6
    controls.maxPolarAngle = Math.PI / 2.2
    controls.target.set(0, 0.45, 0)
    controls.update()
    controlsRef.current = controls

    return () => {
      controls.dispose()
      controlsRef.current = null
    }
  }, [camera, gl])

  useFrame(() => controlsRef.current?.update())

  return null
}
