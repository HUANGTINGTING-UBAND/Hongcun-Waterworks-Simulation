# CommonTree_03 Registry Change Report

## Result

- Milestone: `008-O CommonTree_03 Registry Integration`
- Date: `2026-07-17`
- Status: **completed and validated**
- Registry: `world/assets/nature/manifests/nature_runtime_registry.json`
- Registry size after change: `6,510` bytes
- Registry SHA-256 after change: `97b61f95363b74a5d862fad16fa9b1236d5e464aea2c7194149b197644992f21`
- GLB or Blend modified: no
- CommonTree_5 processed: no
- Git operations: none

## Added runtime asset

| Field | Value |
|---|---|
| `asset_id` | `NAT_TREE_COMMON_03` |
| `source_name` | `CommonTree_3` |
| `production_name` | `Hongcun_Tree_Common_03` |
| `display_name` | `Hongcun Common Tree 03` |
| `usage_category` | `valley_tree` |
| Registry `category` | `valley_tree` |
| Status | `validated` |
| Validation report | `docs/development/COMMON_TREE_3_GLB_VALIDATION.md` |
| Archive commit | `null` |

Processed asset:

```text
world/assets/nature/processed/trees/hongcun_tree_common_03_v001.glb
```

| Field | Value |
|---|---|
| Version | `1` |
| Format | `glTF 2.0 Binary` |
| Bytes | `9,046,344` |
| SHA-256 | `63eeb2dde5ce7e80f1ff7eb2e0cb71514127de79ebee6ecdf80e9dcde5db4ee0` |

## LOD registration

| Level | Runtime node | Triangles |
|---:|---|---:|
| 0 | `NAT_TREE_COMMON_03_LOD0` | 3,505 |
| 1 | `NAT_TREE_COMMON_03_LOD1` | 1,947 |
| 2 | `NAT_TREE_COMMON_03_LOD2` | 905 |

Registered materials:

- `Hongcun_Tree_Common_03_Bark`
- `Hongcun_Tree_Common_03_Leaf`

Collision remains absent from the GLB and is marked `not_in_asset`; a gameplay placement proxy is still required. Instancing is allowed with shared materials required.

## Schema evolution

The Registry uses `additionalProperties: false`. To store the milestone's required identity and version fields without invalidating the document, these backward-compatible optional properties were added to `entrySchema`:

- entry: `source_name`;
- entry: `production_name`;
- entry: `usage_category`;
- processed object: `version`.

The original required fields and enums were not removed or weakened. `category` remains populated alongside `usage_category` so existing Registry consumers retain their current contract.

## Existing CommonTree_A preservation

The existing `hongcun_tree_common_a_v002` record remains present with its approved status, archive commit `045619c`, processed SHA-256 `2fb88ad035acdbd35ce5c681425c4a03f3234ecccf59d7e6104e2d6a2dbd03c9`, LOD data, materials, collision, and instancing metadata unchanged.

Registry asset count after integration: **2**.

## Validation

| Check | Result |
|---|---|
| JSON syntax parse | Pass |
| Embedded entry schema validation | Pass |
| Required properties | Pass |
| Type, enum, minimum and pattern constraints | Pass |
| `additionalProperties: false` constraints | Pass |
| CommonTree_A preservation check | Pass |
| New GLB file existence | Pass |
| New GLB byte-size match | Pass |
| New GLB SHA-256 match | Pass |
| LOD values match approved validation | Pass |

The environment did not contain the third-party Python `jsonschema` package. No dependency was installed. Validation was performed locally against the Registry's embedded schema rules, including required fields, types, enums, constants, minimums, patterns, array constraints, nested properties, and additional-property rejection.

## Scope integrity

Only the Registry and this report were changed. No GLB, Blend, CommonTree_A asset, CommonTree_5 asset, runtime code, or unrelated project content was modified. No Git command was executed.
