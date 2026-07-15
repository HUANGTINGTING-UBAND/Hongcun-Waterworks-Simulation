import type { ZodType } from 'zod'
import { DataLoadError } from './DataLoadError'

export function parseData<T>(
  dataset: string,
  value: unknown,
  schema: ZodType<T>,
): T {
  const result = schema.safeParse(value)

  if (!result.success) {
    throw new DataLoadError(dataset, result.error)
  }

  return result.data
}
