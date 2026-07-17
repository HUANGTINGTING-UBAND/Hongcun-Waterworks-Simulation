# Hongcun Tree Common A GLB Validation

## Validation scope

- Milestone: `008-F1-Q2 CommonTree_A GLB Content Validation`
- Date: `2026-07-17`
- Mode: read-only binary GLB/glTF 2.0 inspection
- Asset modified: no
- Blend files opened or modified: no
- Git operations: none

Validated file:

```text
world/assets/nature/processed/trees/Hongcun_Tree_Common_A_v002.glb
```

## File integrity

| Check | Result |
|---|---|
| File exists | Pass |
| File size | `9,430,560` bytes (approximately 9.0 MiB) |
| SHA-256 | `2fb88ad035acdbd35ce5c681425c4a03f3234ecccf59d7e6104e2d6a2dbd03c9` |
| GLB magic | `glTF` |
| GLB version | `2` |
| Declared length matches file | Pass |
| JSON chunk | Present and valid JSON |
| BIN chunk | Present |
| Generator | `Khronos glTF Blender I/O v4.2.83` |
| Asset glTF version | `2.0` |

Result: the GLB parses normally and its binary container is internally consistent.

## Scene and node structure

- Scene count: **1**
- Scene name: `Hongcun_Tree_Common_A_Work`
- Node count: **3**
- Scene root nodes: `0`, `1`, `2`

Node list:

| Node | Mesh index | Translation | Rotation quaternion | Scale |
|---|---:|---|---|---|
| `NAT_MOUNTAIN_TREE_COMMON_A_LOD0` | 0 | `(0, 0, 0)` | `(0, 0, 0, 1)` | `(1, 1, 1)` |
| `NAT_MOUNTAIN_TREE_COMMON_A_LOD1` | 1 | `(0, 0, 0)` | `(0, 0, 0, 1)` | `(1, 1, 1)` |
| `NAT_MOUNTAIN_TREE_COMMON_A_LOD2` | 2 | `(0, 0, 0)` | `(0, 0, 0, 1)` | `(1, 1, 1)` |

Required node-name checks:

| Required node | Result |
|---|---|
| `NAT_MOUNTAIN_TREE_COMMON_A_LOD0` | Pass |
| `NAT_MOUNTAIN_TREE_COMMON_A_LOD1` | Pass |
| `NAT_MOUNTAIN_TREE_COMMON_A_LOD2` | Pass |

There are no non-mesh leaf nodes and no structural non-mesh nodes. No empty Object was exported.

## Mesh statistics

- Mesh count: **3**
- Each mesh contains two triangle primitives: bark and foliage.

| LOD node | GLB mesh name | Vertex entries | Triangles | Primitive breakdown |
|---|---|---:|---:|---|
| LOD0 | `tree.002` | 10,288 | 6,265 | Bark: 6,448 vertices / 4,345 triangles; foliage: 3,840 / 1,920 |
| LOD1 | `tree.003` | 6,535 | 3,132 | Bark: 2,695 vertices / 1,212 triangles; foliage: 3,840 / 1,920 |
| LOD2 | `tree.004` | 2,619 | 1,127 | Bark: 2,097 vertices / 869 triangles; foliage: 522 / 258 |

`Vertex entries` is the sum of POSITION accessor counts across each mesh's primitives. Separate primitive accessors may duplicate boundary vertices, so this is the correct GLB buffer-level count rather than a welded topology count.

The required asset names are preserved at the node/Object level. Internal mesh datablock names remain Blender-generated names (`tree.002`–`tree.004`); this does not prevent node-based LOD lookup but should be considered if runtime code expects mesh datablock names rather than node names.

## Materials

- Material count: **2**

| Material index | Name |
|---:|---|
| 0 | `Hongcun_Nature_Tree_Bark_Material` |
| 1 | `Hongcun_Nature_Tree_Material` |

All six mesh primitives reference a valid material. Missing-material primitive count: **0**.

## Transform, Pivot, and grounding

All three LOD nodes have:

- Location/translation `(0, 0, 0)`;
- identity rotation `(0, 0, 0, 1)`;
- Scale `(1, 1, 1)`.

This confirms that the exported Pivot/node origin remains at the asset origin and no transform offset was introduced.

Blender's glTF exporter writes Y-Up assets. Therefore source Blender Z corresponds to GLB Y; negative GLB Z values describe horizontal depth and are not underground penetration.

| LOD | GLB bounds min `(X,Y,Z)` | GLB bounds max `(X,Y,Z)` | Source Blender minimum Z via GLB Y |
|---|---|---|---:|
| LOD0 | `(-2.155711, 0.000000, -2.288857)` | `(2.155711, 7.264785, 2.288857)` | `0.000000` |
| LOD1 | `(-2.155711, 0.000000, -2.288857)` | `(2.155711, 7.264785, 2.288857)` | `0.000000` |
| LOD2 | `(-1.652158, 0.000000, -1.580101)` | `(1.602051, 7.213291, 1.929471)` | `0.000000` |

Grounding result: **Pass**. No vertex lies below source Blender `Z = 0` within the `1e-5` validation tolerance.

## Anomaly audit

| Audit item | Result | Notes |
|---|---|---|
| GLB parse failure | None | Header, JSON, and BIN chunks valid |
| Required LOD missing | None | LOD0/LOD1/LOD2 present |
| Empty Object | None | Every node references a mesh |
| Missing material | None | Every primitive has a valid material index |
| Non-unit scale | None | All nodes use `(1,1,1)` |
| Pivot/origin offset | None | All node translations are zero |
| Negative source Z penetration | None | Minimum source Z is exactly zero for all LODs |
| Internal mesh name mismatch | Advisory | Mesh datablocks use `tree.002`–`tree.004`; required names are correctly preserved on nodes |

## Final verdict

**Pass with one non-blocking naming advisory.**

`Hongcun_Tree_Common_A_v002.glb` is a valid glTF 2.0 binary asset containing one scene, three required LOD nodes, three meshes, and two fully assigned materials. Transforms are identity, Pivots remain at the origin, and all LODs are grounded at source Blender `Z = 0`. The only advisory is that internal mesh datablock names are generic while the required LOD names reside on the nodes; runtime loading should resolve the named nodes.
