# CommonTree_3 LOD and Material Report

## Result

- Milestone: `008-K CommonTree_3 LOD and Material Production`
- Date: `2026-07-17`
- Status: **LOD production complete; material adaptation complete with leaf Alpha Clip validation pending**
- Blender: `4.2.22 LTS` through the existing GUI/MCP session
- Modified Blend: `world/assets/nature/source/trees/hongcun_tree_common_03_clean_v001.blend`
- Final Blend size: `10,831,436` bytes
- Final Blend SHA-256: `7048dcf54d120b282fda5fa5cf2fb9bc208a3cde2e778ae667ebd3e227a2d343`
- GLB export: not performed
- Git add/commit/push: not performed

The untouched `CommonTree_3` object remains in `Source_Reference`. Production changes are limited to the three `NAT_TREE_COMMON_03_LOD#` work objects and their copied runtime materials.

## LOD results

| LOD | Vertices | Triangles | Ratio to LOD0 | Required range | Result |
|---|---:|---:|---:|---:|---|
| `NAT_TREE_COMMON_03_LOD0` | 5,749 | 3,505 | 100.00% | Original topology | Pass |
| `NAT_TREE_COMMON_03_LOD1` | 4,214 | 1,947 | 55.55% | 50–60% | Pass |
| `NAT_TREE_COMMON_03_LOD2` | 3,188 | 905 | 25.82% | 20–30% | Pass |

### Material-region retention

| LOD | Bark triangles | Bark retention | Leaf triangles | Leaf retention |
|---|---:|---:|---:|---:|
| LOD0 | 1,705 | 100.00% | 1,800 | 100.00% |
| LOD1 | 1,227 | 71.96% | 720 | 40.00% |
| LOD2 | 545 | 31.96% | 360 | 20.00% |

Bark and leaves were reduced independently before being recombined. Lower LODs therefore remove leaf-region geometry more aggressively while retaining a larger proportion of trunk and main-branch geometry. This supports canopy readability and branch continuity better than a uniform whole-mesh reduction.

LOD0 retains the original production topology. All three runtime objects have identity Location/Rotation/Scale, origin Pivot, minimum mesh Z of `0.0`, and no unapplied modifiers.

The provenance object `CommonTree_3` remains unchanged at 5,749 vertices and 3,505 triangles with its original two materials. Its original minimum Z of `-0.242772` is intentionally preserved in `Source_Reference`.

## Material status

### Bark

- Runtime material: `Hongcun_Tree_Common_03_Bark`
- Role: opaque bark
- Color adaptation: HSV saturation `0.72`, value `0.90`
- Alpha input: unlinked; effective alpha `1.0`
- Alpha policy: `opaque_alpha_1`
- Blender 4.2 surface method: `DITHERED`; the material remains effectively opaque because no alpha drives the shader
- Shadow policy: material-derived Blender 4.2 behavior
- Textures: `Bark_NormalTree.png`, `Bark_NormalTree_Normal.png`
- Texture state: packed in the Blend; original external paths retained as provenance metadata

### Leaves

- Runtime material: `Hongcun_Tree_Common_03_Leaf`
- Role: leaf alpha material
- Color adaptation: HSV saturation `0.58`, value `0.84`
- Alpha input: linked from the leaf texture
- Blender 4.2 surface method: `DITHERED`
- Alpha policy: `dithered_source_pending_clip_validation`
- Shadow policy: material-derived Blender 4.2 behavior
- Texture: `Leaves_NormalTree_C.png`
- Texture state: packed in the Blend; original external path retained as provenance metadata

The adaptation lowers saturation and brightness toward the project's grey-green environment while preserving the source hand-painted texture character. No new texture or high-detail material was introduced.

## Validation notes

- LOD triangle-ratio gates: pass.
- LOD0 original topology: pass.
- Main-branch preservation strategy: pass by region-weighted reduction; bark retention exceeds leaf retention at both reduced levels.
- Runtime transforms and grounding: pass.
- Unapplied modifiers: none.
- Material assignment: two materials present on every LOD.
- Texture availability: pass; all referenced images are packed.
- Bark opacity: pass.
- Leaf alpha connectivity: pass.
- Final Alpha Clip cutoff, fringe, two-sided shadow, and transition review: **pending visual validation**. Blender 4.2 currently reports the imported/adapted leaf material as `DITHERED`, so this report does not claim a completed Alpha Clip approval.

## Scope integrity

No GLB was exported. CommonTree_A, CommonTree_5, Terrain, GIS, Architecture, and React/R3F assets were not modified. No Git staging, commit, or push operation was performed.
