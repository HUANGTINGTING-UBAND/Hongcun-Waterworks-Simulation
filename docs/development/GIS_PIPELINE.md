# 宏村水脉 Project Bible v2.1

# GIS_PIPELINE.md

版本：Milestone 004

用途：定义真实地理来源进入《宏村水脉》世界生产管线的处理、验证与运行时边界。

## 1. 核心流程

```text
DEM / DSM source
        ↓
来源、许可证、CRS、范围与哈希验证
        ↓
GIS processing
        ↓
投影、裁切、NoData、地形统计、坡度与水文参考
        ↓
Blender / Houdini
        ↓
地形清理、河流融合、艺术修正、LOD 与材质分区
        ↓
GLB / tiled heightfield
        ↓
terrain_asset_manifest.json
        ↓
TerrainProvider
        ↓
React Three Fiber
```

## 2. 来源分级

- A级真实来源：DEM、DSM、GIS、官方测绘或可验证遥感产品。
- B级生产修正：核心区雕刻、缺失区域修复、历史空间推演和游戏表现调整。
- D级程序化原型：用于镜头、空间关系和渲染管线验证。

真实来源不自动等于最终游戏资产。所有 B、D 级调整必须保留 `source`、`confidence` 和 `gameAdjustment`。

## 3. Source Manifest

每个地理来源必须登记：

- 数据集、产品、提供者、访问日期和 DOI；
- 许可证状态和 attribution；
- 水平 CRS 和垂直基准；
- bounds、pixel size、resolution 和 NoData；
- 原始文件与派生资产 SHA-256；
- 高程 normalization 参数；
- 来源资产与最终游戏资产的状态边界。

规范位于：

```text
world/geo/schemas/terrain_asset_manifest.schema.json
```

## 4. GIS Processing

离线 GIS 阶段负责：

1. 验证输入 CRS，不得无检查地覆盖 CRS。
2. 将加工数据转换到适合区域的米制投影。
3. 保留原始范围、像元尺寸、NoData 和垂直基准。
4. 生成地形统计、等高线、坡度、hillshade 和必要的水文参考。
5. 保持矩形物理范围，禁止将正方形 heightmap 无条件拉伸为正方形世界。
6. 记录命令、工具版本、重采样方法和输出哈希。

## 5. Blender / Houdini

该阶段负责：

- 区域地形网格生成；
- 核心区独立修正；
- 河床、河岸和水系融合；
- 道路和村落台地修正；
- 地表材质 mask；
- LOD、分块和 Web 性能优化；
- GLB 或分块高度场导出。

30 m DSM 不得直接决定月沼、南湖岸坡、水圳和建筑台地。

## 6. Web Assets

浏览器运行时只接收已经完成离线处理的资产：

- 优化后的 GLB；或
- 分块、带 LOD 的二进制高度场；
- 对应材质、mask 与资产 manifest。

禁止在 React 组件中运行 GDAL，禁止直接把 4033×4033 高度图转换为同等密度单一网格。

## 7. TerrainProvider 边界

当前保留：

```text
ProceduralTerrainProvider
```

职责：加载 D 级程序化原型，作为开发、测试和加载失败回退路径。

未来新增：

```text
GeoDataProvider
        ↓
GISTerrainProvider
```

- `GeoDataProvider` 负责来源 manifest、CRS、范围、哈希和许可验证。
- `GISTerrainProvider` 负责异步加载 Web-ready GLB 或高度场。
- 两者不得让运行时代码直接承担复杂 GIS 生产工作。

## 8. Copernicus GLO-30 登记

当前 Copernicus GLO-30 DSM 登记为：

```text
regional_terrain_source
```

状态：

```text
source_not_final_game_asset
```

其用途仅限区域山体、山脊和河谷骨架。原始文件和派生高度图在许可证、公开再分发与大文件存储方案确认前，不进入 Git 仓库。

## 9. 验收条件

真实地形进入运行时前必须满足：

- 来源、许可、attribution 和哈希完整；
- 水平与垂直坐标基准可追溯；
- Web 资产保留正确物理宽高比；
- 至少包含基础 LOD；
- 可与源 DSM 做抽样高度对照；
- 明确区分 A 级来源、B 级修正和 D 级原型；
- `ProceduralTerrainProvider` 回退路径仍可运行。
