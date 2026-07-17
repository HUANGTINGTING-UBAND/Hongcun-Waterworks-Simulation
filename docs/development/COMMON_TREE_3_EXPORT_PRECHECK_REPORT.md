# CommonTree_03 GLB Export Precheck Report

## Result

- Milestone: `008-L CommonTree_03 GLB Export Precheck`
- Date: `2026-07-17`
- Result: **PASS WITH MATERIAL ADVISORY**
- Inspection mode: read-only through the existing Blender GUI/MCP session
- Blender version: `4.2.22 LTS`
- GLB exported: no
- Blend saved or modified during inspection: no
- Git operations: none

Input:

```text
world/assets/nature/source/trees/hongcun_tree_common_03_clean_v001.blend
```

| Integrity field | Value |
|---|---|
| File size | `10,831,436` bytes |
| Expected SHA-256 | `7048dcf54d120b282fda5fa5cf2fb9bc208a3cde2e778ae667ebd3e227a2d343` |
| Observed SHA-256 | `7048dcf54d120b282fda5fa5cf2fb9bc208a3cde2e778ae667ebd3e227a2d343` |
| Hash match | Pass |
| Blender dirty state during audit | `false` |

## Export Collection

The `Export` Collection exists and contains exactly these three direct objects:

```text
Export
├── NAT_TREE_COMMON_03_LOD0
├── NAT_TREE_COMMON_03_LOD1
└── NAT_TREE_COMMON_03_LOD2
```

- Missing expected objects: none
- Unexpected objects: none
- Nested additional export objects: none
- Collection gate: **Pass**

## Runtime naming

| Node | Policy form | Result |
|---|---|---|
| `NAT_TREE_COMMON_03_LOD0` | `NAT_TREE_COMMON_03_LOD#` | Pass |
| `NAT_TREE_COMMON_03_LOD1` | `NAT_TREE_COMMON_03_LOD#` | Pass |
| `NAT_TREE_COMMON_03_LOD2` | `NAT_TREE_COMMON_03_LOD#` | Pass |

All export nodes use the approved runtime identity `NAT_TREE_COMMON_03`. No legacy Common B or Valley A naming is present in the Export Collection.

## Transform and geometry

| Node | Location | Rotation | Scale | Origin | Vertices | Triangles | Expected triangles | Result |
|---|---|---|---|---|---:|---:|---:|---|
| `NAT_TREE_COMMON_03_LOD0` | `(0,0,0)` | `(0,0,0)` | `(1,1,1)` | `(0,0,0)` | 5,749 | 3,505 | 3,505 | Pass |
| `NAT_TREE_COMMON_03_LOD1` | `(0,0,0)` | `(0,0,0)` | `(1,1,1)` | `(0,0,0)` | 4,214 | 1,947 | 1,947 | Pass |
| `NAT_TREE_COMMON_03_LOD2` | `(0,0,0)` | `(0,0,0)` | `(1,1,1)` | `(0,0,0)` | 3,188 | 905 | 905 | Pass |

All three nodes are Mesh objects, are direct members of `Export`, and have no unapplied modifiers.

## Materials and textures

Each LOD has exactly these two material slots:

- `Hongcun_Tree_Common_03_Bark`
- `Hongcun_Tree_Common_03_Leaf`

### Bark

| Check | Observed value | Result |
|---|---|---|
| Role | `bark_opaque` | Pass |
| Alpha linked | No | Pass |
| Alpha default | `1.0` | Pass |
| Alpha policy | `opaque_alpha_1` | Pass |
| Blender surface method | `DITHERED` | Recorded |
| Shadow policy | `material_derived_blender_4_2` | Recorded |
| Base-colour texture | `Bark_NormalTree` — packed | Pass |
| Normal texture | `Bark_NormalTree_Normal` — packed | Pass |

Although Blender reports the material surface method as `DITHERED`, Bark has no linked alpha input and its alpha value is `1.0`; it is therefore effectively opaque.

### Leaf

| Check | Observed value | Result |
|---|---|---|
| Role | `leaf_alpha` | Pass |
| Alpha linked | Yes | Pass |
| Alpha default | `1.0` | Recorded |
| Alpha policy | `dithered_source_pending_clip_validation` | Advisory |
| Blender surface method | `DITHERED` | Recorded |
| Shadow policy | `material_derived_blender_4_2` | Recorded |
| Colour/alpha texture | `Leaves_NormalTree_C` — packed | Pass |

All three texture images are packed in the Blend. Their original external paths remain stored as provenance metadata, but reopening the Blend does not depend on those external files.

## Pre-export decision

The file passes the requested structural GLB export precheck:

- source hash matches;
- Export Collection is exact;
- runtime node names comply with policy;
- transforms are identity;
- triangle counts match approved values;
- Bark and Leaf materials are assigned;
- all referenced textures are packed.

Material advisory: the Leaf material is currently `DITHERED`, and its own policy marks Alpha Clip validation as pending. This does not invalidate the structural precheck, but a future export/visual-validation milestone should explicitly inspect the resulting glTF alpha mode, foliage fringe, and shadow behavior before archive approval.

## Scope integrity

No Blend was saved or altered, no GLB was exported, CommonTree_A and CommonTree_5 were not processed, and no Git command was executed.
