import type { ZodError } from 'zod'

export class DataLoadError extends Error {
  readonly dataset: string
  readonly issues: ZodError['issues']

  constructor(dataset: string, error: ZodError) {
    const details = error.issues
      .map((issue) => `${issue.path.join('.') || 'root'}: ${issue.message}`)
      .join('; ')

    super(`Invalid data in ${dataset}: ${details}`)
    this.name = 'DataLoadError'
    this.dataset = dataset
    this.issues = error.issues
  }
}
