# Nature Batch Production Plan

## 1. Purpose and boundary

This document defines a repeatable offline production pipeline for the approved Hongcun Nature asset batch. It is a production plan, not authorization to import, edit, export, scatter, or integrate any new asset.

Current milestone restrictions:

- Keep `Hongcun_Tree_Common_A_v002.glb` immutable.
- Do not process `CommonTree_3`, `CommonTree_5`, or any other queued asset in this milestone.
- Do not open Blender or modify Nature models.
- Do not modify Architecture, Terrain, GIS, React, or R3F files.
- Do not stage, commit, or push Git changes.
- Third-party source packages remain external/local or cloud-side; do not copy the full package into the repository.

## 2. Nature directory audit

Audited root:

```text
world/assets/nature/
```

| Required directory | Exists | Intended responsibility | Audit result |
|---|---|---|---|
| `source/` | Yes | Local editable workfiles and source references; not normal Git output | Pass |
| `processed/` | Yes | Approved game-facing GLB assets grouped by category | Pass |
| `materials/` | Yes | Shared material definitions and future processed material resources | Pass |
| `prefabs/` | Yes | Future reviewed Nature prefab assemblies; no scattering scenes | Pass |
| `previews/` | Yes | Neutral asset previews and review sheets | Pass |
| `manifests/` | Yes | Selection, provenance, rules, processing, and runtime registry records | Pass |
| `processing/` | Yes | Offline processing/export/validation tooling | Pass |

Existing category subdirectories include `source/trees/` and `processed/trees/`. The structure conforms to the existing Nature asset-management rules. No file or directory needs to be moved for batch preparation.

Audit advisories:

- `processed/trees/Hongcun_Tree_Common_A_v002.glb` is the approved immutable baseline.
- Earlier CommonTree_A export candidates and failed-attempt reports remain present locally; they must not be mistaken for approved output.
- `.blend` and `.blend1` files under `source/trees/` are local production sources/backups and are not approved runtime archive files.
- Empty category roots are acceptable until an asset in that category is explicitly approved for processing.

## 3. Completed production baseline

| Source asset | Runtime asset | Status | Archive commit |
|---|---|---|---|
| `CommonTree_1` | `world/assets/nature/processed/trees/Hongcun_Tree_Common_A_v002.glb` | Approved | `045619c` |

Baseline acceptance evidence:

- glTF 2.0 GLB container valid;
- LOD0, LOD1, and LOD2 nodes present;
- two materials assigned;
- identity transforms and origin Pivot;
- source Blender grounding at `Z = 0`;
- SHA-256 `2fb88ad035acdbd35ce5c681425c4a03f3234ecccf59d7e6104e2d6a2dbd03c9`;
- validation report: `docs/development/HONGCUN_TREE_COMMON_A_GLB_VALIDATION.md`.

CommonTree_A is the batch reference, not a file to duplicate blindly. Later assets must use their approved names, budgets, alpha rules, and collision policies.

## 4. Next Primary production queue

The queue is planning metadata only. Each asset requires a separate approved production milestone before work begins.

| Order | Source asset | Class | Planned game asset |
|---:|---|---|---|
| 1 | `CommonTree_3` | Tree | `Hongcun_Tree_Valley_A` |
| 2 | `CommonTree_5` | Tree | `Hongcun_Tree_Valley_B` |
| 3 | `Bush_Common` | Plant | `Hongcun_Bush_Common_A` |
| 4 | `Fern_1` | Plant | `Hongcun_Fern_Riverside_A` |
| 5 | `Plant_1` | Plant | `Hongcun_Plant_Riverside_A` |
| 6 | `Grass_Common_Tall` | Plant | `Hongcun_Grass_Riverside_Tall_A` |
| 7 | `Grass_Common_Short` | Plant | `Hongcun_Grass_Ground_Short_A` |
| 8 | `Grass_Wispy_Short` | Plant | `Hongcun_Grass_Ground_Wispy_A` |
| 9 | `Rock_Medium_1` | Rock | `Hongcun_Rock_Medium_A` |
| 10 | `Rock_Medium_2` | Rock | `Hongcun_Rock_Medium_B` |
| 11 | `Pebble_Round_1` | Rock | `Hongcun_Rock_Pebble_Round_A` |
| 12 | `Flower_3_Single` | Plant | `Hongcun_Flower_Decorative_A` |
| 13 | `Flower_4_Single` | Plant | `Hongcun_Flower_Decorative_B` |

Queue order follows the approved first-processing batch. It does not authorize automatic batch conversion; assets advance one review gate at a time.

## 5. Common production gates

Every asset moves through these state gates:

```text
selected
  → source_verified
  → clean_source_ready
  → processed_workfile_ready
  → export_candidate
  → content_validated
  → archive_checked
  → approved
```

Rules:

1. Record external source asset ID, package manifest reference, source hash, license, and attribution before processing.
2. Work on an isolated asset file; never open the terrain/world scene for asset conversion.
3. Use 1 Blender Unit = 1 metre, root/ground Pivot, identity transforms, and stable approved naming.
4. Apply only reviewed mesh/material/LOD changes. Do not add random geometry or unrecorded scale changes.
5. Export one asset prefab per GLB with approved LOD and collision nodes only.
6. Validate the binary independently from the source workfile.
7. Do not archive failed exports, source Blend files, or temporary previews.
8. Update the runtime registry only when validation evidence exists.

## 6. Class pipelines

### 6.1 Tree

```text
Source
→ Blender Clean Source
→ LOD
→ GLB
→ Validation
→ Archive
```

Required gates:

- Confirm trunk/leaf material separation and metre scale.
- Ground Pivot at the tree-root centre and source Blender `Z = 0`.
- Produce LOD0/LOD1/LOD2 within the per-asset triangle budget.
- Preserve canopy silhouette and validate leaf alpha clip/two-sided policy.
- Use simple collision proxy only when explicitly approved; never render-mesh collision.
- Validate protected sightline readability before world placement approval.

### 6.2 Plant

```text
Source
→ Material cleanup
→ Alpha Clip verification
→ LOD
→ GLB
→ Validation
```

This class includes bushes, fern, generic plants, grass, and flowers.

Required gates:

- Use alpha clip only where transparency is required; alpha blend is prohibited for the batch baseline.
- Verify cutoff stability, no bright fringe, no missing texture, and controlled two-sided rendering.
- Meet LOD0/LOD1 budgets and define far aggregation/cull behavior.
- Omit collision unless gameplay later provides explicit justification.
- Validate instance-safe shared materials and no per-instance material duplication.

### 6.3 Rock

```text
Source
→ Mesh cleanup
→ Collision check
→ LOD
→ GLB
→ Validation
```

Required gates:

- Remove invalid/hidden geometry without changing the approved silhouette.
- Do not use alpha materials.
- Medium rocks require simple proxy collision; pebbles normally use no collision.
- Produce required LODs within budget and verify manifold/normal quality as applicable.
- Ground the Pivot at the support plane and verify no negative source Z penetration.

## 7. Batch output contract

For an approved future asset `<game_asset>`:

```text
world/assets/nature/
├── processed/<category>/<game_asset>_v###.glb
├── previews/<category>/preview_<game_asset>_neutral.png
└── manifests/manifest_<game_asset>_v###.json

docs/development/
└── <GAME_ASSET>_GLB_VALIDATION.md
```

Only the GLB, validation evidence, approved manifest/registry update, preview when required, and governing production specification are Git archive candidates. See `NATURE_BATCH_COMMIT_RULES.md`.

## 8. Batch execution rule

Do not run a monolithic “process all” operation. A batch may share scripts and schemas, but each asset must produce an isolated log, hash, validation report, and approval decision. Failure of one asset stops that asset only and must not mutate or invalidate already approved outputs.
