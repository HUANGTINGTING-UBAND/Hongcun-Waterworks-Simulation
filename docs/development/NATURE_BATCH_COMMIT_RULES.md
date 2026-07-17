# Nature Batch Commit Rules

## 1. Purpose

These rules keep approved game-facing Nature assets reviewable and prevent third-party source libraries, failed conversions, Blender workfiles, and unrelated project changes from entering an asset archive commit.

Every asset archive uses exact-path staging. Broad staging is prohibited in a mixed worktree.

## 2. Allowed commit content

An approved Nature asset commit may include only files required to run, verify, and identify the processed asset:

- approved `world/assets/nature/processed/**/*.glb` files;
- final validation report for each included GLB;
- asset-specific processed manifest and runtime registry update;
- approved selection/production/validation specification when introduced or intentionally revised by the same milestone;
- neutral preview image only when it is an explicit acceptance artifact;
- narrowly scoped processing script only when separately reviewed and requested as part of pipeline provenance.

Every GLB must have a matching validation report, exact SHA-256, byte size, stable runtime ID, and status record.

## 3. Prohibited commit content

Do not commit:

- `*.blend` source or clean workfiles;
- `*.blend1` automatic backups;
- original third-party ZIP/DMG/archive packages;
- complete original FBX/OBJ/glTF source libraries;
- failed or superseded GLB exports;
- failure logs and temporary recovery reports in an approved-asset commit;
- temporary renders, audit images, viewport captures, caches, autosaves, or scratch scripts;
- demo scenes, forest/scattering scenes, terrain/world files, or runtime integration not authorized by the milestone;
- unrelated Architecture, Terrain, GIS, React/R3F, package, or configuration changes.

Source packages and editable source workfiles remain local or in the approved cloud archive with their source hashes and license records.

## 4. Naming and status gate

Before staging:

1. Confirm the candidate is the final approved version, not an earlier unsuffixed or failed candidate.
2. Confirm the validation report verdict is pass or pass with accepted advisory.
3. Confirm the runtime registry path/hash/size/LOD/material data matches the GLB.
4. Mark registry status `validated`; change to `approved` and set `archive_commit` only through the archive workflow.
5. Ensure failed reports are not referenced as final validation evidence.

## 5. Exact staging pattern

Use explicit paths for one reviewed batch. Example shape:

```bash
git add -- \
  world/assets/nature/processed/<category>/<approved-asset>.glb \
  world/assets/nature/manifests/<asset-manifest>.json \
  world/assets/nature/manifests/nature_runtime_registry.json \
  docs/development/<ASSET>_GLB_VALIDATION.md \
  docs/development/NATURE_BATCH_VALIDATION_SPEC.md
```

Never use:

```text
git add .
git add -A
git add world/
git add world/assets/nature/
git add docs/development/
```

## 6. Mandatory staged audit

After `git add` and before commit:

```bash
git diff --cached --name-status
git diff --cached --stat
git diff --cached --check
```

Review requirements:

- every staged path belongs to the approved Nature milestone;
- no source `.blend`, `.blend1`, archive, failed GLB, temporary file, or unrelated subsystem appears;
- staged GLB byte size and SHA-256 still match the validation report;
- manifest and registry JSON parse successfully;
- no unexpectedly large file or duplicate binary is staged;
- commit message identifies the approved Nature asset/batch.

If the staged list is wrong, stop and request scope correction. Do not commit merely because the binary is below the hosting size limit.

## 7. Git LFS policy

A single validated GLB below the repository hosting limit may use normal Git when consistent with current project policy. Reassess Git LFS before committing a large production batch or frequent binary revisions because GLB history can grow rapidly and binary deltas may be inefficient.

Do not introduce Git LFS implicitly during an asset commit. LFS adoption requires a separate repository policy decision, `.gitattributes` review, migration plan, and collaborator compatibility check.

## 8. Commit and push separation

- Preparation milestone: audit and propose exact paths only.
- Staging milestone: run exact `git add`, inspect cached output, then pause.
- Commit milestone: commit only after staged scope approval.
- Push milestone: push only after explicit user confirmation.

An asset is not marked with a final `archive_commit` until the commit exists. Remote archival is confirmed only after a separate push and remote-state check.
