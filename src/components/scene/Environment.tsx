export function SceneEnvironment() {
  return (
    <>
      <color attach="background" args={['#dce2dc']} />
      <fog attach="fog" args={['#dce2dc', 12, 30]} />
    </>
  )
}
