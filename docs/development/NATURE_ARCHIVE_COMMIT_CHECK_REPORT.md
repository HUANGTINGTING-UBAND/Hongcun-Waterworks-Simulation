# Nature Archive Commit Check Report

## Audit scope

- Milestone: `008-F2 Nature Asset Archive Commit Preparation`
- Date: `2026-07-17`
- Mode: read-only Git and filesystem audit, plus this report
- `git add`: not executed
- `git commit`: not executed
- `git push`: not executed
- Blender operations: none

## Git state

| Check | Result |
|---|---|
| Current branch | `main` |
| HEAD | `f42c994 chore: archive frozen Hui architecture assets` |
| Configured upstream | `origin/main` |
| Local divergence from recorded upstream | 0 ahead / 0 behind |
| Staging area | Empty |
| Working tree clean | No |

The synchronization result uses the repository's existing local `origin/main` reference. No network fetch was performed, so it does not prove that the GitHub branch has not changed since that reference was last refreshed.

The working tree contains numerous unrelated modified/untracked Architecture, Terrain, GIS, and Nature files. The tracked file `world/geo/README.md` is modified. Broad staging commands such as `git add .`, `git add world`, or `git add docs/development` are unsafe for this archive commit.

## Required-file audit

| File | Exists | Git tracking |
|---|---|---|
| `world/assets/nature/processed/trees/Hongcun_Tree_Common_A_v002.glb` | Yes | Untracked |
| `docs/development/HONGCUN_TREE_COMMON_A_GLB_VALIDATION.md` | Yes | Untracked |
| `docs/development/NATURE_GAME_ASSET_SELECTION.md` | Yes | Untracked |
| `docs/development/NATURE_ASSET_PROCESSING_SPEC.md` | Yes | Untracked |
| `world/assets/nature/manifests/nature_game_asset_selection.json` | Yes | Untracked |

All requested archive inputs are present. None is currently tracked or staged.

## GLB integrity

Asset:

```text
world/assets/nature/processed/trees/Hongcun_Tree_Common_A_v002.glb
```

| Check | Actual | Validation report | Result |
|---|---:|---:|---|
| File size | `9,430,560` bytes | `9,430,560` bytes | Match |
| SHA-256 | `2fb88ad035acdbd35ce5c681425c4a03f3234ecccf59d7e6104e2d6a2dbd03c9` | Same | Match |

The GLB has already passed the content audit documented in `HONGCUN_TREE_COMMON_A_GLB_VALIDATION.md`: glTF 2.0 parsing, three required LOD nodes, three meshes, two assigned materials, identity transforms, origin Pivot, and no source-Blender negative-Z penetration.

## `.gitignore` audit

`git check-ignore --no-index` confirms that neither the final GLB nor a probe path under `world/assets/nature/processed/trees/` is ignored.

The current `.gitignore` has Architecture preview exclusions but no rule matching:

```text
world/assets/nature/processed/
*.glb
```

Result: the final Nature GLB can be staged normally. No `.gitignore` change is needed for this commit.

## Recommended commit files

Core approved archive set:

1. `world/assets/nature/processed/trees/Hongcun_Tree_Common_A_v002.glb`
2. `docs/development/HONGCUN_TREE_COMMON_A_GLB_VALIDATION.md`
3. `world/assets/nature/manifests/nature_game_asset_selection.json`
4. `docs/development/NATURE_GAME_ASSET_SELECTION.md`
5. `docs/development/NATURE_ASSET_PROCESSING_SPEC.md`
6. `docs/development/NATURE_ARCHIVE_COMMIT_CHECK_REPORT.md`

The first five files total `9,483,040` bytes before Git compression. The check report adds only a small text file. The 9.0 MiB GLB is below GitHub's per-file hard limit; Git LFS is not required for this single asset, though it should be reconsidered if processed binary assets begin accumulating rapidly.

## Files not recommended for this commit

### Superseded or failed-output evidence

- `world/assets/nature/processed/trees/Hongcun_Tree_Common_A.glb` — earlier deprecated candidate, not the approved v002 asset.
- `world/assets/nature/processed/trees/Hongcun_Tree_Common_A_export_report.md` — failed MCP export recovery evidence.
- `world/assets/nature/processed/trees/Hongcun_Tree_Common_A_validation_v002.md` — earlier failed validation evidence.
- `world/assets/nature/processed/trees/Hongcun_Tree_Common_A_v002_validation.md` — historical failed headless-attempt report; the final approved content report is under `docs/development/`.
- `docs/development/BLENDER_HEADLESS_STABILITY_REPORT.md`
- `docs/development/BLENDER_VERSION_COMPATIBILITY_REPORT.md`
- `docs/development/HONGCUN_TREE_SOURCE_CLEAN_TEST.md`

These may be retained locally as troubleshooting records but would obscure the approved archive scope if mixed into the asset commit.

### Local production sources and backups

- `world/assets/nature/source/trees/Hongcun_Tree_Common_A.blend`
- `world/assets/nature/source/trees/Hongcun_Tree_Common_A_clean_v001.blend`
- `world/assets/nature/source/trees/Hongcun_Tree_Common_A.blend1`

The `.blend1` file is an automatic backup and should not be committed. The source Blend files are approximately 11 MiB each and are outside the requested final GLB archive. Keep them local/cloud-side unless a later source-asset archive milestone explicitly approves them.

### Pipeline and broader library files not approved in this commit

- `world/assets/nature/processing/export_tree_glb_headless.py`
- `world/assets/nature/manifests/quaternius_nature_megakit_manifest.json`
- `world/assets/nature/manifests/nature_asset_rules.json`
- `world/assets/nature/manifests/asset_manifest.json`
- other Nature documentation not listed in the recommended set
- all Architecture, Terrain, GIS, React/R3F, and package/configuration changes

These files may be useful in later scoped commits, but they should not be pulled into this approved-asset archive implicitly.

## Current risks

1. **Dirty mixed-purpose worktree:** unrelated Architecture, Terrain, GIS, and Nature artifacts are present. Exact-path staging is mandatory.
2. **Recorded remote state may be stale:** local `HEAD` and the existing `origin/main` reference match, but no fetch was run during this read-only audit.
3. **Duplicate CommonTree outputs:** both the deprecated `Hongcun_Tree_Common_A.glb` and approved `Hongcun_Tree_Common_A_v002.glb` exist. Only v002 should be staged.
4. **Multiple validation reports:** several failed-attempt reports coexist with the final content validation. Stage only `docs/development/HONGCUN_TREE_COMMON_A_GLB_VALIDATION.md` for approval evidence.
5. **Binary history growth:** the current GLB is acceptable in normal Git, but repeated binary revisions will not delta-compress as efficiently as text. Reassess Git LFS before committing many more processed GLBs.

## Suggested next `git add`

Do not run this until the user approves the archive scope:

```bash
git add -- \
  world/assets/nature/processed/trees/Hongcun_Tree_Common_A_v002.glb \
  world/assets/nature/manifests/nature_game_asset_selection.json \
  docs/development/HONGCUN_TREE_COMMON_A_GLB_VALIDATION.md \
  docs/development/NATURE_GAME_ASSET_SELECTION.md \
  docs/development/NATURE_ASSET_PROCESSING_SPEC.md \
  docs/development/NATURE_ARCHIVE_COMMIT_CHECK_REPORT.md
```

After staging, the required safety checks are:

```bash
git diff --cached --name-status
git diff --cached --stat
```

The cached file list must contain only the six paths above before any commit is authorized.
