# CommonTree_3 Clean Source Report

## Result

- Milestone: `008-J CommonTree_3 Clean Source Creation`
- Date: `2026-07-17`
- Status: **clean source created; production LOD/material work still pending**
- Blender GUI instance: existing connected instance only
- Blender headless/background: not used
- GLB generated: no
- CommonTree_A or CommonTree_5 modified: no
- Git add/commit/push: none

Created file:

```text
world/assets/nature/source/trees/hongcun_tree_common_03_clean_v001.blend
```

- File size: `11,194,948` bytes
- SHA-256: `6bc1427ac8e1b8861cce0a5af5a78509255a5063697b130ecd46d17cfacbf433`

## Identity

| Layer | Value |
|---|---|
| Source | `CommonTree_3` |
| Production | `Hongcun_Tree_Common_03` |
| Runtime ID | `NAT_TREE_COMMON_03` |
| Display name | `Hongcun Common Tree 03` |
| Usage | `valley_tree` |

No prohibited legacy candidate name (`NAT_MOUNTAIN_TREE_COMMON_B`, `NAT_VALLEY_TREE_COMMON_A`, or `Hongcun_Tree_Common_B`) was created.

## Source information

Only the canonical glTF source was imported:

```text
/Users/huangtingting/Downloads/Stylized Nature MegaKit[Standard]/glTF/CommonTree_3.gltf
```

| Source file | Bytes | SHA-256 |
|---|---:|---|
| `CommonTree_3.gltf` | 3,864 | `17c6537aa8042bb7255fbb501bf7aeb2b6db87c16766f794f9c37be3ce70c976` |
| `CommonTree_3.bin` | 296,984 | `24541f46fd9553e2389aba15575f2697009226e82118ed76868284114fd12b49` |

FBX, Unity FBX, and OBJ versions were discovered during Phase 1 but were not imported.

## Blender environment

- Blender version: `4.2.22 LTS`
- Source file before save: unsaved default GUI scene
- Unit system: `METRIC`
- Unit scale: `1.0`
- Effective rule: 1 Blender Unit = 1 metre

The connected GUI initially contained only default Cube, Camera, and Light in an unsaved scene. Those defaults were removed before source import. No project Blend or existing asset was opened.

## Import result

Import succeeded through Blender's glTF importer.

Imported source object:

| Object | Type | Mesh | Vertices | Triangles | Materials |
|---|---|---|---:|---:|---|
| `CommonTree_3` | Mesh | `tree.025` | 5,749 | 3,505 | `Bark_NormalTree`, `Leaves_NormalTree` |

The imported source reference remains unchanged with:

- Location `(0,0,0)`;
- Rotation `(0,0,0)`;
- Scale `(1,1,1)`;
- Origin `(0,0,0)`;
- source bounds Z `-0.242772` to `9.182418` metres.

The negative source minimum Z is preserved only in `Source_Reference` as provenance. Runtime work duplicates were grounded separately.

## Collection structure

```text
Nature_Asset_Work
├── Source_Reference
├── LOD
├── Export
├── Collision
└── Preview
```

| Collection | Objects | Status |
|---|---|---|
| `Nature_Asset_Work` | none directly; owns child Collections | Pass |
| `Source_Reference` | `CommonTree_3` | Untouched imported reference |
| `LOD` | three named LOD work objects | Structure prepared |
| `Export` | same three named LOD objects | Structure prepared; not approved for export yet |
| `Collision` | empty | No collision created |
| `Preview` | empty | No preview scene created |

## Runtime object list

| Object | Mesh datablock | Vertices | Triangles | Min Z | Transform |
|---|---|---:|---:|---:|---|
| `NAT_TREE_COMMON_03_LOD0` | `NAT_TREE_COMMON_03_LOD0_MESH` | 5,749 | 3,505 | `0.0` | Identity |
| `NAT_TREE_COMMON_03_LOD1` | `NAT_TREE_COMMON_03_LOD1_MESH` | 5,749 | 3,505 | `0.0` | Identity |
| `NAT_TREE_COMMON_03_LOD2` | `NAT_TREE_COMMON_03_LOD2_MESH` | 5,749 | 3,505 | `0.0` | Identity |

For all three work objects:

- Location `(0,0,0)`;
- Rotation `(0,0,0)`;
- Scale `(1,1,1)`;
- Pivot/origin `(0,0,0)`;
- grounded geometry at Blender Z `0`;
- maximum Z `9.425190` metres.

Each work object owns independent mesh data. The untouched source object remains in `Source_Reference`.

## Materials and textures

Material count: **2**

| Material | Current render method | Alpha input | Texture data |
|---|---|---|---|
| `Bark_NormalTree` | `DITHERED` | Linked | base colour and normal |
| `Leaves_NormalTree` | `DITHERED` | Linked | leaf colour/alpha |

Textures:

| Image | Packed | Source path status |
|---|---|---|
| `Bark_NormalTree.png` | Yes | External source path retained in metadata |
| `Bark_NormalTree_Normal.png` | Yes | External source path retained in metadata |
| `Leaves_NormalTree_C.png` | Yes | External source path retained in metadata |

The images are packed in the Blend, so the clean source is not dependent on the external files merely to reopen. Source paths remain recorded for provenance.

Alpha Clip feasibility: **feasible but not approved**. The leaf texture is connected to Principled Alpha, but the imported material currently uses Blender 4.2 `DITHERED`, not the required reviewed Alpha Clip policy. Cutoff, two-sided behavior, fringe, shadow flicker, and overdraw remain future material-production checks.

## Transform and grounding status

| Check | Source reference | Runtime work objects |
|---|---|---|
| Location/Rotation/Scale identity | Pass | Pass |
| Pivot/origin at `(0,0,0)` | Pass | Pass |
| Grounding at Z=0 | Source preserved at `-0.242772` | Pass |
| Metre units | Scene Metric 1.0 | Scene Metric 1.0 |

The source mesh was not altered. Grounding was applied only to independent work mesh copies by shifting their mesh-space Z while retaining identity Object transforms and origin Pivot.

## Known issues and required next work

1. **LOD1 and LOD2 are structural duplicates, not reduced production LODs.** All three objects currently contain 3,505 triangles.
2. LOD1 exceeds its future budget of 2,000 triangles.
3. LOD2 exceeds its future budget of 800 triangles.
4. Source material names and colors have not yet been converted to final Hongcun project materials.
5. Leaf Alpha Clip has not been configured or visually validated.
6. Bark currently also reports a linked alpha input and imported DITHERED method; final bark must be opaque.
7. Collision is intentionally empty and requires a separate gameplay decision.
8. Preview and LOD transition review have not been performed.
9. Export Collection is structurally populated but **must not be exported** until LOD and material gates pass.

## Integrity and stop status

The clean source and report were completed using the existing Blender GUI/MCP connection. No GLB was exported, no existing GLB was changed, CommonTree_A and CommonTree_5 were not touched, and no Terrain/GIS/Architecture file or Git state was modified.
