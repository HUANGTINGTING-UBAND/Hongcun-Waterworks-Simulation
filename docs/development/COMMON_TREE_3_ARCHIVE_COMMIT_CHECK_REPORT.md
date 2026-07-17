# CommonTree_03 Archive Commit Check Report

## Result

- Milestone: `008-P Nature CommonTree_03 Archive Commit Preparation`
- Date: `2026-07-17`
- Status: **ready for path-scoped staging; not yet staged**
- Audit mode: read-only except creation of this report
- Git add/commit/push: not executed

## Git state

| Check | Result |
|---|---|
| Current branch | `main` |
| HEAD | `2bb5bc28dd28e39b7800daba7af9b6a1d89af810` |
| HEAD summary | `docs: establish nature batch production pipeline` |
| Upstream | `origin/main` |
| Ahead / behind | `0 / 0` |
| Staging area | Empty |
| Working tree | Not clean |

The ahead/behind result compares HEAD with the locally stored `origin/main` reference. This audit intentionally did not run `git fetch`, so it does not claim that the remote server has not changed since the last fetch.

Recent relevant history:

- `2bb5bc2` — Nature batch production pipeline;
- `045619c` — archived validated CommonTree_A runtime asset;
- `f42c994` — frozen Hui architecture archive.

## Runtime asset integrity

GLB:

```text
world/assets/nature/processed/trees/hongcun_tree_common_03_v001.glb
```

| Check | Value | Result |
|---|---|---|
| Exists | Yes | Pass |
| Size | `9,046,344` bytes | Pass |
| Actual SHA-256 | `63eeb2dde5ce7e80f1ff7eb2e0cb71514127de79ebee6ecdf80e9dcde5db4ee0` | Pass |
| Registry SHA-256 | `63eeb2dde5ce7e80f1ff7eb2e0cb71514127de79ebee6ecdf80e9dcde5db4ee0` | Pass |
| Hash match | Yes | Pass |

Registry:

```text
world/assets/nature/manifests/nature_runtime_registry.json
```

| Check | Result |
|---|---|
| JSON parses successfully | Pass |
| `NAT_TREE_COMMON_03` entry count | 1 |
| Processed path matches target GLB | Pass |
| Processed byte size matches | Pass |
| Status | `validated` |
| CommonTree_A entry retained | Pass |

## Recommended commit scope

### Core archive files

These files are required to archive and register the validated runtime asset:

```text
world/assets/nature/processed/trees/hongcun_tree_common_03_v001.glb
world/assets/nature/manifests/nature_runtime_registry.json
docs/development/COMMON_TREE_3_GLB_VALIDATION.md
docs/development/COMMON_TREE_3_REGISTRY_CHANGE_REPORT.md
docs/development/COMMON_TREE_3_ARCHIVE_COMMIT_CHECK_REPORT.md
```

### Supporting production evidence

These CommonTree_03-only reports are also recommended so the archive retains the complete LOD, material, precheck, and export audit trail:

```text
docs/development/COMMON_TREE_3_LOD_MATERIAL_REPORT.md
docs/development/COMMON_TREE_3_EXPORT_PRECHECK_REPORT.md
docs/development/COMMON_TREE_3_EXPORT_REPORT.md
```

The seven files measured before creation of this report total `9,074,019` bytes. Including this small Markdown report, the proposed commit remains approximately **9.08 MB** before Git compression.

## Files not recommended for this commit

Do not stage the following:

- `world/assets/nature/source/trees/hongcun_tree_common_03_clean_v001.blend` — production source Blend; excluded by Nature batch commit rules;
- any CommonTree_A failed export, recovery report, or source file;
- any CommonTree_5 file;
- `world/geo/**` and Terrain/GIS changes;
- `world/assets/architecture/**`;
- unrelated Nature manifests, previews, processing scripts, or reports;
- any other currently untracked file outside the explicit list above.

## Working-tree risk

The working tree contains many unrelated modified and untracked files from Architecture, Terrain/GIS, older Nature preparation, Blender compatibility investigation, source Blend production, and failed/recovery exports. It also contains an unrelated modification to:

```text
world/geo/README.md
```

The target Registry is modified and the CommonTree_03 GLB/reports are untracked. Because unrelated files coexist in the working tree, a future archive command must use an explicit `git add -- <exact paths>` list. Broad commands such as `git add .`, `git add -A`, or directory-wide Nature staging are unsafe.

## Preparation decision

The CommonTree_03 runtime asset is ready for a narrowly scoped archive staging review:

- GLB exists and matches the registered SHA-256;
- Registry JSON is valid and contains exactly one `NAT_TREE_COMMON_03` entry;
- validation and production reports exist;
- staging area is currently empty;
- no blocking integrity issue was found.

No staging, commit, push, fetch, asset modification, or Registry modification was performed during this audit.
