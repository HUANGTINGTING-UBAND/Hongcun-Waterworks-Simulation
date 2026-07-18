# Rock Medium 01 Runtime Preparation Report

## Result

Milestone 009-F completed using Blender 4.2.22 LTS through the connected GUI/MCP session. `NAT_ROCK_MEDIUM_01` now contains the unchanged source LOD0, an accepted LOD1, and an independent non-rendering simple collision proxy. An LOD2 candidate was evaluated and removed because its bounds deviation did not meet the silhouette-stability gate.

No GLB was exported. The Runtime Registry, Tree assets, and Git state were not modified.

This remains a D-level game-art asset. It is not surveyed Hongcun geology, historical stone evidence, GIS output, or a reconstruction of a real slope, bank, or riverbed.

## Input

| Field | Value |
|---|---|
| Source Blend | `world/assets/nature/source/rocks/hongcun_rock_medium_01_clean_v001.blend` |
| Runtime ID | `NAT_ROCK_MEDIUM_01` |
| Blender version | `4.2.22 LTS` |
| Input Blend SHA-256 | `0635398eff89c36462f9b9c6de0cc24cebbc5a1b496bcfd309df66e65575d43f` |
| Input size | 3,033,760 bytes |

The input hash matched `ROCK_MEDIUM_01_CLEAN_SOURCE_REPORT.md` before runtime preparation began.

## Final structure

```text
NAT_ROCK_MEDIUM_01
├── NAT_ROCK_MEDIUM_01_LOD0
├── NAT_ROCK_MEDIUM_01_LOD1
└── NAT_ROCK_MEDIUM_01_COL
```

`NAT_ROCK_MEDIUM_01_LOD2` is deliberately absent and recorded as `not required`.

## LOD decision

### LOD0

- Object: `NAT_ROCK_MEDIUM_01_LOD0`.
- Vertices: 351.
- Triangles: 342.
- Decision: retained without topology reduction.
- Result: exactly matches the required clean-source geometry count.

### LOD1

- Object: `NAT_ROCK_MEDIUM_01_LOD1`.
- Vertices: 233.
- Triangles: 188.
- Triangle reduction from LOD0: `45.03%`.
- Target requirement: approximately `40–60%` reduction.
- Maximum bounds deviation from LOD0: `1.36%`.
- Grounding: minimum Blender `Z = 0.0`.
- Decision: **accepted**.

LOD1 preserves the principal dimensions and support plane while meeting the requested reduction range. The reduction was applied and no Modifier remains on the saved object.

### LOD2 evaluation

A temporary candidate was evaluated:

- Candidate triangles: 95.
- Candidate maximum bounds deviation: `6.84%`.
- Candidate grounding: minimum Blender `Z = 0.0`.

Although the candidate met the numerical triangle budget, its horizontal bounds deviation exceeded the conservative silhouette-stability gate. Decision: **not required**. The candidate object and orphaned candidate mesh were removed before saving.

This avoids retaining a low-value LOD whose outline changes more than the asset's very low source triangle count justifies. Far-range handling should use the future placement/culling policy rather than an unstable LOD2.

## Triangle and mesh statistics

| Object | Vertices | Triangles | Material slots | Unapplied Modifiers | Rendered |
|---|---:|---:|---:|---:|---|
| `NAT_ROCK_MEDIUM_01_LOD0` | 351 | 342 | 1 | 0 | Yes |
| `NAT_ROCK_MEDIUM_01_LOD1` | 233 | 188 | 1 | 0 | Yes |
| `NAT_ROCK_MEDIUM_01_COL` | 90 | 180 | 0 | 0 | No |

## Collision decision

Collision object: `NAT_ROCK_MEDIUM_01_COL`.

Decision: **created** as an independent simple convex-hull proxy.

Validation points:

- independent mesh object;
- 90 vertices / 180 triangles;
- approximately `74.36%` fewer vertices than LOD0;
- no material slots;
- `hide_render = true`;
- wire display for editor readability;
- custom role `simple_convex_hull`;
- minimum Blender `Z = 0.0`;
- no unapplied Modifier;
- not generated as a direct render-mesh collision assignment.

The collision mesh preserves the full support bounds of LOD0. Gameplay collision behavior remains subject to a later runtime integration test; this milestone only prepares the asset-side proxy.

## Transform and grounding

Every final object passed:

```text
Location = (0, 0, 0)
Rotation = (0, 0, 0)
Scale = (1, 1, 1)
Minimum Blender Z = 0.0
```

There are no negative scales, undocumented offsets, or unapplied modifiers. Blender Z-up to glTF Y-up grounding must be revalidated after future GLB export.

## Material status

| Check | Result |
|---|---|
| Material count | 1 |
| Material name | `Hongcun_Rock_Medium_01_Material` |
| Runtime alpha contract | `OPAQUE` |
| Material alpha | `1.0` |
| LOD0 assignment | Present |
| LOD1 assignment | Present; shared material |
| Collision assignment | None |
| Image count | 1 |
| Image | `Rocks_Diffuse` |
| Image dimensions | `2048 × 2048` |
| Texture packed | Yes |

No new texture, alpha mask, alpha blend, wetness system, moss layer, or random material variation was introduced.

Blender 4.2's viewport compatibility enum remains an export advisory: the future GLB precheck and binary Validator must verify that the glTF material is emitted as `OPAQUE`. The clean source stores `hongcun_runtime_alpha_mode = OPAQUE`, uses alpha 1, and has no intentional transparent surface.

## Hash change

| State | Bytes | SHA-256 |
|---|---:|---|
| Before runtime preparation | 3,033,760 | `0635398eff89c36462f9b9c6de0cc24cebbc5a1b496bcfd309df66e65575d43f` |
| After runtime preparation | 3,079,364 | `63dc965bda15f6550194bc503274ce1432e587ded3cbcdb7c0c80562a8681274` |

The hash change is expected and attributable to:

- addition of `NAT_ROCK_MEDIUM_01_LOD1`;
- addition of `NAT_ROCK_MEDIUM_01_COL`;
- runtime-preparation decision metadata;
- removal of the rejected temporary LOD2 candidate before final save.

No GLB or Registry file was changed.

## Saved milestone state

The Blend records:

```text
lod_status = LOD0_LOD1_COMPLETE_LOD2_NOT_REQUIRED
collision_status = NAT_ROCK_MEDIUM_01_COL_SIMPLE_CONVEX_HULL_CREATED
runtime_prep_milestone = 009-F
```

## Acceptance decision

Runtime export preparation status: **PASS WITH EXPORT ADVISORY**.

Passed:

- required LOD0 counts preserved;
- LOD1 reduction is within the requested range;
- unstable optional LOD2 excluded;
- independent simplified collision proxy created and hidden from rendering;
- material shared, texture packed, and OPAQUE contract retained;
- all final transforms are identity;
- all final meshes are grounded at Blender `Z = 0`;
- no unapplied modifiers remain;
- final Blend saved and hashed.

Future, separately authorized work:

- export precheck;
- GLB export;
- binary validation of glTF OPAQUE material, node set, transforms, texture embedding, and collision role;
- Runtime Registry integration;
- runtime collision behavior and placement tests;
- Git archive.
