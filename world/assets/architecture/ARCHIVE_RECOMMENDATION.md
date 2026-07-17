# Architecture Archive Recommendation

## Scope and safety

This report prepares the Milestone 009 architecture set for Git version management and possible local-space cleanup. It does not delete, move, stage, or commit any file.

Inspection date: 2026-07-17  
Asset root: `world/assets/architecture/`

## KEEP_LOCAL

These files define the approved asset, its verification evidence, manifest, and production rules. Keep them in the active workspace.

| File | Size (bytes) | SHA-256 | Upload to Git | Delete locally |
|---|---:|---|---|---|
| `prefabs/hongcun_house_test_b_material_v001.blend` | 719,223 | `30223e9cfb7ffc5cfe100644ccf9858c0c9f60002dadc05d83571f89a856a073` | Yes; Git LFS is recommended for Blender binaries | No |
| `manifests/architecture_asset_manifest.json` | 5,498 | `fd6bee265c042fe6bcb7a2e468f74ddeab3605ec646b9bc76d1a05cfc9e35eb5` | Yes | No |
| `previews/preview_render.png` | 530,958 | `b09a2755e1271fe0a5d02b55309896cbd660768daed827c2367d7da9cceba920` | Yes | No |
| `ARCHITECTURE_ASSET_STATUS.md` | 3,860 | `b2d467e7f457d80318ba8901da87a539a3f15d9f6271d17c16394485f353ac65` | Yes | No |
| `docs/development/HUI_ARCHITECTURE_MODULAR_SPEC.md` | 15,801 | `77d0cefd01aa95d54a78b5f972b1b2d1c391f6886ac55ee984dce850fd0c2790` | Yes | No |

`ARCHIVE_RECOMMENDATION.md` itself is also a KEEP_LOCAL document and should be included in the same architecture archive commit.

## ARCHIVE_CANDIDATE: prototype source files

These are editable prototype sources used to create the approved prefab. Upload them before considering any local cleanup. Because they preserve reconstruction capability, local deletion is only recommended after the remote object and its SHA-256 have been verified and an intentional archive/sparse-checkout policy exists.

| File | Size (bytes) | SHA-256 | Upload to Git | Delete locally |
|---|---:|---|---|---|
| `source/hui_architecture_blockout_walls_v001.blend` | 103,238 | `a2fd26d4b6eeaf9cfbf2ca622ef5f84ebf3bc44af9794cf3a9ddbc63a1cfb80e` | Yes; preferably Git LFS | Conditional after remote verification |
| `source/roof_blockout_v001.blend` | 406,981 | `8ee97cb28e4e966779f56b37d4c5f949bc2c0ea406504f48f46e63cec5cd5c0e` | Yes; preferably Git LFS | Conditional after remote verification |
| `source/hui_architecture_details_v001.blend` | 480,294 | `e225342d7c24e40429050ecf9c69fe820641e7fafa0a1f180e2d73c349a95808` | Yes; preferably Git LFS | Conditional after remote verification |
| `source/architecture_scale_extension_v001.blend` | 477,458 | `7683d01f4743e8782041b7dffa2e5c2c89a13fdd03a94957bc60c78f91b5c681` | Yes; preferably Git LFS | Conditional after remote verification |

Prototype source total: 1,467,971 bytes, approximately 1.40 MiB.

## ARCHIVE_CANDIDATE: superseded working scenes

These files are not approved assets. They remain useful as assembly evidence but are lower priority than the module source files.

| File | Size (bytes) | SHA-256 | Upload to Git | Delete locally |
|---|---:|---|---|---|
| `prefabs/hongcun_house_test_a_v001.blend` | 691,202 | `eff512a7d00e6752c88d7e0d2fa025bb98b9a4e2a9944359e5d6c2b44266e5cd` | Optional; Git LFS if retained | Conditional after approved prefab and manifest are verified remotely |
| `prefabs/hongcun_house_test_b_v001.blend` | 626,904 | `f974dbb0e7a5635eaa15068fe83c053e5859ee64ca0cbada854e544883f5a606` | Recommended as geometry-validation provenance | Conditional after remote verification |
| `previews/architecture_test_layout_v001.blend` | 106,609 | `afaa69a7db43d28fc5474f775028098033fc418924b201a81e64e5789cee831c` | Optional | Conditional after remote verification |

Superseded working-scene total: 1,424,715 bytes, approximately 1.36 MiB.

## Optional prototype preview

| File | Size (bytes) | SHA-256 | Upload to Git | Delete locally |
|---|---:|---|---|---|
| `previews/preview_sheet.png` | 1,552,184 | `ae50a256bee4a97c24cf116294c3ba44b6ba8232ebd33cd44e7d3b69c5bca660` | Optional; it is not the final material validation image | Yes, but only after deciding it is not needed as review evidence |

## Local recovery files not suitable for Git

These are Blender automatic backups or operating-system metadata, not registered assets. They are potential cleanup items, but this milestone does not delete them.

| File | Size (bytes) | SHA-256 | Upload to Git | Delete locally |
|---|---:|---|---|---|
| `prefabs/hongcun_house_test_a_v001.blend1` | 443,505 | `b46df0e6e6f9f3ac01817824468dede08514615a184e87783eaf5247deed057f` | No | Yes, after confirming the corresponding `.blend` opens |
| `prefabs/hongcun_house_test_b_v001.blend1` | 688,696 | `a5b309a09c6c36f39fd92f6c3df931060c4cfe8660d7c44b170cdfb1a7aa2a43` | No | Yes, after confirming the corresponding `.blend` opens |
| `prefabs/hongcun_house_test_b_material_v001.blend1` | 719,223 | `f4be63c7ccdd73c80b263787b902ee2b4258ea1f0d745c2c7b92fc290d508432` | No | Yes, after approved asset and remote copy are verified |
| `source/architecture_scale_extension_v001.blend1` | 441,555 | `f950ff1a86ece0b134882e6607f157c14c5fd34b5b9b307d67a56c54eb4d9d27` | No | Yes, after the source `.blend` and remote copy are verified |
| `.DS_Store` | 6,148 | `afa324d07cbd5d9e79cbdbf4f2a68ac0f07e0dce71cd275568a5f871d28d886d` | No | Yes |

The four `.blend1` files occupy 2,292,979 bytes, approximately 2.19 MiB. They offer the clearest low-risk disk-recovery opportunity after verification.

## `.gitignore` audit

Current result:

| Pattern | Present | Result |
|---|---|---|
| `.DS_Store` | Yes | Correctly excluded |
| `*.blend1` | No | Blender recovery files may be accidentally committed |
| Temporary render files | No explicit rule | Temporary render outputs may be accidentally committed |

Recommended additions for a later approved edit:

```gitignore
*.blend1
world/assets/architecture/previews/*_temp.*
world/assets/architecture/previews/*_audit.*
world/assets/architecture/previews/render_tmp/
```

Do not ignore `world/assets/architecture/previews/preview_render.png`; it is the final registered validation image.

## Disk-space assessment

The volume currently reports approximately 16 GiB available and 85% utilization. Architecture files are not the primary disk-space risk:

- Registered architecture directory: approximately 7.7 MiB.
- Prototype source and superseded working scenes: approximately 2.76 MiB combined.
- Blender `.blend1` recovery files: approximately 2.19 MiB.
- Optional prototype preview sheet: approximately 1.48 MiB.

Deleting every identified architecture cleanup candidate would recover only about 6.4 MiB. Large terrain builds, DEM derivatives, Blender caches, and external asset packages should be audited separately if substantial space recovery is required.

## Recommended Git sequence

1. Add `*.blend` to Git LFS tracking before staging Blender files, if Git LFS is part of the repository policy.
2. Add the missing `.gitignore` rules in a separately reviewed edit.
3. Stage only the approved prefab, manifest, final preview, architecture documentation, and intentionally retained prototype sources.
4. Verify staged paths with `git diff --cached --name-status`.
5. Commit with the suggested message below.
6. Push and verify that every LFS object or binary file exists remotely and matches the recorded SHA-256.
7. Only then decide whether to remove local `.blend1` backups or use a sparse/archive workflow for prototype sources.

Suggested commit message:

```text
chore: archive frozen Hui architecture assets
```

No Git command that changes repository state was executed by this milestone.
