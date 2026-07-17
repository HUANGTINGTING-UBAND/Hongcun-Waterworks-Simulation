# CommonTree_03 GLB Export Report

## Result

- Milestone: `008-M CommonTree_03 GLB Export`
- Date: `2026-07-17`
- Status: **export completed**
- Blender: `4.2.22 LTS`
- Execution environment: existing Blender GUI through MCP
- Background/headless Blender: not used
- GLB content validation: not performed in this milestone
- Git operations: none

## Input integrity

Input:

```text
world/assets/nature/source/trees/hongcun_tree_common_03_clean_v001.blend
```

| Check | Value | Result |
|---|---|---|
| Expected input SHA-256 | `7048dcf54d120b282fda5fa5cf2fb9bc208a3cde2e778ae667ebd3e227a2d343` | — |
| SHA-256 before export | `7048dcf54d120b282fda5fa5cf2fb9bc208a3cde2e778ae667ebd3e227a2d343` | Pass |
| SHA-256 after export | `7048dcf54d120b282fda5fa5cf2fb9bc208a3cde2e778ae667ebd3e227a2d343` | Pass |
| Blender dirty state before export | `false` | Pass |
| Blender dirty state after export | `false` | Pass |

The clean source was not saved or modified by the export operation.

## Output

```text
world/assets/nature/processed/trees/hongcun_tree_common_03_v001.glb
```

| Field | Value |
|---|---|
| Format | glTF 2.0 Binary (`.glb`) |
| File size | `9,046,344` bytes |
| SHA-256 | `63eeb2dde5ce7e80f1ff7eb2e0cb71514127de79ebee6ecdf80e9dcde5db4ee0` |
| Export completion time | `2026-07-17 18:03:50 +08:00` |
| Blender export operator result | `FINISHED` |

## Export scope

Only the three objects in the `Export` Collection were selected for export:

1. `NAT_TREE_COMMON_03_LOD0`
2. `NAT_TREE_COMMON_03_LOD1`
3. `NAT_TREE_COMMON_03_LOD2`

Export settings:

- glTF Binary (`GLB`);
- selection-only export;
- node names retained;
- material export enabled;
- automatic image format;
- transforms not applied during export;
- no additional modifier was added or applied.

## Materials

The selected Blender objects reference exactly two materials:

- `Hongcun_Tree_Common_03_Bark`
- `Hongcun_Tree_Common_03_Leaf`

## Texture source state

| Material | Image | Packed in source Blend |
|---|---|---|
| `Hongcun_Tree_Common_03_Bark` | `Bark_NormalTree` | Yes |
| `Hongcun_Tree_Common_03_Bark` | `Bark_NormalTree_Normal` | Yes |
| `Hongcun_Tree_Common_03_Leaf` | `Leaves_NormalTree_C` | Yes |

The textures were packed and available to Blender at export time. Confirmation of their final embedded GLB representation belongs to the next content-validation milestone and is not claimed here.

## Exporter messages

Blender completed the export but emitted these non-fatal warnings:

- the active vertex color was not exported because it was not used by the material node tree;
- one material path contained more than one image texture involved in a texture operation, so the glTF sampler follows the first image texture behavior;
- vertex-color alpha without vertex color is not managed by the exporter.

These warnings did not stop export. Because this milestone explicitly excludes GLB content validation, their runtime impact has not been evaluated here. The next validation should check material count and names, embedded images, foliage alpha behavior, bark normal mapping, transforms, and LOD node geometry.

## Scope integrity

No background Blender or additional Blender instance was started. CommonTree_A, CommonTree_5, the clean source Blend, and unrelated project assets were not modified. No registry update or Git operation was performed.
