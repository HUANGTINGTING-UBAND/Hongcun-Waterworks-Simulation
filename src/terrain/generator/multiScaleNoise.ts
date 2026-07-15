import type { TerrainNoiseLayer } from '../../types'
import { lerp, smoothstep } from './math'

function hashGrid(x: number, z: number, seed: number): number {
  let value = Math.imul(x, 374761393) + Math.imul(z, 668265263)
  value = Math.imul(value ^ (value >>> 13) ^ seed, 1274126177)
  return ((value ^ (value >>> 16)) >>> 0) / 4294967295
}

function valueNoise(x: number, z: number, seed: number): number {
  const x0 = Math.floor(x)
  const z0 = Math.floor(z)
  const tx = smoothstep(0, 1, x - x0)
  const tz = smoothstep(0, 1, z - z0)
  const top = lerp(hashGrid(x0, z0, seed), hashGrid(x0 + 1, z0, seed), tx)
  const bottom = lerp(
    hashGrid(x0, z0 + 1, seed),
    hashGrid(x0 + 1, z0 + 1, seed),
    tx,
  )

  return lerp(top, bottom, tz) * 2 - 1
}

export function sampleMultiScaleNoise(
  x: number,
  z: number,
  seed: number,
  layers: TerrainNoiseLayer[],
): number {
  return layers.reduce(
    (height, layer, index) =>
      height +
      valueNoise(
        (x + 1.31) * layer.frequency,
        (z - 0.73) * layer.frequency,
        seed + index * 101,
      ) *
        layer.amplitude,
    0,
  )
}
