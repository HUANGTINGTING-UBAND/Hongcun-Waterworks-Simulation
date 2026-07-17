# Architecture Asset Status

## Freeze declaration

Milestone 009-G freezes the currently validated Hui-style architecture asset set. No additional model production, duplicate GLB export, or source-module editing is authorized by this milestone.

Freeze date: 2026-07-17  
Manifest: `world/assets/architecture/manifests/architecture_asset_manifest.json`

## Approved asset

| Asset | Type | Meshes | Status |
|---|---|---:|---|
| `prefabs/hongcun_house_test_b_material_v001.blend` | Material-validated assembled prefab | 42 | approved |

This is the only approved building asset in the current freeze. Its four validated materials are:

- `Hui_WhiteWall`
- `Hui_BlackTile`
- `Hui_StoneBase`
- `Hui_Wood`

The associated validation image is `previews/preview_render.png`.

## Prototype assets

| Asset | Purpose | Meshes | Status |
|---|---|---:|---|
| `source/hui_architecture_blockout_walls_v001.blend` | Wall and foundation module source | 6 | prototype |
| `source/roof_blockout_v001.blend` | 3 m roof module source | 5 | prototype |
| `source/hui_architecture_details_v001.blend` | Window, door, and small/medium detail source | 5 | prototype |
| `source/architecture_scale_extension_v001.blend` | 5 m roof and large horse-head-wall extension source | 5 | prototype |
| `prefabs/hongcun_house_test_a_v001.blend` | Superseded assembly test | 43 | prototype |
| `prefabs/hongcun_house_test_b_v001.blend` | Geometry-validated Test B before final material validation | 42 | prototype |
| `previews/architecture_test_layout_v001.blend` | Wall/foundation snap-layout test | 12 | prototype |

Prototype status means the file remains useful as editable production evidence, but it is not approved as a final game-ready asset.

## Directory archive policy

- `source/` retains the final editable module source files used by the approved prefab.
- `prefabs/` retains the approved material prefab and its earlier assembly prototypes for traceability.
- `previews/` retains the final material validation image and earlier prototype review evidence.
- `manifests/` contains the immutable metadata snapshot for this freeze.
- `processed/`, `materials/`, and other placeholder directories remain available but contain no new exported asset from this milestone.
- `.blend1` files are Blender automatic recovery backups. They are retained on disk but excluded from the asset registry.
- `.DS_Store` and `.gitkeep` files are not assets and are excluded from the registry.

No file was deleted or moved during the freeze, preventing accidental loss of source or recovery data.

## Verification summary

- Every registered Blender file was opened individually and inspected for Mesh count and used material names.
- File sizes and SHA-256 hashes are recorded in the manifest.
- `hongcun_house_test_b_material_v001.blend` remains the only approved record.
- No terrain file was opened.
- No building geometry was generated.
- No GLB was exported.
- No Git commit was created.

## Disk capacity

At freeze time, the workspace volume reported:

- Total: 113 GiB
- Used: 84 GiB
- Available: 16 GiB
- Capacity used: 85%

The remaining space is adequate for this archive, but the volume is already above 80% utilization. Future large Blender caches, textures, DEM derivatives, or duplicated exports should be reviewed before generation.

## Git commit suggestion

Review the complete uncommitted worktree before staging because the repository also contains unrelated terrain and nature-asset changes. For an architecture-only commit, stage only:

```text
world/assets/architecture/
docs/development/HUI_ARCHITECTURE_MODULAR_SPEC.md
```

Suggested commit message:

```text
feat: freeze validated Hui architecture asset set
```

Do not commit `.DS_Store`. Decide separately whether Blender `.blend1` recovery backups belong in version control; they are not registered release assets.
