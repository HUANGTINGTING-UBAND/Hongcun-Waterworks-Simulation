# World Geo Data

本目录管理《宏村水脉》的地理数据来源、生产配置和派生资产清单。

目录约定：

```text
world/geo/
├── schemas/       地理资产 manifest 规范
├── sources/       已登记的原始数据来源；不等同于最终游戏资产
├── processing/    GIS 中间产物目录
└── exports/       供 Blender/Houdini、GLB 或高度场管线使用的输出目录
```

规则：

- 原始 DEM、DSM、影像和 GIS 数据不得直接作为运行时资产。
- 每个来源必须提供 `terrain_asset_manifest.json`，记录来源、许可、坐标基准、范围、分辨率、哈希和归一化参数。
- `sources/` 中的记录只证明来源数据已进入生产管线，不代表完成历史复原或达到最终游戏质量。
- 大型二进制文件进入仓库前必须单独确认许可证、再分发条件和 Git LFS/外部存储策略。
- 运行时仍保留 D 级 `ProceduralTerrainProvider`；真实 GIS 数据须经过离线处理后再由未来的 `GISTerrainProvider` 加载。

生产流程见 [`docs/development/GIS_PIPELINE.md`](../../docs/development/GIS_PIPELINE.md)。
