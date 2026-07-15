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

const identifiedPrototypeSchema = stage00PrototypeMetadataSchema.extend({
  id: z.string().min(1),
})

const prototypeValueSchema = z.number().min(-1).max(1)
const positivePrototypeValueSchema = z.number().positive().max(1)
const prototypePointSchema = z.tuple([prototypeValueSchema, prototypeValueSchema])
const prototypeScaleSchema = z.tuple([
  positivePrototypeValueSchema,
  positivePrototypeValueSchema,
])

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

const prototypeSpaceSchema = identifiedPrototypeSchema.extend({
  type: z.literal('normalized_visual_space'),
  extent: prototypeScaleSchema,
  sceneSpan: z.number().positive().max(30),
  heightMode: z.literal('relative_visual_units'),
})

const noiseLayerSchema = identifiedPrototypeSchema.extend({
  type: z.enum([
    'large_scale_noise',
    'medium_scale_noise',
    'small_scale_noise',
  ]),
  frequency: z.number().positive().max(12),
  amplitude: z.number().min(0).max(0.5),
})

const heightFieldSchema = identifiedPrototypeSchema.extend({
  type: z.literal('procedural_height_field'),
  gridResolution: z.number().int().min(16).max(256),
  seed: z.number().int(),
  baseLevel: z.number().min(-0.25).max(0.5),
  visualHeightScale: z.number().positive().max(5),
  noiseLayers: z.array(noiseLayerSchema).length(3),
})

const landformSchema = identifiedPrototypeSchema.extend({
  type: z.enum(['distant_hills', 'foothill_slope', 'valley_plain']),
  position: prototypePointSchema,
  radius: prototypeScaleSchema,
  relativeHeight: z.number().min(0).max(1),
  transition: z.number().positive().max(4),
})

const surfaceZoneSchema = identifiedPrototypeSchema.extend({
  type: z.enum(['hill', 'foothill', 'valley', 'riparian']),
  color: z.string().regex(/^#[0-9a-fA-F]{6}$/),
})

export const stage00TerrainSchema = stage00PrototypeMetadataSchema.extend({
  id: z.string().min(1),
  type: z.literal('terrain_prototype'),
  stage: z.literal('stage_00_natural_hongcun'),
  coordinateSystem: z.literal('prototype_normalized_local'),
  disclaimer: z.string().min(1),
  space: prototypeSpaceSchema,
  heightField: heightFieldSchema,
  landforms: z.array(landformSchema).length(3),
  surfaceZones: z.array(surfaceZoneSchema).length(4),
})

const riverCorridorSchema = identifiedPrototypeSchema.extend({
  type: z.literal('visual_river_corridor'),
  surfaceWidth: positivePrototypeValueSchema,
  bedWidth: positivePrototypeValueSchema,
  bankWidth: positivePrototypeValueSchema,
  bedDepth: positivePrototypeValueSchema,
  surfaceOffset: positivePrototypeValueSchema,
}).refine(
  (corridor) =>
    corridor.surfaceWidth <= corridor.bedWidth &&
    corridor.bedWidth < corridor.bankWidth,
  { message: 'River widths must satisfy surfaceWidth <= bedWidth < bankWidth' },
)

const naturalWatercourseSchema = identifiedPrototypeSchema.extend({
  type: z.literal('natural_stream'),
  flowModel: z.literal('visual_direction_only'),
  color: z.string().regex(/^#[0-9a-fA-F]{6}$/),
  path: z.array(prototypePointSchema).min(4),
  corridor: riverCorridorSchema,
})

export const stage00WaterSchema = stage00PrototypeMetadataSchema.extend({
  id: z.string().min(1),
  type: z.literal('natural_water_prototype'),
  stage: z.literal('stage_00_natural_hongcun'),
  coordinateSystem: z.literal('prototype_normalized_local'),
  disclaimer: z.string().min(1),
  watercourses: z.array(naturalWatercourseSchema).min(1),
})
