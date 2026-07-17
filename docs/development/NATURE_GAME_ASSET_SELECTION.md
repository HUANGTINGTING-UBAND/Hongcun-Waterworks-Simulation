# Nature Game Asset Selection

## Selection objective

Milestone 008-D selects only the Quaternius assets that have a clear role in the Hongcun game composition. The full third-party package remains in local/cloud storage. No source model, texture, Blender file, forest scene, or processed prefab is added by this milestone.

Selection manifest: `world/assets/nature/manifests/nature_game_asset_selection.json`  
Source archive SHA-256: `298f6732b872e4cf7b30e6e7abf9641c7f6dc6b326df37ac089533ed7e3d58c9`

All candidates remain D-level stylized game assets. Their names and visual forms are not claims about real Hongcun species, ecology, geology, seasonality, or historical vegetation.

## Selection summary

| Category | Primary | Secondary | Reject |
|---|---:|---:|---:|
| mountain_tree | 2 | 2 | 13 |
| valley_tree | 3 | 1 | 0 |
| riverside_plant | 4 | 3 | 0 |
| ground_cover | 3 | 2 | 1 |
| rock | 5 | 5 | 14 |
| decorative_flower | 2 | 3 | 5 |
| **Total** | **19** | **16** | **33** |

Every one of the 68 reviewed source assets has exactly one selection decision. The rock count includes the ten disabled RockPath terrain props.

## Primary assets recommended for game production

Primary means “eligible for processing,” not “ready to commit.” Only processed, validated, game-used prefabs and their portable textures should later enter GitHub.

### mountain_tree

- `CommonTree_1`
- `CommonTree_2`

### valley_tree

- `CommonTree_3`
- `CommonTree_5`
- `Bush_Common`

### riverside_plant

- `Fern_1`
- `Grass_Common_Tall`
- `Plant_1`
- `Plant_7`

### ground_cover

- `Clover_1`
- `Grass_Common_Short`
- `Grass_Wispy_Short`

### rock

- `Pebble_Round_1`
- `Pebble_Round_2`
- `Rock_Medium_1`
- `Rock_Medium_2`
- `Rock_Medium_3`

### decorative_flower

- `Flower_3_Single`
- `Flower_4_Single`

These 19 unique Primary assets form the maximum approved production pool. The JSON manifest is authoritative if a summary count differs.

## Secondary reserve assets

Secondary assets remain in the original cloud package and are not part of the first processing pass:

- Mountain: `DeadTree_1`, `TwistedTree_1`
- Valley: `CommonTree_4`
- Riverside: `Grass_Wispy_Tall`, `Plant_1_Big`, `Plant_7_Big`
- Ground cover: `Clover_2`, `Mushroom_Common`
- Rock: `Pebble_Round_3`, `Pebble_Round_4`, `Pebble_Round_5`, `Pebble_Square_1`, `Pebble_Square_2`
- Flowers: `Bush_Common_Flowers`, `Flower_3_Group`, `Flower_4_Group`

Secondary assets should only move into production after the first batch proves material consistency, LOD cost, alpha-overdraw limits, and protected-view compatibility.

## Reject assets retained only in the cloud package

Reject means “not used in the current game archive,” not “delete from the source ZIP.”

- `DeadTree_2` through `DeadTree_5`: redundant dead-tree variants.
- `Pine_1` through `Pine_5`: disabled until ecological suitability is reviewed.
- `TwistedTree_2` through `TwistedTree_5`: expensive redundant hero-tree variants.
- `Mushroom_Laetiporus`: ecology-specific and relatively expensive.
- `Pebble_Square_3` through `Pebble_Square_6`: redundant angular variants with paving-conflict risk.
- All ten `RockPath_*` assets: prohibited as substitutes for historic Hongcun paving.
- `Petal_1` through `Petal_5`: effect props, not static vegetation.

Rejected files remain available only inside the unchanged original ZIP or cloud archive. They should not be extracted into the project or uploaded to GitHub.

## Recommended GitHub asset policy

Recommend entering GitHub later:

- This selection JSON and its documentation.
- Only the processed GLB prefabs that are actually used by the game.
- Only portable, optimized texture files required by those prefabs.
- Processing manifests containing source hash, license, scale, pivot, materials, LOD, and output hash.
- Small validation previews.

Do not enter GitHub:

- The 99.27 MiB original ZIP.
- Unselected FBX, Unity FBX, OBJ/MTL, glTF/BIN duplicates.
- Secondary and Reject source files that have no current game use.
- Temporary Blender imports, texture-conversion caches, or forest-scattering tests.

The complete original ZIP, bundled license, and checksum should remain in local/cloud archival storage.

## First processing batch: 14 assets

The first batch intentionally covers every visual layer while keeping scope small:

| Order | Asset | Category | First-pass purpose |
|---:|---|---|---|
| 1 | `CommonTree_1` | mountain_tree | Establish mountain canopy material and LOD pipeline |
| 2 | `CommonTree_3` | valley_tree | Validate open-valley silhouette and warmer foliage |
| 3 | `CommonTree_5` | valley_tree | Test lower-cost village-edge tree repetition |
| 4 | `Bush_Common` | valley_tree | Validate low-height woody transition near village outskirts |
| 5 | `Fern_1` | riverside_plant | Establish understory/wet-edge alpha workflow |
| 6 | `Plant_1` | riverside_plant | Test very low-cost wetland placeholder instancing |
| 7 | `Grass_Common_Tall` | riverside_plant | Validate water-edge scale, alpha, and wind pivot |
| 8 | `Grass_Common_Short` | ground_cover | Baseline broad ground-cover instancing asset |
| 9 | `Grass_Wispy_Short` | ground_cover | Test softer grass variation and overdraw budget |
| 10 | `Rock_Medium_1` | rock | Establish cyan-gray mountain rock material |
| 11 | `Rock_Medium_2` | rock | Validate efficient rock variation and slope placement |
| 12 | `Pebble_Round_1` | rock | Test small bank/mountain detail without historic-paving use |
| 13 | `Flower_3_Single` | decorative_flower | Establish restrained flower saturation and seasonality review |
| 14 | `Flower_4_Single` | decorative_flower | Validate a second controlled accent silhouette |

## Processing gates before any game asset commit

Every first-batch asset must pass:

1. Canonical source extraction from glTF/BIN only; do not copy all four format variants.
2. Metre-scale and pivot verification in an isolated Blender processing file.
3. Gray-green, gray-brown, or cyan-gray material adaptation according to category.
4. Portable texture relinking and color-space validation.
5. Alpha clip, double-sided, shadow, and overdraw review for foliage.
6. LOD0/LOD1/LOD2 and impostor review for trees; appropriate simplified LOD for other repeated assets.
7. GLB re-export and bounds/hash verification.
8. Neutral and project-lighting previews, including a white-wall/black-tile conflict test.
9. Runtime instancing and performance validation before any forest or large-scale scattering.

No automatic forest placement or final scene generation is authorized by this selection milestone.
