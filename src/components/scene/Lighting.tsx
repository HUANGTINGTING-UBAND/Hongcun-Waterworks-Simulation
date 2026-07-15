export function SceneLighting() {
  return (
    <>
      <hemisphereLight args={['#dfe8df', '#6c6652', 1.35]} />
      <directionalLight
        castShadow
        color="#fff3d7"
        intensity={2.1}
        position={[6, 10, 4]}
        shadow-mapSize-height={1024}
        shadow-mapSize-width={1024}
      />
      <ambientLight color="#b8c5bd" intensity={0.45} />
    </>
  )
}
