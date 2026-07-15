import { z } from 'zod'

const historicalMetadataSchema = z.object({
  source: z.string().min(1),
  confidence: z.enum(['A', 'B', 'C', 'D']),
  gameAdjustment: z.boolean(),
})

const stage00PrototypeMetadataSchema = historicalMetadataSchema.extend({
  confidence: z.literal('D'),
  gameAdjustment: z.literal(true),
})

const prototypePointSchema = z.tuple([z.number(), z.number()])

export const historicalStageSchema = stage00PrototypeMetadataSchema.extend({
  id: z.string().min(1),
  name: z.string().min(1),
  type: z.literal('historical_stage'),
  description: z.string().min(1),
  features: z.array(z.string().min(1)),
  systems: z.object({
    water_system: z.boolean(),
    moon_pond: z.boolean(),
    south_lake: z.boolean(),
  }),
  player_goal: z.string().min(1),
})

const terrainFeatureSchema = historicalMetadataSchema.extend({
  id: z.string().min(1),
  type: z.enum(['mountain_mass', 'valley']),
  shape: z.string().min(1),
  position: prototypePointSchema,
  relativeHeight: z.number().min(0).max(1),
  relativeScale: prototypePointSchema,
})

export const stage00TerrainSchema = stage00PrototypeMetadataSchema.extend({
  id: z.string().min(1),
  type: z.literal('terrain_prototype'),
  stage: z.literal('stage_00_natural_hongcun'),
  coordinateSystem: z.literal('prototype_normalized_local'),
  disclaimer: z.string().min(1),
  features: z.array(terrainFeatureSchema).min(1),
})

const naturalWatercourseSchema = historicalMetadataSchema.extend({
  id: z.string().min(1),
  type: z.literal('natural_stream'),
  flowModel: z.literal('visual_direction_only'),
  path: z.array(prototypePointSchema).min(2),
})

export const stage00WaterSchema = stage00PrototypeMetadataSchema.extend({
  id: z.string().min(1),
  type: z.literal('natural_water_prototype'),
  stage: z.literal('stage_00_natural_hongcun'),
  coordinateSystem: z.literal('prototype_normalized_local'),
  disclaimer: z.string().min(1),
  watercourses: z.array(naturalWatercourseSchema).min(1),
})
