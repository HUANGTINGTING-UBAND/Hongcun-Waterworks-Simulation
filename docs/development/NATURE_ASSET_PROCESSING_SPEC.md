# Nature Asset Processing Specification

## 1. Purpose and status

Version: Milestone 008-E  
Scope: first processing batch of 14 selected Quaternius nature assets  
Status: specification only; processing has not started

This document defines how the selected external assets may later become Hongcun game prefabs. It does not authorize Blender import, model modification, GLB export, forest generation, terrain integration, or Git operations in Milestone 008-E.

All selected assets remain D-level stylized game material. They are not evidence of real Hongcun species, ecology, geology, seasonality, or historical vegetation.

Authoritative inputs:

- `world/assets/nature/manifests/quaternius_nature_megakit_manifest.json`
- `world/assets/nature/manifests/nature_game_asset_selection.json`
- `world/assets/nature/manifests/nature_asset_rules.json`
- `docs/development/NATURE_STYLE_GUIDE.md`
- `docs/development/NATURE_BLENDER_IMPORT_SPEC.md`

## 2. Processing pipeline

```text
Verified external ZIP in local/cloud archive
        ↓
Extract one selected asset to a temporary source workspace
        ↓
Verify source hash, bundled CC0 record, and canonical glTF/BIN pair
        ↓
Import into an isolated Blender workfile
        ↓
Inspect unit, axis, dimensions, transform, pivot, mesh, UV, and normals
        ↓
Create a processed duplicate; never edit Source_Reference
        ↓
Convert material to the Hongcun low-saturation nature palette
        ↓
Produce required LOD and collision/culling policy
        ↓
Run neutral, project-lighting, alpha, and architecture-conflict previews
        ↓
Export one validated GLB prefab
        ↓
Re-open/parse GLB and record bounds, triangle counts, materials, textures, and SHA-256
        ↓
Only approved game-used outputs become Git candidates
```

### 2.1 Import

- Use `glTF + BIN` as the canonical source for all 14 assets.
- Use ordinary FBX only if the glTF import is demonstrably broken and the exception is recorded.
- Do not copy FBX, Unity FBX, OBJ/MTL, and glTF variants together into the project.
- Import each asset into an isolated file named `work_nature_<category>_<asset>_v###.blend`.
- Use the Collection structure from `NATURE_BLENDER_IMPORT_SPEC.md`: `Source_Reference`, `Processed`, `LOD`, `Collision`, `Preview`, and `Export`.
- Keep `Source_Reference` read-only, hidden from render, and excluded from export.
- Do not open terrain, village, architecture production, or forest-scattering scenes during asset processing.

### 2.2 Unit and axis checks

- Blender units: Metric, Unit Scale `1.0`.
- `1 Blender Unit = 1 metre`.
- Project axes: `+X` local right/east, `+Y` local forward/north, `+Z` up.
- Before applying any transform, record imported dimensions, rotation, scale, and source transform in the processed manifest.
- Validate dimensions against a metre ruler and a 1.7 m reference person; do not infer biological accuracy from asset names.
- Final export object Rotation must be `(0,0,0)` and Scale `(1,1,1)`.
- Any scale correction must be uniform and recorded as `sourceScale`, `processedScaleFactor`, and `scaleEvidence: game_visual_calibration`.

### 2.3 Pivot rules

| Asset type | Pivot rule |
|---|---|
| Tree and bush | Bottom centre of the stable trunk/root contact footprint |
| Fern, plant, grass, flower | Bottom centre of the foliage clump; suitable for terrain-normal alignment |
| Medium rock | Centre of the stable ground-contact polygon, not geometric volume centre |
| Pebble | Bottom centre of the intended resting orientation |

- Pivot local Z must be `0` at the intended contact plane.
- No visible mesh may remain below the contact plane unless it is an explicitly recorded bury margin.
- Allowed bury margin for vegetation: `0.01–0.04 m`; for rocks: `0.01–0.08 m`.
- Pivot changes are performed only on `Processed` copies and recorded as `sourcePivot` and `processedPivotRule`.
- Do not apply random pivot offsets per instance.

### 2.4 Scale rules

- Preserve uniform XYZ scale.
- Do not non-uniformly stretch trees, plants, flowers, grass, or rocks to create variants.
- Runtime variation must stay inside `nature_asset_rules.json` category ranges.
- Tree canopy and plant height are visual-game calibration values, not ecological measurements.
- Create explicit prefab variants only when a different physical size has a clear gameplay role and its own manifest entry.
- Final GLB bounds must match the approved Blender Export object within `0.001 m` per axis.

### 2.5 Material conversion

All runtime materials use Principled BSDF-compatible values and project-shared naming. Do not retain high-saturation source colours unchanged.

| Surface | Direction | Roughness direction | Notes |
|---|---|---|---|
| Mountain foliage | Deep cyan-gray/olive-gray green | Medium-high | Reduce contrast for fog-distance readability |
| Valley foliage | Slightly warmer and lighter olive green | Medium-high | Must not form a saturated frame around Hui architecture |
| Riverside foliage | Dark cyan-gray green | Medium; limited wet reduction | Wet does not mean mirror-like |
| Bark/wood | Gray-brown with low red content | High | Avoid bright orange/red bark |
| Rock/pebble | Cyan-gray or gray-brown | High | Optional controlled darker wet-edge variant |
| Flower | Strongly desaturated accent colour | Medium-high | Limit colour area and require seasonality review |

Material rules:

- Base Color uses sRGB; Normal, Roughness, Metalness, and masks use Non-Color.
- Verify normal-map Y orientation before accepting source normal maps.
- Foliage Alpha is allowed only as `alpha clip`; alpha blend is prohibited for the first pass.
- Double-sided rendering is allowed only for leaf/grass/flower cards that need it.
- Tree trunks and rocks remain opaque and single-sided unless a documented mesh defect requires correction.
- Record alpha threshold, double-sided state, texture dimensions, compression, and colour-space settings.
- Do not bake world fog, terrain coordinates, or random scene colour into an asset material.
- Web delivery should evaluate KTX2/Basis; do not automatically publish all original PNG files.

### 2.6 LOD production

- LOD budgets in this document are upper bounds, not targets that must be filled.
- LOD simplification must preserve silhouette, grounding, and main branch/rock volume.
- Trees require `LOD0`, `LOD1`, `LOD2`, and a far impostor before forest use.
- Bush, fern, plant, grass, and flower require at least `LOD0` and `LOD1`; far distance uses aggregation, terrain cover, or culling rather than individual meshes.
- Medium rocks require `LOD0`, `LOD1`, and `LOD2`.
- Pebble requires `LOD0` and `LOD1`; distant instances are culled or clustered.
- Runtime switch distances are not embedded in source assets and must be established through later performance testing.

### 2.7 GLB export

- Export only the `Export` Collection.
- Export one prefab per GLB, with approved LOD nodes only.
- Do not export `Source_Reference`, camera, light, ruler, preview plane, or test scene objects.
- Format: glTF 2.0 Binary (`.glb`).
- Apply only reviewed transforms/modifiers on the export duplicate; retain editable processed and source references in the workfile.
- Re-open or parse each GLB and verify node names, mesh counts, material names, textures, bounds, pivot, and LOD names.
- Calculate GLB SHA-256 and record it with source archive hash and processed-workfile hash.
- A GLB is not approved for Git merely because export succeeded; it must pass the full asset acceptance gate.

## 3. Naming specification

### 3.1 Stable game asset names

| Source asset | Game asset name | Object base name |
|---|---|---|
| `CommonTree_1` | `Hongcun_Tree_Common_A` | `NAT_MOUNTAIN_TREE_COMMON_A` |
| `CommonTree_3` | `Hongcun_Tree_Valley_A` | `NAT_VALLEY_TREE_COMMON_A` |
| `CommonTree_5` | `Hongcun_Tree_Valley_B` | `NAT_VALLEY_TREE_COMMON_B` |
| `Bush_Common` | `Hongcun_Bush_Common_A` | `NAT_VALLEY_BUSH_COMMON_A` |
| `Fern_1` | `Hongcun_Fern_Riverside_A` | `NAT_RIVERSIDE_FERN_A` |
| `Plant_1` | `Hongcun_Plant_Riverside_A` | `NAT_RIVERSIDE_PLANT_A` |
| `Grass_Common_Tall` | `Hongcun_Grass_Riverside_Tall_A` | `NAT_RIVERSIDE_GRASS_TALL_A` |
| `Grass_Common_Short` | `Hongcun_Grass_Ground_Short_A` | `NAT_GROUND_GRASS_SHORT_A` |
| `Grass_Wispy_Short` | `Hongcun_Grass_Ground_Wispy_A` | `NAT_GROUND_GRASS_WISPY_A` |
| `Rock_Medium_1` | `Hongcun_Rock_Medium_A` | `NAT_ROCK_MEDIUM_A` |
| `Rock_Medium_2` | `Hongcun_Rock_Medium_B` | `NAT_ROCK_MEDIUM_B` |
| `Pebble_Round_1` | `Hongcun_Rock_Pebble_Round_A` | `NAT_ROCK_PEBBLE_ROUND_A` |
| `Flower_3_Single` | `Hongcun_Flower_Decorative_A` | `NAT_DECORATIVE_FLOWER_A` |
| `Flower_4_Single` | `Hongcun_Flower_Decorative_B` | `NAT_DECORATIVE_FLOWER_B` |

### 3.2 File and node names

- Workfile: `work_nature_<category>_<game-name>_v###.blend`
- GLB: `nature_<category>_<game-name>_v###.glb`
- LOD object: `<OBJECT_BASE>_LOD0`, `_LOD1`, `_LOD2`
- Collision: `<OBJECT_BASE>_COL`
- Material: `MAT_NAT_<SURFACE>_<VARIANT>`
- Texture: `T_NAT_<SET>_<BC|N|R|M|A>_<SIZE>.<ext>`
- Preview: `preview_<game-name>_<neutral|project|alpha|lod|architecture>.png`
- Processed manifest: `manifest_<game-name>_v###.json`

Published filenames use lowercase ASCII and underscores. Blender object/material names use uppercase ASCII tokens. Spaces, Chinese characters, negative-scale variants, and `.001` suffixes are prohibited in published outputs.

## 4. Performance budgets

`Source triangles` are read-only review measurements from the Quaternius glTF accessors. `LOD0 max` is the processed game-prefab ceiling.

| Order | Asset | Source triangles | LOD0 max | LOD1 max | LOD2/Far policy | LOD required | Alpha allowed | Instancing allowed |
|---:|---|---:|---:|---:|---|---|---|---|
| 1 | `CommonTree_1` | 6,265 | 6,500 | 3,200 | LOD2 ≤1,200 + impostor | Yes: 0/1/2/Far | Yes, leaf alpha clip only | Yes |
| 2 | `CommonTree_3` | 3,505 | 4,000 | 2,000 | LOD2 ≤800 + impostor | Yes: 0/1/2/Far | Yes, leaf alpha clip only | Yes |
| 3 | `CommonTree_5` | 3,182 | 3,500 | 1,800 | LOD2 ≤700 + impostor | Yes: 0/1/2/Far | Yes, leaf alpha clip only | Yes |
| 4 | `Bush_Common` | 900 | 1,000 | 500 | Far aggregation/cull; optional LOD2 ≤200 | Yes: 0/1 | Yes, leaf alpha clip only | Yes |
| 5 | `Fern_1` | 288 | 350 | 180 | Far aggregation/cull | Yes: 0/1 | Yes, alpha clip | Yes |
| 6 | `Plant_1` | 120 | 150 | 80 | Far aggregation/cull | Yes: 0/1 | Yes, alpha clip | Yes |
| 7 | `Grass_Common_Tall` | 326 | 400 | 180 | Far ground-cover aggregation/cull | Yes: 0/1 | Yes, alpha clip | Yes |
| 8 | `Grass_Common_Short` | 155 | 200 | 100 | Far ground-cover aggregation/cull | Yes: 0/1 | Yes, alpha clip | Yes |
| 9 | `Grass_Wispy_Short` | 494 | 550 | 250 | Far ground-cover aggregation/cull | Yes: 0/1 | Yes, alpha clip | Yes |
| 10 | `Rock_Medium_1` | 342 | 400 | 220 | LOD2 ≤100 | Yes: 0/1/2 | No | Yes |
| 11 | `Rock_Medium_2` | 244 | 300 | 160 | LOD2 ≤80 | Yes: 0/1/2 | No | Yes |
| 12 | `Pebble_Round_1` | 136 | 200 | 100 | Distance cull or cluster | Yes: 0/1 | No | Yes |
| 13 | `Flower_3_Single` | 285 | 350 | 160 | Far aggregation/cull | Yes: 0/1 | Yes, alpha clip | Yes |
| 14 | `Flower_4_Single` | 642 | 700 | 300 | Far aggregation/cull; optional simplified card ≤100 | Yes: 0/1 | Yes, alpha clip | Yes |

Global performance rules:

- Triangle budget alone cannot approve foliage; alpha-overdraw, shadow cost, and material count must also pass.
- Repeated assets use GPU instancing and shared material/texture sets.
- Per-instance material duplication is prohibited.
- Trees should use no more than two runtime materials unless a reviewed atlas reduces them further.
- Other first-batch assets should target one runtime material; two are allowed for flowers only when foliage and flower colour cannot share one material safely.
- No asset may embed unique world-position or random-instance data into its material.
- Collision is omitted for grass, flowers, fern, and small plants unless gameplay later requires it.
- Tree and medium-rock collision must use simple proxy shapes, never render-mesh collision.

## 5. Asset-specific processing requirements

### Trees: `CommonTree_1`, `CommonTree_3`, `CommonTree_5`

- Preserve trunk and canopy silhouette; remove no major branch without before/after review.
- Validate bark/leaf material separation, leaf alpha edges, shadow flicker, and two-sided state.
- Produce LOD0/1/2 and far impostor.
- Prepare lower-contrast far material variation without baking scene fog.
- Test canopy against protected mountain-valley-water-village sightlines and a white-wall/black-tile reference block.

### Bush: `Bush_Common`

- Keep height below architecture silhouette clearance in village-outskirts tests.
- Review alpha overdraw and reduce overlapping cards in LOD1.
- Do not use as a real-species claim or hedge reconstruction.

### Fern and plant: `Fern_1`, `Plant_1`

- Treat as wet-edge/understory placeholders only.
- Verify terrain-normal alignment and bury margin.
- Use dark cyan-gray green; no glossy wet mirror effect.
- Confirm alpha clipping remains stable under blue-gray fog and soft sunlight.

### Grass: three assets

- Establish a shared grass texture/material where possible.
- Pivot must support future shader wind while keeping the base fixed.
- Test instance density and alpha overdraw before approving any scattering use.
- Tall grass remains outside canal-flow clearance and open water.

### Rocks: three assets

- Preserve stable resting orientation and ground-contact footprint.
- Recalculate normals after simplification; do not smooth away major planes.
- Convert to cyan-gray/gray-brown and keep high roughness.
- `Pebble_Round_1` is natural detail only and cannot substitute for historic paving.

### Flowers: two assets

- Strongly reduce source colour saturation and total colour area.
- Require seasonality review and protected-core exclusion.
- Validate alpha clip, double-sided state, shadow, and distance culling.
- Do not create decorative flower bands or theme-park-like planting.

## 6. Git and cloud archive rules

### GitHub candidates after processing approval

Only these outputs may be proposed for Git:

- Processed, game-used GLB prefab for each approved asset.
- Required optimized runtime textures only; no unused source textures.
- Processed manifest containing source/output SHA-256, unit, pivot, bounds, LOD, materials, textures, license, usage zones, and performance results.
- Small validation previews: neutral, project lighting, alpha/LOD, and architecture-conflict views.
- Processing specification and selection documents.

Do not commit workfiles or outputs merely because they exist. A later Git archive milestone must use an explicit whitelist.

### Cloud/local archive

Keep outside Git:

- Original `Stylized Nature MegaKit[Standard].zip`.
- Bundled original FBX, Unity FBX, OBJ/MTL, glTF/BIN, textures, previews, and license file.
- Original canonical-source extractions before processing.
- Blender source/import workfiles unless a later policy explicitly approves a small editable production source.
- Secondary and Reject assets.
- Temporary texture conversions, caches, test exports, and failed processing attempts.

The original ZIP remains byte-identical and is identified by SHA-256:

```text
298f6732b872e4cf7b30e6e7abf9641c7f6dc6b326df37ac089533ed7e3d58c9
```

## 7. First-batch processing order

The order is risk-gated and must be followed unless a later milestone records a change:

1. `CommonTree_1` — establish the most demanding tree material, LOD, impostor, and sightline pipeline.
2. `CommonTree_3` — validate valley-tree colour and medium-cost LOD reuse.
3. `CommonTree_5` — validate lower-cost village-edge repetition.
4. `Bush_Common` — validate low-height woody alpha and architecture clearance.
5. `Fern_1` — establish wet-edge/understory alpha and terrain-alignment rules.
6. `Plant_1` — validate the lowest-cost riverside placeholder and instancing.
7. `Grass_Common_Tall` — establish tall-grass alpha, wind pivot, and water-clearance checks.
8. `Grass_Common_Short` — establish baseline ground-cover instancing.
9. `Grass_Wispy_Short` — test softer grass variation and overdraw ceiling.
10. `Rock_Medium_1` — establish shared cyan-gray rock material and rock LOD rules.
11. `Rock_Medium_2` — verify material/LOD reuse on a second medium rock.
12. `Pebble_Round_1` — validate small detail grounding and historic-path exclusion.
13. `Flower_3_Single` — establish flower desaturation, alpha, and seasonality review.
14. `Flower_4_Single` — validate a second flower silhouette without expanding colour coverage.

Each item pauses at the prefab acceptance gate. Failure of scale, pivot, material, alpha, LOD, or manifest validation blocks the next asset in the same category from being promoted.

## 8. Required processed manifest fields

Every processed asset manifest must record:

- Source archive filename and SHA-256.
- Canonical source relative path and source-file SHA-256.
- CC0 license reference and recommended Quaternius credit.
- Source asset ID and stable Hongcun game asset name.
- Category, priority, allowed usage zones, forbidden zones, `confidence: D`, and `gameAdjustment: true`.
- Source and processed dimensions, bounds, axis, scale, pivot, bury margin, and transform.
- LOD vertex/triangle counts and simplification method.
- Material names, texture names, dimensions, colour spaces, alpha threshold, double-sided state, and compression.
- Collision or explicit `collision: none`.
- GLB path, size, SHA-256, parsed bounds, node count, mesh count, and material count.
- Preview paths and SHA-256.
- Runtime instancing, alpha-overdraw, shadow, and architecture-conflict review results.
- Approval status: `prototype`, `reviewed`, or `approved`.

## 9. Stop conditions

Stop processing and do not export when any of the following occurs:

- Source hash or license cannot be matched to the reviewed package.
- Scale is guessed without a recorded game-calibration decision.
- Pivot or ground contact remains unstable.
- Required textures are missing or use unresolved absolute paths.
- Alpha produces visible halos, sorting problems, excessive overdraw, or unstable shadows.
- LOD breaks the defining silhouette or causes floating geometry.
- Material remains overly saturated or conflicts with Hui white-wall/black-tile readability.
- The asset enters a forbidden zone or is presented as real ecological/historical reconstruction.

Milestone 008-E ends with this specification. No asset processing or Git operation is part of this milestone.
