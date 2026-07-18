# Rock Medium 01 Archive Commit Check Report

## Result

Milestone 009-J read-only archive audit completed. `NAT_ROCK_MEDIUM_01` has a validated GLB, matching Runtime Registry entry, complete production/validation evidence, and a compatible standalone Registry schema.

Archive preparation status: **READY WITH STRICT EXACT-PATH STAGING REQUIRED**.

No Git add, commit, push, fetch, restore, or file deletion was performed. No GLB, Blend, Registry, Tree asset, or Blender scene was modified during this audit. This report is the only file created by the milestone.

## Git state

| Check | Result |
|---|---|
| Branch | `main` |
| HEAD | `136df6d51adb87d9bfb1762eb30c9449d6badc86` |
| HEAD summary | `136df6d feat: archive validated CommonTree_03 nature asset` |
| Local tracking ref | `origin/main` |
| Local `origin/main` hash | `136df6d51adb87d9bfb1762eb30c9449d6badc86` |
| Ahead / behind | `0 / 0` |
| Staging area | Empty |
| Working tree | Dirty; mixed-scope changes present |

No network fetch was performed. Synchronization is assessed against the existing local `origin/main` tracking ref.

### Relevant working-tree state

- Modified and intended: `world/assets/nature/manifests/nature_runtime_registry.json`.
- Untracked and intended: Rock GLB, five Rock production/validation reports, standalone Registry schema, and this archive report.
- Modified and unrelated: `world/geo/README.md`.
- Numerous unrelated untracked files exist under Architecture, Terrain/GIS, older Nature work, source Blend directories, processing directories, previews, and other documentation.

Risk level: **high if broad staging is used**. `git add .`, `git add -A`, directory-wide adds, or wildcard staging are prohibited for this archive.

## Required Runtime files audit

All user-required files exist:

| Required file | Bytes | Tracking state | Archive decision |
|---|---:|---|---|
| `world/assets/nature/processed/rocks/hongcun_rock_medium_01_v001.glb` | 2,542,280 | Untracked | Include |
| `world/assets/nature/manifests/nature_runtime_registry.json` | 10,157 | Tracked, modified | Include |
| `docs/development/ROCK_MEDIUM_01_CLEAN_SOURCE_REPORT.md` | 6,562 | Untracked | Include |
| `docs/development/ROCK_MEDIUM_01_RUNTIME_PREP_REPORT.md` | 6,370 | Untracked | Include |
| `docs/development/ROCK_MEDIUM_01_EXPORT_REPORT.md` | 6,031 | Untracked | Include |
| `docs/development/ROCK_MEDIUM_01_GLB_VALIDATION.md` | 8,129 | Untracked | Include |
| `docs/development/ROCK_MEDIUM_01_REGISTRY_CHANGE_REPORT.md` | 7,191 | Untracked | Include |

## Required schema dependency

The Registry integration introduced optional `biome_roles` and nested `validation` fields. The standalone Validator schema was extended accordingly:

| Dependency | Bytes | SHA-256 | Tracking state | Decision |
|---|---:|---|---|---|
| `world/assets/nature/validation/schemas/nature_registry.schema.json` | 5,032 | `57c78b1a865d46fce2c6e73711838f19714d90a28b7442f8c70fb4e4e05a46ec` | Untracked | Mandatory include |

Excluding this schema while committing the new Registry would leave the repository internally inconsistent: the existing standalone Validator schema on `origin/main` would reject the new Rock entry's additional fields.

## Hash audit

### GLB

| Check | Value | Result |
|---|---|---|
| Expected SHA-256 | `635c527039c2a4a500606910238a249329bd11aa68b6b12f945ef48cd04c6feb` | — |
| Observed SHA-256 | `635c527039c2a4a500606910238a249329bd11aa68b6b12f945ef48cd04c6feb` | Pass |
| Expected bytes | 2,542,280 | — |
| Observed bytes | 2,542,280 | Pass |

The GLB hash matches the export report, binary validation report, and Runtime Registry.

### Runtime Registry

| Check | Value | Result |
|---|---|---|
| Expected SHA-256 | `b0e14c3cf00e0a1f13bf01bba032e12058c53649e5c2bbcc19b9aa9fca853eca` | — |
| Observed SHA-256 | `b0e14c3cf00e0a1f13bf01bba032e12058c53649e5c2bbcc19b9aa9fca853eca` | Pass |
| Registry asset count | 4 | Pass |
| Rock entry | `NAT_ROCK_MEDIUM_01` | Pass |
| Rock status | `validated` | Pass |

The project read-only Registry Validator already confirmed schema validity, unique IDs/nodes, processed-path existence, exact file sizes, and SHA agreement for all four Registry assets.

## Archive scope

The exact recommended archive commit contains nine files:

1. `world/assets/nature/processed/rocks/hongcun_rock_medium_01_v001.glb`
2. `world/assets/nature/manifests/nature_runtime_registry.json`
3. `world/assets/nature/validation/schemas/nature_registry.schema.json`
4. `docs/development/ROCK_MEDIUM_01_CLEAN_SOURCE_REPORT.md`
5. `docs/development/ROCK_MEDIUM_01_RUNTIME_PREP_REPORT.md`
6. `docs/development/ROCK_MEDIUM_01_EXPORT_REPORT.md`
7. `docs/development/ROCK_MEDIUM_01_GLB_VALIDATION.md`
8. `docs/development/ROCK_MEDIUM_01_REGISTRY_CHANGE_REPORT.md`
9. `docs/development/ROCK_MEDIUM_01_ARCHIVE_COMMIT_CHECK_REPORT.md`

The first eight files total 2,591,752 bytes before Git object compression. Including this report keeps the expected working-file payload near 2.6 MB. The exact Git pack size will only be known after staging/commit and must not be inferred from filesystem bytes.

The 2.54 MB GLB is small enough for normal Git under the current asset strategy. Git LFS is not required for this individual archive, although a future repository-wide binary policy may adopt LFS as the runtime library grows.

## Supporting documents not included in this exact archive scope

The following governing/planning documents are relevant but were not listed as required Runtime files and are currently mixed with broader untracked planning work:

- `docs/development/NATURE_NON_TREE_NAMING_POLICY.md`;
- `docs/development/NATURE_ROCK_ASSET_AUDIT.md`;
- `docs/development/NATURE_ROCK_BATCH_PRODUCTION_PLAN.md`;
- `docs/development/HONGCUN_NATURE_LIBRARY_V001_ROADMAP.md`.

Recommendation: archive these in a separately reviewed Nature policy/documentation commit, or explicitly expand the Rock archive scope before staging. Do not silently add them to the exact Runtime archive command below.

## Excluded files

Explicitly exclude:

```text
world/assets/nature/source/rocks/hongcun_rock_medium_01_clean_v001.blend
world/assets/nature/source/rocks/*.blend1
world/assets/nature/source/**
world/assets/nature/processed/trees/**
world/assets/nature/processing/**
world/assets/nature/previews/**
world/assets/architecture/**
world/geo/**
src/**
```

Also exclude:

- source ZIP and duplicate FBX/OBJ/glTF source-library files;
- failed exports and temporary validation outputs;
- CommonTree_05 and older CommonTree_A local artifacts;
- all unrelated documentation not named in the archive scope;
- `.DS_Store`, `.blend1`, temporary renders, caches, and recovery files;
- any pre-existing user work not explicitly approved for this commit.

The clean Blend remains local/cloud production source according to Nature archive policy. It is not a Runtime Git asset.

## Exact future Git add command

Do not run until a separate commit instruction is issued:

```bash
git add -- \
  world/assets/nature/processed/rocks/hongcun_rock_medium_01_v001.glb \
  world/assets/nature/manifests/nature_runtime_registry.json \
  world/assets/nature/validation/schemas/nature_registry.schema.json \
  docs/development/ROCK_MEDIUM_01_CLEAN_SOURCE_REPORT.md \
  docs/development/ROCK_MEDIUM_01_RUNTIME_PREP_REPORT.md \
  docs/development/ROCK_MEDIUM_01_EXPORT_REPORT.md \
  docs/development/ROCK_MEDIUM_01_GLB_VALIDATION.md \
  docs/development/ROCK_MEDIUM_01_REGISTRY_CHANGE_REPORT.md \
  docs/development/ROCK_MEDIUM_01_ARCHIVE_COMMIT_CHECK_REPORT.md
```

Never replace this command with `git add .`, `git add -A`, a directory path, or a wildcard.

## Mandatory post-staging checks

After a future authorized `git add`, stop before commit and run:

```bash
git diff --cached --name-status
git diff --cached --stat
```

Acceptance conditions:

- exactly nine paths are staged;
- no Architecture, Terrain/GIS, Tree, source Blend, processing, preview, React/R3F, package, or unrelated documentation file appears;
- Registry diff contains only the backward-compatible schema additions and `NAT_ROCK_MEDIUM_01` entry;
- standalone schema diff contains only matching optional `biome_roles` and `validation` properties;
- GLB remains the validated 2,542,280-byte file with SHA `635c5270…c6feb`.

If any unexpected path appears, stop. Do not use restore/reset without a separately reviewed correction step in this mixed worktree.

## Risks and decision

1. **Mixed dirty worktree — high scope risk.** Exact-path staging is mandatory.
2. **Standalone schema dependency.** Registry and schema must be committed atomically.
3. **Untracked governing documents.** They should receive a separate documentation archive decision rather than accidental inclusion.
4. **Binary performance advisory.** GLB validation passed with `doubleSided=true`; this is non-blocking and must not be silently changed during archive.
5. **Registry status.** Asset remains `validated`; `approved` and `archive_commit` must not be populated until the actual archive commit exists and a later reviewed update is authorized.
6. **Local remote state.** Ahead/behind is based on the local `origin/main` ref because this audit intentionally did not fetch.

Final decision: the Rock Runtime asset is ready for exact-scope staging in a later authorized milestone. It is not yet staged, committed, approved, or pushed.
