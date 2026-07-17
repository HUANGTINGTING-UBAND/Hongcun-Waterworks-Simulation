# CommonTree_03 GLB Binary Validation

## Result

- Milestone: `008-N CommonTree_03 GLB Binary Validation`
- Date: `2026-07-17`
- Status: **PASS**
- Validation method: read-only GLB 2.0 binary and JSON inspection
- Blender opened: no
- GLB or Blend modified: no
- Registry or Git operations: none

Target:

```text
world/assets/nature/processed/trees/hongcun_tree_common_03_v001.glb
```

| File field | Value | Result |
|---|---|---|
| File size | `9,046,344` bytes | Pass |
| SHA-256 | `63eeb2dde5ce7e80f1ff7eb2e0cb71514127de79ebee6ecdf80e9dcde5db4ee0` | Recorded |
| Magic | ASCII `glTF`, hex `676c5446` | Pass |
| Container version | `2` | Pass |
| Asset version | `2.0` | Pass |
| Declared length | `9,046,344` bytes | Pass |
| Actual length | `9,046,344` bytes | Pass |
| Generator | `Khronos glTF Blender I/O v4.2.83` | Recorded |

## Chunk structure

| Order | Chunk | Declared size | Result |
|---:|---|---:|---|
| 1 | `JSON` | 7,720 bytes | Present and parsed |
| 2 | `BIN\0` | 9,038,596 bytes | Present |

Chunk traversal ended exactly at byte `9,046,344`. There are no truncated bytes, trailing bytes, or declared-length mismatches.

## Scene and nodes

- Scene count: **1**
- Default scene: index `0`
- Scene name: `Hongcun_Tree_Common_03_Clean_Source`
- Node count: **3**
- Root nodes: `0`, `1`, `2`

| Node | Mesh | Translation | Rotation quaternion | Scale | Result |
|---|---:|---|---|---|---|
| `NAT_TREE_COMMON_03_LOD0` | 0 | `(0,0,0)` | `(0,0,0,1)` | `(1,1,1)` | Pass |
| `NAT_TREE_COMMON_03_LOD1` | 1 | `(0,0,0)` | `(0,0,0,1)` | `(1,1,1)` | Pass |
| `NAT_TREE_COMMON_03_LOD2` | 2 | `(0,0,0)` | `(0,0,0,1)` | `(1,1,1)` | Pass |

All required runtime nodes exist. None uses a matrix override, and no extra node is present.

## Mesh and geometry

Mesh count: **3**. Each Mesh contains two triangle-mode primitives: Bark and Leaf.

| Node / Mesh | GLB vertex-accessor total | Bark triangles | Leaf triangles | Total triangles | Result |
|---|---:|---:|---:|---:|---|
| `NAT_TREE_COMMON_03_LOD0` | 6,075 | 1,705 | 1,800 | 3,505 | Pass |
| `NAT_TREE_COMMON_03_LOD1` | 3,694 | 1,227 | 720 | 1,947 | Pass |
| `NAT_TREE_COMMON_03_LOD2` | 1,947 | 545 | 360 | 905 | Pass |

The GLB vertex values are the sums of the exported POSITION accessor counts across material primitives. They are not expected to equal Blender's source Mesh vertex counts because glTF export splits/rebuilds vertex streams at primitive, material, normal, UV, and other attribute boundaries. Triangle totals remain exactly equal to the approved source values.

Exported Mesh datablock names are:

- `NAT_TREE_COMMON_03_LOD0_MESH.001`
- `NAT_TREE_COMMON_03_LOD1_MESH.001`
- `NAT_TREE_COMMON_03_LOD2_MESH.001`

The `.001` suffix affects Mesh datablock names only. Required runtime Node names remain exact.

## Materials and assignment

Material count: **2**.

| Index | Material | glTF alpha mode | Double-sided | Texture roles | Result |
|---:|---|---|---|---|---|
| 0 | `Hongcun_Tree_Common_03_Bark` | `OPAQUE` | Yes | base color, normal | Pass |
| 1 | `Hongcun_Tree_Common_03_Leaf` | `MASK` | Yes | base color/alpha | Pass |

Every Mesh uses material index `0` on its Bark primitive and material index `1` on its Leaf primitive. No primitive has a missing or out-of-range material assignment.

The binary result resolves the Blender export-time Alpha advisory: Bark is encoded as opaque, while Leaf is encoded as masked alpha rather than blended transparency.

## Textures and images

- Texture count: **3**
- Image count: **3**
- Embedded image count: **3**
- External image URI count: **0**
- Missing texture count: **0**

| Image | MIME type | Storage | Role |
|---|---|---|---|
| `Bark_NormalTree_Normal` | `image/png` | Embedded BIN bufferView | Bark normal |
| `Bark_NormalTree` | `image/png` | Embedded BIN bufferView | Bark base color |
| `Leaves_NormalTree_C` | `image/png` | Embedded BIN bufferView | Leaf base color/alpha |

The GLB has no external texture dependency.

## Transform and grounding

Blender uses Z-up coordinates; glTF uses Y-up coordinates. The approved runtime source objects were previously recorded with Blender minimum Z `0.0`. After export conversion, the POSITION bounds are:

| LOD | glTF min Y | glTF max Y | Grounding |
|---|---:|---:|---|
| LOD0 | `0.0` | `9.425190` | Pass |
| LOD1 | `0.0` | `9.129370` | Pass |
| LOD2 | `0.0` | `8.690989` | Pass |

Node transforms are identity, so no translation or scale shifts these bounds. Blender source Z=0 therefore converts correctly to glTF Y=0 for all three LODs. Negative glTF Z values represent horizontal depth after axis conversion and are not below-ground penetration.

## Anomaly audit

| Check | Result | Notes |
|---|---|---|
| Invalid GLB magic/version | Pass | None found |
| Declared/actual length mismatch | Pass | Exact match |
| Missing JSON or BIN chunk | Pass | Both present |
| Missing required node | Pass | All three present |
| Extra scene node | Pass | None |
| Incorrect transform | Pass | All identity |
| Triangle-count mismatch | Pass | None |
| Missing material assignment | Pass | None |
| Missing/external texture | Pass | None |
| Grounding failure | Pass | All min Y = 0 |
| Mesh name suffix | Advisory | `.001` appears on Mesh datablocks, not runtime Nodes |
| Export vertex-count expansion | Expected | Caused by glTF primitive/attribute splitting |
| Extensions required | None | No extension dependency |

No blocking anomaly was found. The asset is structurally suitable for the next registry/archive preparation stage, subject to any separate visual alpha and shading review the project requires.

## Scope integrity

This validation did not open Blender, modify the GLB or any Blend, update the runtime registry, or execute Git commands.
