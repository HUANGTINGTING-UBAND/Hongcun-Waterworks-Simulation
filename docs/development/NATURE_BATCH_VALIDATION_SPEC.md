# Nature Batch Validation Specification

## 1. Acceptance principle

Every Nature asset is validated independently. Successful Blender export is not approval. An asset reaches `approved` only after file, structure, transform, material, geometry, registry, and archive checks pass and a validation report records the evidence.

Validation is read-only against the candidate GLB. Failed candidates are not renamed into approved paths and are not added to the runtime registry as approved.

## 2. Required validation record

Each report records:

- asset ID and display name;
- candidate GLB path;
- source package/asset reference;
- exporter/generator and glTF version;
- validation date and tool/version;
- exact file size and SHA-256;
- node, mesh, primitive, material, texture, and LOD results;
- transform, Pivot, bounds, and grounding results;
- class-specific geometry/material checks;
- final `pass`, `pass_with_advisory`, or `fail` verdict.

## 3. File gate

| Check | Pass condition |
|---|---|
| GLB exists | Candidate path is a regular non-empty file |
| glTF 2.0 valid | GLB header/version/declared length, JSON chunk, BIN references, accessors, and indices parse without error |
| SHA-256 recorded | 64-character lowercase hash appears in validation report and processed manifest/registry |
| Size recorded | Exact byte size appears in report and registry |
| Reproducible identity | Hash and size match every archive record |

## 4. Structure gate

Validate and report:

- scene count and selected default scene;
- complete node list and root-node relationships;
- required stable asset/LOD node names;
- mesh count and primitive count;
- unexpected cameras, lights, preview planes, source references, or empty leaf nodes;
- collision node presence only where policy requires it.

LOD requirements:

| Class | Required LOD |
|---|---|
| Tree | LOD0, LOD1, LOD2; far impostor is a later placement gate when not embedded |
| Bush/fern/plant/grass/flower | LOD0, LOD1; optional reviewed LOD2/card |
| Medium rock | LOD0, LOD1, LOD2 |
| Pebble | LOD0, LOD1; distant cull/cluster policy |

Generic Blender suffixes such as `.001` are prohibited on published node names. If mesh datablock names differ from node names, record an advisory and ensure runtime lookup is explicitly node-based.

## 5. Transform and Pivot gate

For every exported render and collision node:

- Scale must equal `(1, 1, 1)` within `1e-6`.
- Translation must equal `(0, 0, 0)` for root asset/LOD nodes unless an approved manifest documents an offset.
- Rotation must be identity unless an approved coordinate-conversion wrapper is explicitly documented.
- No negative scale or mirrored winding is allowed.
- Pivot must be at the class support origin: tree root centre, plant stem/base centre, or rock support-plane centre.
- Bounds must be finite and plausible in metres.

Blender glTF export is Y-Up: source Blender Z maps to GLB Y. Grounding validation must use the converted source-up axis and must not misclassify horizontal negative GLB Z as underground penetration.

## 6. Material gate

All classes:

- every render primitive has an assigned valid material;
- required textures are embedded or resolve according to the approved packaging rule;
- no per-instance duplicated material;
- no missing image, invalid sampler, or accidental high-saturation fallback;
- material count remains within the class budget;
- material names are stable and recorded.

Alpha rule:

| Class/surface | Rule |
|---|---|
| Tree leaves | Alpha Clip allowed; bark must be opaque |
| Bush, fern, plant, grass, flower | Alpha Clip only where needed |
| Rock/pebble | Alpha prohibited; fully opaque |

Alpha Blend is not accepted for the baseline batch. Alpha Clip assets must record cutoff mode/value, confirm no bright edge fringe, and pass two-sided/backface and shadow-flicker review.

## 7. Geometry gate

### 7.1 Tree LOD budgets

| Asset | LOD0 max triangles | LOD1 max | LOD2 max |
|---|---:|---:|---:|
| `CommonTree_1` | 6,500 | 3,200 | 1,200 |
| `CommonTree_3` | 4,000 | 2,000 | 800 |
| `CommonTree_5` | 3,500 | 1,800 | 700 |

Tree validation also checks canopy/trunk silhouette continuity, bark/leaf primitive separation, no detached floating branch cards, grounding, and progressive LOD reduction. Triangle compliance alone does not approve excessive alpha overdraw.

### 7.2 Plant rules

- Alpha Clip only; no Alpha Blend.
- LOD0/LOD1 counts remain within `NATURE_ASSET_PROCESSING_SPEC.md` budgets.
- LOD reduction must remove hidden overlap before destroying the visible silhouette.
- No collision unless explicitly required by gameplay.
- Check card orientation, terrain bury margin, two-sided state, overdraw, and distant cull/aggregation policy.

### 7.3 Rock rules

- No alpha channel or transparent blend mode.
- Normals/winding are valid and no accidental hidden duplicate shell remains.
- LOD count stays within the per-asset budget.
- Medium-rock collision uses an approved simple proxy, never render-mesh collision.
- Pebbles default to no collision and use cull/cluster policy.
- Pivot and support plane must place the asset at source Blender `Z = 0` without visible underground penetration.

## 8. Runtime registry gate

Before approval, the proposed `nature_runtime_registry.json` entry must match validated evidence:

- processed path, exact byte size, and SHA-256;
- required node names and triangle counts;
- material names;
- collision and instancing policy;
- validation report path;
- status `validated` before archive and `approved` only after commit;
- exact archive commit after Git archive completes.

## 9. Batch failure behavior

If any check fails:

1. Stop that asset's pipeline.
2. Do not modify already approved GLBs.
3. Preserve failure evidence outside the approved archive scope.
4. Do not update status to `approved`.
5. Do not continue automatically to the next queued asset when the failure suggests a shared script/material/schema defect.

## 10. CommonTree_A reference result

`Hongcun_Tree_Common_A_v002.glb` is the reference passing asset:

- LOD triangles: `6265 / 3132 / 1127`;
- materials: bark and tree foliage;
- transforms: identity;
- source grounding: `Z = 0`;
- hash and complete evidence: `HONGCUN_TREE_COMMON_A_GLB_VALIDATION.md`.

Its internal mesh names are generic, but required LOD names are stable on nodes. Future runtime loaders must remain node-based unless a later migration explicitly renames mesh datablocks and revalidates the asset.
