import { SceneCanvas } from './components/scene'
import { loadStage00Data } from './data'

const stage00 = loadStage00Data()

export function App() {
  return <SceneCanvas stage00={stage00} />
}
