# Rock Medium 01 GLB Validation

## Validation result

Final verdict: **PASS WITH NON-BLOCKING ADVISORIES**.

The visual GLB parses as a complete glTF 2.0 Binary asset. It contains exactly the required LOD0 and LOD1 Runtime nodes, preserves identity transforms, is grounded on the glTF Y-up ground plane, uses one OPAQUE material with one embedded PNG texture, and excludes the source collision proxy.

This validation was read-only. The GLB, Blend, Runtime Registry, and Git state were not modified.

This remains a D-level game-art asset. Technical validation does not make it surveyed Hongcun geology, GIS output, historical evidence, or a reconstruction of a real slope, bank, or riverbed.

## Validation record

| Field | Value |
|---|---|
| Asset ID | `NAT_ROCK_MEDIUM_01` |
| Source file | `world/assets/nature/processed/rocks/hongcun_rock_medium_01_v001.glb` |
| Validation timestamp | `2026-07-18T10:30:12.839089+08:00` |
| File size | 2,542,280 bytes |
| SHA-256 | `635c527039c2a4a500606910238a249329bd11aa68b6b12f945ef48cd04c6feb` |
| Validation method | Direct read-only GLB container, JSON, BIN, accessor, and POSITION decoding |
| Reference export report | `docs/development/ROCK_MEDIUM_01_EXPORT_REPORT.md` |

The observed file size and SHA-256 match the export report.

## Container validation

| Check | Observed | Result |
|---|---|---|
| Magic | `glTF` | Pass |
| Container version | `2` | Pass |
| glTF asset version | `2.0` | Pass |
| Generator | `Khronos glTF Blender I/O v4.2.83` | Recorded |
| Declared length | 2,542,280 bytes | Pass |
| Actual length | 2,542,280 bytes | Pass |
| Parsed end offset | 2,542,280 bytes | Pass |
| JSON chunk | Present, 3,016 bytes | Pass |
| BIN chunk | Present, 2,539,236 bytes | Pass |

The declared length, actual file size, and final parsed chunk boundary are identical. Both required GLB chunks are present and parse successfully.

## Scene validation

| Check | Observed | Result |
|---|---:|---|
| Scene count | 1 | Pass |
| Default scene | 0 | Pass |
| Root scene nodes | `[0, 1]` | Pass |
| Node count | 2 | Pass |
| Mesh count | 2 | Pass |
| Unexpected nodes | 0 | Pass |

No camera, light, preview, source-reference, empty helper, collision, or LOD2 node is present.

## Runtime node validation

Required and observed nodes:

```text
NAT_ROCK_MEDIUM_01_LOD0
NAT_ROCK_MEDIUM_01_LOD1
```

| Node | Mesh index | Naming | Result |
|---|---:|---|---|
| `NAT_ROCK_MEDIUM_01_LOD0` | 0 | Matches `<RUNTIME_ID>_LOD0` | Pass |
| `NAT_ROCK_MEDIUM_01_LOD1` | 1 | Matches `<RUNTIME_ID>_LOD1` | Pass |

The base ID `NAT_ROCK_MEDIUM_01` follows `NAT_ROCK_<FORM>_<NN>` and contains no biome, scene, version, or approval-state token.

## Geometry and LOD validation

The Validator decoded index and POSITION accessors directly from the embedded BIN chunk.

| Mesh | Primitives | Vertices in GLB | Triangles | Expected triangles | Result |
|---|---:|---:|---:|---:|---|
| `NAT_ROCK_MEDIUM_01_LOD0` | 1 | 351 | 342 | 342 | Pass |
| `NAT_ROCK_MEDIUM_01_LOD1` | 1 | 232 | 188 | 188 | Pass |

LOD ordering:

```text
342 > 188
```

Result: Pass.

The Blender runtime-preparation report recorded 233 LOD1 vertices, while the exported accessor contains 232. Triangle count, node identity, material assignment, bounds intent, and grounding remain correct. This is recorded as an informational exporter vertex-reorganization advisory, not a geometry failure.

LOD2 is absent by approved design because its candidate silhouette did not pass the runtime-preparation stability gate.

## Transform validation

| Node | Translation | Rotation quaternion | Scale | Matrix override | Result |
|---|---|---|---|---|---|
| `NAT_ROCK_MEDIUM_01_LOD0` | `(0,0,0)` | `(0,0,0,1)` | `(1,1,1)` | None | Pass |
| `NAT_ROCK_MEDIUM_01_LOD1` | `(0,0,0)` | `(0,0,0,1)` | `(1,1,1)` | None | Pass |

Both Runtime nodes have identity transforms. No negative scale, translation offset, rotation wrapper, or matrix override is present.

## Grounding validation

Blender source Z-up becomes glTF Y-up. Grounding was therefore checked against decoded POSITION component Y, not glTF Z.

| Mesh | Decoded minimum XYZ | Decoded maximum XYZ | Minimum Y | Result |
|---|---|---|---:|---|
| LOD0 | `(-1.612574, 0.0, -1.494568)` | `(1.612574, 2.259797, 1.494568)` | `0.0` | Pass |
| LOD1 | `(-1.616177, 0.0, -1.494568)` | `(1.612574, 2.290454, 1.494568)` | `0.0` | Pass |

Both meshes touch the glTF ground plane exactly at `Y = 0.0`. No negative Y penetration is present.

Binary grounding confirms technical origin placement only; it does not approve any world location or imply a surveyed support surface.

## Material validation

| Check | Observed | Result |
|---|---|---|
| Material count | 1 | Pass |
| Material name | `Hongcun_Rock_Medium_01_Material` | Pass |
| LOD0 material index | 0 | Pass |
| LOD1 material index | 0 | Pass |
| glTF alpha mode | `OPAQUE` | Pass |
| Double-sided | `true` | Advisory |

Both mesh primitives share the same valid material. The authoritative exported material is OPAQUE; no alpha blend or alpha mask is present.

`doubleSided=true` is retained as a non-blocking performance advisory from the export report. A future reviewed source revision may test single-sided rendering, but this Validator does not modify or auto-fix the asset.

## Texture validation

| Check | Observed | Result |
|---|---|---|
| Texture count | 1 | Pass |
| Image count | 1 | Pass |
| Image name | `Rocks_Diffuse` | Pass |
| MIME type | `image/png` | Pass |
| Image bufferView | 4 | Pass |
| External URI | None | Pass |
| Missing external URI | 0 | Pass |
| Embedded in BIN | Yes | Pass |

The image references a BIN `bufferView` and has no URI. The visual GLB is self-contained with respect to its texture.

## Collision validation

Required visual-GLB behavior: collision must not be included.

| Check | Observed | Result |
|---|---|---|
| `NAT_ROCK_MEDIUM_01_COL` node present | No | Pass |
| Other collision-like node present | No | Pass |
| Unexpected node count | 0 | Pass |

The source Blend collision proxy remains a separately recorded production object and does not contaminate the visual Runtime GLB. This validation does not test physics behavior or create a collision asset.

## Anomaly and advisory audit

### Errors

None.

### Warnings

1. `MAT_DOUBLE_SIDED`: the opaque rock material exports with `doubleSided=true`. This is valid glTF and non-blocking, but may cost additional rendering work.

### Information

1. `MESH_VERTEX_REORGANIZATION`: LOD1 exports 232 POSITION vertices versus 233 Blender-source vertices. Triangles, bounds intent, grounding, and topology contract remain valid.
2. `LOD2_NOT_REQUIRED`: absence of LOD2 matches the approved Milestone 009-F decision.
3. `COLLISION_VISUAL_EXCLUSION`: collision absence is intentional; physics validation remains a separate future milestone.

## Check summary

| Area | Pass | Warning | Error |
|---|---:|---:|---:|
| Container | 7 | 0 | 0 |
| Scene/structure | 6 | 0 | 0 |
| Runtime naming | 2 | 0 | 0 |
| Geometry/LOD | 5 | 0 | 0 |
| Transform | 8 | 0 | 0 |
| Grounding | 2 | 0 | 0 |
| Material | 5 | 1 | 0 |
| Texture | 8 | 0 | 0 |
| Collision exclusion | 3 | 0 | 0 |
| **Total** | **46** | **1** | **0** |

## Safety statement

Allowed read-only actions performed:

- read GLB bytes;
- calculate size and SHA-256;
- parse container, JSON, and BIN chunks;
- decode indices and POSITION accessors;
- inspect Scene, Node, Mesh, Material, Texture, and transform data;
- create this Markdown validation report.

Not performed:

- no GLB rewrite, optimization, recompression, or re-export;
- no Blend open/save/modify operation;
- no Registry or manifest update;
- no Blender operation;
- no Git add, commit, or push;
- no automatic fix.

## Acceptance decision

`hongcun_rock_medium_01_v001.glb` is technically valid for the next Registry-integration review.

Validation status: **PASS WITH NON-BLOCKING ADVISORIES**.

Registry integration remains a separate milestone and must use the exact validated path, file size, SHA-256, node names, LOD triangle counts, OPAQUE material name, collision-exclusion policy, and this report path.
