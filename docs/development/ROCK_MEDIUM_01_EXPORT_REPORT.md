# Rock Medium 01 GLB Export Report

## Result

Milestone 009-G completed successfully. The first Rock Runtime visual GLB was exported from the validated `NAT_ROCK_MEDIUM_01` clean source using Blender 4.2.22 LTS and the Khronos Blender glTF exporter.

The GLB contains only `NAT_ROCK_MEDIUM_01_LOD0` and `NAT_ROCK_MEDIUM_01_LOD1`. The independent collision object `NAT_ROCK_MEDIUM_01_COL` remains recorded in the source Blend and was intentionally excluded from the visual GLB.

No Runtime Registry entry was created or modified. No Tree asset or Git state was modified.

This remains a D-level game-art asset. It is not surveyed Hongcun geology, GIS output, historical stone evidence, or a reconstruction of real terrain or riverbed material.

## Input and output

| Field | Value |
|---|---|
| Input Blend | `world/assets/nature/source/rocks/hongcun_rock_medium_01_clean_v001.blend` |
| Output GLB | `world/assets/nature/processed/rocks/hongcun_rock_medium_01_v001.glb` |
| Runtime ID | `NAT_ROCK_MEDIUM_01` |
| Export source collection | `NAT_ROCK_MEDIUM_01` |
| Blender version | `4.2.22 LTS` |
| glTF exporter | `Khronos glTF Blender I/O v4.2.83` |
| Export timestamp | `2026-07-18T10:25:20+0800` |
| Export result | `FINISHED` |

## Source Blend immutability check

| Check | Before export | After export | Result |
|---|---|---|---|
| SHA-256 | `63dc965bda15f6550194bc503274ce1432e587ded3cbcdb7c0c80562a8681274` | `63dc965bda15f6550194bc503274ce1432e587ded3cbcdb7c0c80562a8681274` | Pass |
| File size | 3,079,364 bytes | 3,079,364 bytes | Pass |
| Blender unsaved changes after export | — | `False` | Pass |

The exporter did not save or mutate the source Blend. The before/after SHA-256 is identical.

## Export scope

The visual export selection contained:

```text
NAT_ROCK_MEDIUM_01_LOD0
NAT_ROCK_MEDIUM_01_LOD1
```

Explicitly excluded:

```text
NAT_ROCK_MEDIUM_01_COL
```

Collision record:

- source object exists in the Blend;
- source role is `simple_convex_hull`;
- source object has `hide_render = true`;
- source object was not selected for export;
- GLB node audit confirms collision is absent.

No cameras, lights, preview helpers, source-reference objects, animation, or LOD2 were exported.

## Export settings

Key settings:

```text
Format: glTF 2.0 Binary (GLB)
Selection only: true
Renderable filter: true
Y-up conversion: true
Apply additional transforms: false
Materials: export
Textures: included in GLB
Cameras: false
Lights: false
Animations: false
Extras: true
```

The object selection was built explicitly from the approved visual nodes. Collision exclusion did not depend only on viewport state.

## GLB file record

| Field | Value |
|---|---|
| File size | 2,542,280 bytes |
| SHA-256 | `635c527039c2a4a500606910238a249329bd11aa68b6b12f945ef48cd04c6feb` |
| GLB magic | `glTF` |
| Container version | `2` |
| Declared length | 2,542,280 bytes |
| Actual length | 2,542,280 bytes |
| JSON chunk | Present, 3,016 bytes |
| BIN chunk | Present, 2,539,236 bytes |
| glTF asset version | `2.0` |

Container length and chunks are internally consistent.

## Scene and node validation

| Check | Result |
|---|---|
| Scene count | 1 |
| Default scene | 0 |
| Node count | 2 |
| Unexpected nodes | 0 |
| Collision node present | No |

Nodes:

| Node | Mesh | Translation | Rotation | Scale |
|---|---:|---|---|---|
| `NAT_ROCK_MEDIUM_01_LOD0` | 0 | `(0,0,0)` | `(0,0,0,1)` | `(1,1,1)` |
| `NAT_ROCK_MEDIUM_01_LOD1` | 1 | `(0,0,0)` | `(0,0,0,1)` | `(1,1,1)` |

Both Runtime nodes preserve identity transforms.

## Mesh and LOD validation

| Mesh | Primitives | Triangles | Material index | Result |
|---|---:|---:|---:|---|
| `NAT_ROCK_MEDIUM_01_LOD0` | 1 | 342 | 0 | Pass |
| `NAT_ROCK_MEDIUM_01_LOD1` | 1 | 188 | 0 | Pass |

Validation points:

- mesh names match Runtime node names;
- LOD0 retains the required 342 triangles;
- LOD1 retains the approved 188 triangles;
- LOD triangle counts decrease monotonically;
- LOD2 is absent as decided in Milestone 009-F;
- both meshes use the same material.

## Material validation

| Field | Value | Result |
|---|---|---|
| Material count | 1 | Pass |
| Name | `Hongcun_Rock_Medium_01_Material` | Pass |
| glTF `alphaMode` | `OPAQUE` | Pass |
| Primitive assignment | Both LOD meshes use material 0 | Pass |
| `doubleSided` | `true` | Advisory |

The authoritative GLB material is OPAQUE, resolving the Blender 4.2 viewport-enum advisory recorded during clean-source preparation.

`doubleSided=true` is not a blocker for this milestone because opacity and material assignment are correct. It is a performance advisory: a future visual/performance review should determine whether the closed rock mesh can safely use single-sided rendering. Any change would require a new source revision, export, hash, and validation rather than silent binary modification.

## Texture validation

| Field | Value | Result |
|---|---|---|
| Texture count | 1 | Pass |
| Image count | 1 | Pass |
| Image name | `Rocks_Diffuse` | Pass |
| MIME type | `image/png` | Pass |
| External URI | None | Pass |
| BIN `bufferView` | 4 | Pass |
| Embedded | Yes | Pass |

The final GLB has no external texture dependency and no missing image URI.

## Validation summary

| Area | Status |
|---|---|
| glTF 2.0 container | Pass |
| Scene | Pass |
| Runtime nodes | Pass |
| Meshes and primitives | Pass |
| LOD names/counts/order | Pass |
| Identity transforms | Pass |
| OPAQUE material | Pass |
| Embedded texture | Pass |
| Collision exclusion | Pass |
| Source Blend hash unchanged | Pass |
| Double-sided performance state | Advisory |

Export status: **PASS WITH NON-BLOCKING PERFORMANCE ADVISORY**.

## Not performed

- no Registry integration;
- no Runtime loader integration;
- no collision GLB or physics test;
- no world placement or biome scattering;
- no Tree asset modification;
- no Git add, commit, or push.

The next safe milestone is a dedicated read-only binary validation using the Nature Validator, followed by Registry integration only if that validation passes.
