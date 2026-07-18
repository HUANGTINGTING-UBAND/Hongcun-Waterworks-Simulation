# Rock Medium 01 Clean Source Report

## Result

Milestone 009-E completed successfully using the connected Blender 4.2.22 LTS GUI/MCP environment. The clean source contains one grounded LOD0 mesh for `NAT_ROCK_MEDIUM_01`. No GLB was exported, no Runtime Registry entry was created or changed, and no Tree asset was opened or modified.

This is a D-level game-art asset derived from the external Quaternius source library. It is not surveyed Hongcun geology, historical stone evidence, GIS data, or a reconstruction of the real landscape or riverbed.

## Identity

| Field | Value |
|---|---|
| Source name | `Rock_Medium_1` |
| Production name | `Hongcun_Rock_Medium_01` |
| Runtime ID | `NAT_ROCK_MEDIUM_01` |
| Display name | `Hongcun Medium Rock 01` |
| Blender scene | `NAT_ROCK_MEDIUM_01_CleanSource` |
| Runtime collection | `NAT_ROCK_MEDIUM_01` |
| Clean source | `world/assets/nature/source/rocks/hongcun_rock_medium_01_clean_v001.blend` |

## Source audit

Canonical source:

```text
/Users/huangtingting/Downloads/Stylized Nature MegaKit[Standard]/glTF/Rock_Medium_1.gltf
```

| Source dependency | Bytes | SHA-256 |
|---|---:|---|
| `Rock_Medium_1.gltf` | 1,831 | `5d1ab1fa03218f4a908728ed0eabe6486344a7e4f510b0d3d6f837fd1d011bd8` |
| `Rock_Medium_1.bin` | 13,284 | `ea79527d4c2b3bdb7a28bf589bf0c1fd96d192a4d0738636753911dd58681e99` |
| `Rocks_Diffuse.png` | 2,517,398 | `b813696df6081f469a127092eb0ce45cc39d56f3ccc0699152bfcd3804810b48` |

The canonical glTF source, matching BIN, and referenced texture were present. No FBX, Unity FBX, OBJ, terrain, world, or Tree source was imported.

## Clean Blend audit

| Check | Result |
|---|---|
| Blender version | `4.2.22 LTS` |
| File | `world/assets/nature/source/rocks/hongcun_rock_medium_01_clean_v001.blend` |
| File size | 3,033,760 bytes |
| Blend SHA-256 | `0635398eff89c36462f9b9c6de0cc24cebbc5a1b496bcfd309df66e65575d43f` |
| Saved timestamp | `2026-07-18T10:09:28+0800` |
| Unit system | Metric |
| Unit scale | `1 Blender Unit = 1 metre` |
| Save verification | File exists after `save_as_mainfile` |

## Collection and object structure

```text
NAT_ROCK_MEDIUM_01
└── NAT_ROCK_MEDIUM_01_LOD0
```

Only the runtime Rock collection and LOD0 mesh are present. Imported container/empty objects were not retained.

## Mesh statistics

| Object | Vertices | Edges | Polygons | Triangles | Dimensions in metres |
|---|---:|---:|---:|---:|---|
| `NAT_ROCK_MEDIUM_01_LOD0` | 351 | 671 | 342 | 342 | `3.225148 × 2.989136 × 2.259797` |

LOD0 preserves the audited source topology. No decimation or random geometry change was performed in this milestone.

## Transform and Pivot

| Property | Result | Status |
|---|---|---|
| Location | `(0, 0, 0)` | Pass |
| Rotation | `(0, 0, 0)` | Pass |
| Scale | `(1, 1, 1)` | Pass |
| Pivot | Support-plane XY centre at object origin | Pass |
| Minimum source Z | `0.0 m` | Pass |
| Maximum source Z | `2.259797 m` | Recorded |

The imported transform was applied before grounding. Mesh coordinates were offset so the support-plane centre becomes the object origin while the final object transform remains identity.

## Grounding result

- Lowest mesh contact reaches Blender `Z = 0.0 m`.
- No negative-Z vertex remained in the final LOD0 audit.
- The underside was not flattened and the source silhouette was preserved.
- Grounding is a game-asset support-plane operation, not a claim about a real placement surface.

## Material and texture audit

| Check | Result |
|---|---|
| Material slots | 1 |
| Material | `Hongcun_Rock_Medium_01_Material` |
| Material nodes | Enabled |
| Runtime alpha contract | `OPAQUE` |
| Principled alpha | `1.0` |
| Alpha input links | 0 |
| Texture images | 1 |
| Image | `Rocks_Diffuse` |
| Image dimensions | `2048 × 2048` |
| Packed in Blend | Yes |
| Source texture path | `/Users/huangtingting/Downloads/Stylized Nature MegaKit[Standard]/glTF/Rocks_Diffuse.png` |

Blender 4.2 exposes the viewport compatibility state as `HASHED` / `DITHERED` even when the material has constant alpha 1 and no alpha input link. To avoid treating that UI compatibility enum as the runtime packaging contract, the clean source records `hongcun_runtime_alpha_mode = OPAQUE`. The future GLB export precheck must verify that the exported glTF material is actually `OPAQUE`; this report does not substitute for binary GLB validation.

The source texture was preserved and packed. No new texture, alpha mask, procedural wetness, moss, or color-randomization system was added.

## LOD status

| Level | Object | State | Triangles |
|---|---|---|---:|
| LOD0 | `NAT_ROCK_MEDIUM_01_LOD0` | Complete; source topology preserved | 342 |
| LOD1 | `NAT_ROCK_MEDIUM_01_LOD1` | Not created in this milestone | — |
| LOD2 | `NAT_ROCK_MEDIUM_01_LOD2` | Not created in this milestone | — |

The scene records `LOD0_COMPLETE_LOD1_LOD2_NOT_CREATED`. Empty placeholder meshes were deliberately avoided. LOD1 and LOD2 require a separately approved silhouette-aware production pass.

## Collision status

- Reserved runtime name: `NAT_ROCK_MEDIUM_01_COL`.
- Collision object created: No.
- Scene status: `NAT_ROCK_MEDIUM_01_COL_RESERVED_NOT_CREATED`.
- Future rule: use a reviewed simple proxy; never use the render mesh automatically as collision.

## Known issue and recovery note

The first MCP execution reached the final selection-cleanup line and reported:

```text
'Context' object has no attribute 'selected_objects'
```

The Blender MCP connection then temporarily disconnected. After reconnection, a read-only scene audit confirmed that the imported Rock, collection, naming, transforms, grounding, packed texture, and material remained intact. Final validation and saving were completed using `bpy.data` access without relying on `selected_objects`. The asset was not re-imported and no duplicate object was created.

## Acceptance decision

Clean-source status: **PASS WITH RECORDED ADVISORY**.

Passed:

- canonical source and dependencies present;
- isolated Rock-only scene;
- required collection and LOD0 naming;
- source topology retained;
- identity transform;
- support-plane Pivot and `Z = 0` grounding;
- one verifiable opaque-contract material;
- texture preserved and packed;
- Blend saved and hashed.

Advisory:

- Blender 4.2 viewport material enum must be rechecked at GLB export because the authoritative runtime requirement is glTF `OPAQUE`.

Not yet authorized or completed:

- LOD1 / LOD2;
- collision proxy;
- GLB export;
- GLB validation;
- Runtime Registry integration;
- world placement or scattering;
- Git archive.
