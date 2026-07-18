# Rock Medium 01 Registry Change Report

## Result

Milestone 009-I completed successfully. The validated Runtime asset `NAT_ROCK_MEDIUM_01` was added to `world/assets/nature/manifests/nature_runtime_registry.json` with its exact GLB path, size, SHA-256, LOD nodes and triangle counts, material, collision policy, validation status, report reference, and requested biome metadata.

The Registry's embedded entry schema and the standalone Validator schema received the same minimal backward-compatible extension for `biome_roles` and nested `validation`. All four Registry entries pass both schemas. Existing CommonTree_A, `NAT_TREE_COMMON_03`, and `NAT_TREE_COMMON_05` entry content remains unchanged.

No GLB or Blend was modified. Blender and Git operations were not performed.

## Before and after Registry

| Property | Before | After |
|---|---|---|
| Registry path | `world/assets/nature/manifests/nature_runtime_registry.json` | Same |
| Schema version | `1.0.0` | `1.0.0` |
| Asset count | 3 | 4 |
| Registry SHA-256 | `a3394d2e21835f927a8b15b8710f8115b31db714af7d2ea659595cc7988d5fb7` | `b0e14c3cf00e0a1f13bf01bba032e12058c53649e5c2bbcc19b9aa9fca853eca` |
| Registry bytes | 8,011 | 10,157 |
| New asset | None | `NAT_ROCK_MEDIUM_01` |

The hash and size changes are expected from the additive schema properties and one new asset entry.

## Existing asset immutability audit

Each pre-existing entry was canonicalized with sorted JSON keys and hashed before and after the change.

| Existing asset | Before normalized SHA-256 | After normalized SHA-256 | Result |
|---|---|---|---|
| CommonTree_A Registry key `hongcun_tree_common_a_v002` | `3b8453ec4f04ebf933079ebf1a04bc3fa3ee211fa5e083b0b9da1c997705e375` | `3b8453ec4f04ebf933079ebf1a04bc3fa3ee211fa5e083b0b9da1c997705e375` | Unchanged |
| `NAT_TREE_COMMON_03` | `44d2e5f1a218c79354594d12eb8c7601e825fc65717e14eca707951504f68e1d` | `44d2e5f1a218c79354594d12eb8c7601e825fc65717e14eca707951504f68e1d` | Unchanged |
| `NAT_TREE_COMMON_05` | `11627e14e82118d0b9facfc0ac863e5c27e94190250cb4d966affb9437e121a4` | `11627e14e82118d0b9facfc0ac863e5c27e94190250cb4d966affb9437e121a4` | Unchanged |

No existing Tree path, hash, size, node, LOD count, material, status, validation reference, or archive commit was changed.

## New Runtime asset

```json
{
  "asset_id": "NAT_ROCK_MEDIUM_01",
  "source_name": "Rock_Medium_1",
  "production_name": "Hongcun_Rock_Medium_01",
  "display_name": "Hongcun Medium Rock 01",
  "usage_category": "rock",
  "biome_roles": [
    "stream_bank",
    "mountain_slope"
  ],
  "category": "rock",
  "processed": {
    "path": "world/assets/nature/processed/rocks/hongcun_rock_medium_01_v001.glb",
    "version": 1,
    "format": "glTF 2.0 Binary",
    "sha256": "635c527039c2a4a500606910238a249329bd11aa68b6b12f945ef48cd04c6feb",
    "bytes": 2542280
  },
  "lod": [
    {
      "level": 0,
      "node": "NAT_ROCK_MEDIUM_01_LOD0",
      "triangles": 342
    },
    {
      "level": 1,
      "node": "NAT_ROCK_MEDIUM_01_LOD1",
      "triangles": 188
    }
  ],
  "materials": [
    "Hongcun_Rock_Medium_01_Material"
  ],
  "collision": {
    "type": "simple_proxy",
    "status": "not_in_asset",
    "required_before_gameplay_placement": true
  },
  "status": "validated",
  "validation": {
    "status": "validated",
    "report": "docs/development/ROCK_MEDIUM_01_GLB_VALIDATION.md"
  },
  "validation_report": "docs/development/ROCK_MEDIUM_01_GLB_VALIDATION.md",
  "archive_commit": null
}
```

The complete Registry entry also records source library/manifest provenance and instancing rules.

## Hash and file agreement

| Check | Expected | Observed | Result |
|---|---|---|---|
| GLB path exists | `world/assets/nature/processed/rocks/hongcun_rock_medium_01_v001.glb` | Same | Pass |
| GLB bytes | 2,542,280 | 2,542,280 | Pass |
| GLB SHA-256 | `635c527039c2a4a500606910238a249329bd11aa68b6b12f945ef48cd04c6feb` | Same | Pass |
| LOD0 node/triangles | `NAT_ROCK_MEDIUM_01_LOD0` / 342 | Same | Pass |
| LOD1 node/triangles | `NAT_ROCK_MEDIUM_01_LOD1` / 188 | Same | Pass |
| Material | `Hongcun_Rock_Medium_01_Material` | Same | Pass |

Asset byte integrity after Registry integration:

| Immutable asset | SHA-256 |
|---|---|
| GLB | `635c527039c2a4a500606910238a249329bd11aa68b6b12f945ef48cd04c6feb` |
| Source Blend | `63dc965bda15f6550194bc503274ce1432e587ded3cbcdb7c0c80562a8681274` |

Both hashes remain identical to the pre-integration records.

## Validation reference

Registry validation status:

```text
status = validated
validation.status = validated
```

Validation report:

```text
docs/development/ROCK_MEDIUM_01_GLB_VALIDATION.md
```

The report records:

- valid glTF 2.0 container and chunks;
- exact LOD0/LOD1 Runtime nodes;
- 342 / 188 triangles;
- identity transforms;
- glTF Y-up grounding at `Y = 0.0`;
- OPAQUE material;
- embedded texture with no external URI;
- collision excluded from the visual GLB;
- no validation errors.

The new Registry status is `validated`, not `approved`. Approval and `archive_commit` require a separate Git archive milestone.

## Biome metadata

Recorded exactly as requested:

```json
"biome_roles": [
  "stream_bank",
  "mountain_slope"
]
```

These values are game-art placement metadata. They do not:

- enter the Runtime ID;
- authorize world placement;
- override slope, water-flow, architecture, navigation, or protected-view exclusions;
- claim surveyed Hongcun geology or historic stone distribution.

The metadata remains D-level and requires a later placement-system review before use.

## Schema extension

Files changed for schema compatibility:

- Registry embedded `entrySchema`;
- `world/assets/nature/validation/schemas/nature_registry.schema.json`.

Added optional properties:

```text
biome_roles: non-empty unique string array
validation: object with required status and optional report
```

The fields are optional for backward compatibility, so frozen and existing Tree entries remain valid without migration.

Standalone schema after-change record:

| Field | Value |
|---|---|
| Path | `world/assets/nature/validation/schemas/nature_registry.schema.json` |
| Bytes | 5,032 |
| SHA-256 | `57c78b1a865d46fce2c6e73711838f19714d90a28b7442f8c70fb4e4e05a46ec` |

## Registry Validator result

The project read-only Validator passed:

- Registry JSON parse;
- standalone v001 Schema;
- embedded entrySchema for all four entries;
- asset ID uniqueness;
- Runtime LOD node uniqueness;
- processed path existence for all four assets;
- processed byte-size agreement for all four assets;
- processed SHA-256 agreement for all four assets.

Entry schema errors:

```text
hongcun_tree_common_a_v002: []
NAT_TREE_COMMON_03: []
NAT_TREE_COMMON_05: []
NAT_ROCK_MEDIUM_01: []
```

Final Registry integration status: **PASS**.

## Safety statement

Modified:

- `world/assets/nature/manifests/nature_runtime_registry.json`;
- `world/assets/nature/validation/schemas/nature_registry.schema.json`;
- this change report.

Not modified or executed:

- no GLB modification;
- no Blend modification;
- no Blender operation;
- no Tree Registry entry modification;
- no Git add, commit, or push;
- no world placement or Runtime loader integration.
