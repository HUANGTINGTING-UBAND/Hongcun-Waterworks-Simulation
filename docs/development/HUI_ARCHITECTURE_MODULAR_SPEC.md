# 宏村水脉 Project Bible v2.1

# Hui Architecture Modular Specification

版本：设计阶段 v0.1

用途：定义《宏村水脉》徽派民居游戏资产的模块拆分、尺寸、命名、Blender 建模、GLB 导出和程序化组合规则。

## 1. 设计边界

本规范依据上传的“徽派民居模块化设计参考图”进行游戏资产拆解。参考图不是测绘资料、最终贴图或需复制的成品设计。

本文尺寸属于 D 级游戏模块标准：

- `confidence: D`
- `gameAdjustment: true`
- 用于 Blockout、Snap、程序化组合和技术验证；
- 不代表宏村任何具体历史建筑的实测尺寸、结构做法或立面复原；
- 进入真实历史建筑生产前，必须用测绘、图纸、照片和建筑史资料建立独立 A/B 级资产记录。

禁止从参考图直接推断真实年代、家族、建筑等级、构造细节或装饰寓意。

## 2. 核心技术标准

- 1 Blender Unit = 1 metre。
- Blender Unit System：Metric，Unit Scale = 1.0。
- 模块主网格：1 m。
- 细部辅助网格：0.1 m；只用于檐口、门窗框和拼缝。
- 项目轴向：`+X = 模块长度/立面向右`，`+Y = 建筑进深/朝后`，`+Z = 向上`。
- 正立面朝向：本地 `-Y`。
- Pivot：位于模块底面中心，Z=0；Corner 模块 Pivot 位于两条 Snap 轴交点的底部。
- Object Scale 导出前必须为 `(1,1,1)`。
- Object Rotation 导出前必须为 `(0,0,0)`；镜像模块应生成正式左右版本，不能依赖负缩放。
- 所有拼接边界必须落在 0.5 m 或 1 m 网格上。
- GLB 中不得包含 Blender Camera、Light、测量体、参考图平面或未应用的测试对象。

## 3. 模块层级

```text
Architecture_Modular_Set
├── Wall
│   ├── White_Plaster_Wall_Solid
│   ├── White_Plaster_Wall_Window
│   ├── White_Plaster_Wall_Door
│   └── Corner_Wall
├── Roof
│   ├── Black_Tile_Roof_Small
│   ├── Black_Tile_Roof_Long
│   ├── Roof_Ridge
│   └── Roof_Eave
├── Gable
│   ├── HorseHeadWall_Small
│   ├── HorseHeadWall_Medium
│   └── HorseHeadWall_Large
├── Foundation
│   ├── Stone_Base_1m
│   └── Stone_Base_Corner
├── Window
│   ├── Wooden_Window_Small
│   └── Wooden_Window_Large
└── Door
    ├── Wooden_Door_Single
    └── Wooden_Door_Double
```

每个分类在 Blender 中使用独立 Collection；组合建筑不得覆盖原始模块。

## 4. 模块列表与尺寸

### 4.1 Wall

墙体统一高度为 3.0 m，其中包含 0.5 m 石基对齐区和 2.5 m 白灰墙可见区。墙体厚度采用 0.20 m 的游戏标准值；该值不是历史实测。

| 模块 | 长度 X | 厚度 Y | 高度 Z | 开口 |
|---|---:|---:|---:|---|
| White_Plaster_Wall_Solid_1m | 1.0 | 0.20 | 3.0 | 无 |
| White_Plaster_Wall_Solid_2m | 2.0 | 0.20 | 3.0 | 无 |
| White_Plaster_Wall_Solid_3m | 3.0 | 0.20 | 3.0 | 无 |
| White_Plaster_Wall_Solid_5m | 5.0 | 0.20 | 3.0 | 无 |
| White_Plaster_Wall_Window_1m | 1.0 | 0.20 | 3.0 | 适配 1.0 m 窗模块 |
| White_Plaster_Wall_Window_2m | 2.0 | 0.20 | 3.0 | 居中 1.0/1.5 m 窗口配置 |
| White_Plaster_Wall_Door_1m | 1.0 | 0.20 | 3.0 | 适配 1.0 m 单门 |
| White_Plaster_Wall_Door_1_5m | 1.5 | 0.20 | 3.0 | 适配 1.5 m 双门 |
| Corner_Wall_1m | 两翼各 1.0 | 0.20 | 3.0 | 90° 内/外角连接 |

规则：

- 墙开口必须在建模阶段形成真实几何边界，禁止用黑色贴图伪造洞口。
- 开口模块与门窗模块分离，便于替换。
- Solid 长墙用于减少 Draw Call，但必须保持 1 m Snap 端点。
- Corner_Wall 负责转角连续性；普通墙禁止在角点互相穿插。

### 4.2 Foundation

| 模块 | 长度 X | 进深 Y | 高度 Z |
|---|---:|---:|---:|
| Stone_Base_1m | 1.0 | 0.24 | 0.5 |
| Stone_Base_2m | 2.0 | 0.24 | 0.5 |
| Stone_Base_3m | 3.0 | 0.24 | 0.5 |
| Stone_Base_5m | 5.0 | 0.24 | 0.5 |
| Stone_Base_Corner | 两翼各 1.0 | 0.24 | 0.5 |

- 石基底部 Z=0，墙体与石基共享水平 Snap。
- 石基可以比墙面向外突出 0.02–0.04 m，以形成阴影线；具体值属于游戏美术调整。
- 不用 RockPath 等通用自然资产替代石基或历史铺装。

### 4.3 Window

| 模块 | 宽度 X | 参考高度 Z | 推荐窗台高度 |
|---|---:|---:|---:|
| Wooden_Window_Small | 1.0 | 1.2 | 0.9–1.0 |
| Wooden_Window_Large | 1.5 | 1.4 | 0.8–0.9 |

- Pivot 位于窗框底部中心，不位于建筑地面。
- 墙体开口提供 `OpeningAnchor`；窗模块 Pivot Snap 到该 Anchor。
- 窗框、格心、遮檐分层命名，方便 LOD 简化。
- 具体窗棂纹样不得凭参考图直接复制为历史事实。

### 4.4 Door

| 模块 | 宽度 X | 参考高度 Z | 类型 |
|---|---:|---:|---|
| Wooden_Door_Single | 1.0 | 2.2 | 单扇/单开间 |
| Wooden_Door_Double | 1.5 | 2.4 | 双扇/主入口 |

- Pivot 位于门框底部中心。
- 动态门扇应与静态门框拆分；门扇铰链 Pivot 位于转轴底部。
- 第一阶段 GLB 可只导出静态关闭版本，但命名必须预留动画版本。
- 不添加现代门锁、玻璃幕门、金属卷帘等元素。

### 4.5 Roof

屋面采用“坡面主体 + 屋脊 + 屋檐”分层，避免每栋建筑重复高密度瓦片几何。

| 模块 | 建筑跨度/宽度 | 沿屋脊长度 | 说明 |
|---|---:|---:|---|
| Black_Tile_Roof_Small_3m | 3.0 | 1/2/3 m 组合 | 小跨度坡屋面 |
| Black_Tile_Roof_Long_5m | 5.0 | 1/2/3/5 m 组合 | 主体长屋面 |
| Roof_Ridge_1m | — | 1.0 | 标准屋脊段 |
| Roof_Ridge_2m | — | 2.0 | 中段优化 |
| Roof_Ridge_5m | — | 5.0 | 长段优化 |
| Roof_Eave_1m | — | 1.0 | 标准檐口段 |
| Roof_Eave_Corner | — | 角部 | 檐口收边 |

推荐游戏 Blockout 屋面参数：

- 3 m 跨度：檐口高度约 2.7–2.9 m，屋脊高度约 3.5–3.8 m。
- 5 m 跨度：檐口高度约 2.7–2.9 m，屋脊高度约 3.8–4.3 m。
- 出檐：0.35–0.50 m。

以上为构图与拼接范围，不是实测。坡度、举折、瓦作和檐口细节必须在历史资料支持后调整。

屋瓦策略：

- 近景模块允许简化瓦垄几何；
- 中景使用法线、粗糙度和低密度轮廓瓦；
- 远景使用完整屋面块面，不保留单瓦几何；
- 禁止每片瓦独立对象；
- 屋面边缘、屋脊和檐口必须保留清晰剪影。

### 4.6 Gable

| 模块 | 对应跨度 | 建议总高 | 用途 |
|---|---:|---:|---|
| HorseHeadWall_Small | 3.0 m | 约 3.6–4.0 m | 小体量侧墙 |
| HorseHeadWall_Medium | 4.0 m | 约 4.0–4.6 m | 中体量或层级过渡 |
| HorseHeadWall_Large | 5.0 m | 约 4.4–5.2 m | 主体轮廓节点 |

- Pivot 位于山墙底部中心。
- 左右方向必须有正式变体：`L`、`R` 或对称版本。
- 马头墙用于控制建筑群轮廓，不应每个开间都重复。
- Small/Medium/Large 表示游戏轮廓等级，不代表真实建筑等级。
- 具体叠落、脊饰、收头和比例必须接受建筑史审核。

## 5. Snap 规范

### 5.1 Snap 点

每个模块在 Blender 中建立 Empty 或命名顶点组作为制作辅助，导出前可移除辅助 Empty：

```text
SNAP_L        左端
SNAP_R        右端
SNAP_FRONT    正立面基准
SNAP_BACK     背立面基准
SNAP_TOP      墙顶/屋面安装线
SNAP_OPENING  门窗开口中心
SNAP_RIDGE    屋脊连接点
SNAP_EAVE     檐口连接点
```

程序化运行时不依赖 Blender Empty；Snap 数据进入模块 manifest 或 GLB node extras。

### 5.2 接缝容差

- 位置误差目标：≤ 0.001 m。
- 墙/石基接缝不得出现可见缝隙或 Z-fighting。
- 共面模块允许 0.002–0.005 m 的受控内缩，必须全套一致。
- 屋面搭接使用固定规则，禁止每栋随机偏移。
- Corner 模块优先解决法线、UV 和材质连续性。

## 6. 命名规则

文件使用小写 ASCII 与下划线：

```text
arch_hui_<category>_<module>_<size>_<variant>_v###.glb
```

Blender 对象：

```text
ARCH_HUI_<CATEGORY>_<MODULE>_<SIZE>_<VARIANT>_LOD#
```

示例：

```text
ARCH_HUI_WALL_WHITE_SOLID_1M_A_LOD0
ARCH_HUI_WALL_WHITE_WINDOW_1M_A_LOD0
ARCH_HUI_ROOF_BLACK_TILE_5M_A_LOD0
ARCH_HUI_GABLE_HORSEHEAD_MEDIUM_L_LOD0
ARCH_HUI_DOOR_WOOD_DOUBLE_1_5M_A_LOD0
```

材质：

```text
MAT_ARCH_HUI_WHITEWALL_A
MAT_ARCH_HUI_BLACKTILE_A
MAT_ARCH_HUI_WOOD_A
MAT_ARCH_HUI_STONE_A
```

贴图：

```text
T_ARCH_HUI_<SET>_<BC|N|R|M|AO>_<SIZE>.<ext>
```

禁止中文文件名、空格、负缩放变体及 `.001` 自动后缀进入最终资产。

## 7. 材质规范

### WhiteWall

- 颜色：低饱和暖白灰或中性白灰，不使用纯白 `#FFFFFF`。
- 表面：微粗糙、轻微不均匀；避免强烈程序噪声和夸张裂纹。
- Roughness：中高范围。
- 老化：只允许 B/D 级可控 mask；不得用污渍虚构具体历史状态。

### BlackTile

- 颜色：深灰、灰蓝黑、黛色，不使用纯黑。
- Roughness：高；湿润版本可局部降低，但不形成镜面。
- 法线：强调瓦垄方向，不制造过深沟槽。
- 屋面整体先保证轮廓，再增加瓦片细节。

### Wood

- 颜色：暗棕、灰棕、低红度。
- Roughness：中高。
- 门窗格心优先依靠几何轮廓，纹理不替代主要结构。
- 禁止高饱和红木、金漆和现代清漆效果。

### Stone

- 颜色：青灰、灰褐、低饱和。
- Roughness：高；底部可用湿润 mask 轻微加深。
- 石块分缝要控制尺度，不能像砖墙贴纸般重复。
- 不根据参考图宣称真实石材类型。

### 材质性能

- 优先使用四个共享材质集，不为每个模块复制材质。
- Base Color 使用 sRGB；Normal/Roughness/Mask 使用 Non-Color。
- 模块化接缝两侧 texel density 必须一致。
- Web 目标优先评估 KTX2/Basis；GLB 生产前记录贴图压缩方式。

## 8. Blender 建模规则

### 8.1 文件结构

```text
Architecture_Module_Work
├── Reference
├── Blockout
├── Source_High
├── Game_Mesh
├── Collision
├── LOD
├── Snap_Guides
└── Export
```

- `Reference` 仅保存外部参考链接或临时平面，不进入导出。
- `Blockout` 先通过尺寸和 Snap 验收，再制作细节。
- `Game_Mesh` 保留可编辑非破坏 modifier；导出副本应用 modifier。
- `Export` 每次只放一个模块及正式 collision/LOD。

### 8.2 几何规则

- 墙面保持干净拓扑，隐藏面可删除，但模块接口边界要完整。
- 开口边缘需要足够倒角支持光照，不使用无限锐利边。
- 倒角尺度全套一致，避免不同模块拼接时风格跳变。
- 门窗、檐口、屋脊使用独立对象，便于替换和 LOD。
- 细节不突破模块边界，除非属于明确的 eave/ridge overlap。
- 禁止现代雨水管、空调、铝合金窗、现代电表和现代灯具。
- 禁止彩色装饰、过度卡通化和高饱和材质。

### 8.3 法线与 UV

- 应用 Scale 后再计算法线。
- 硬边与 UV seam 规则保持一致。
- 避免跨 90° 转角的错误平滑。
- 共享材质保持统一 texel density。
- 屋瓦 UV 方向必须与排水坡向一致。
- Lightmap UV 仅在目标平台需要时生成并登记。

### 8.4 LOD

- LOD0：保留门窗主要格心、檐口与屋面轮廓。
- LOD1：简化细窗棂、瓦垄和次要脊饰。
- LOD2：保留墙、屋面、马头墙和大开口剪影。
- 远景建筑群可使用合并块或 impostor，但不得改变山—村轮廓关系。
- LOD 切换距离由未来性能测试确定，不硬编码在模型文件中。

## 9. GLB 导出规则

标准流程：

```text
Blender Game_Mesh
    ↓
尺寸 / Pivot / Snap / 法线 / UV 检查
    ↓
导出副本应用确认过的 modifier
    ↓
只选择 Export Collection
    ↓
glTF 2.0 Binary (.glb)
    ↓
重新解析并验证
    ↓
asset manifest + SHA-256
```

导出检查：

- 单位为 metre，Scale `(1,1,1)`。
- Pivot 符合模块底部中心规则。
- 不包含 Reference、Camera、Light、Snap Empty 或测试组合。
- 材质名称与项目规范一致。
- 贴图嵌入或使用明确、可移植的发布策略。
- GLB bounds 与 Blender 模块尺寸一致。
- GLB node 不含多余对象。
- 每个输出记录顶点、三角形、材质、贴图、LOD 和 SHA-256。

R3F 加载边界：

- React Three Fiber 只加载处理完成的 GLB prefab。
- 组件不得直接读取 Blender 源文件。
- 模块 manifest 提供尺寸、Pivot、Snap、分类和允许连接关系。
- 运行时重复模块使用 instancing；门等动态部件单独加载或作为命名子节点。
- 建筑生成器不能靠模型包围盒猜测 Snap；必须使用登记数据。

## 10. 程序化组合规则

### 10.1 基本生成顺序

```text
Footprint / Bay Count
        ↓
Foundation Grid
        ↓
Wall Solid / Window / Door bays
        ↓
Corner resolution
        ↓
Roof span and ridge
        ↓
Gable silhouette
        ↓
Door / Window inserts
        ↓
Material and LOD assignment
        ↓
Validation
```

### 10.2 开间规则

- 基本开间单位：1 m 游戏网格。
- 推荐 Blockout 面宽：3、5、7 开间。
- 主入口优先位于奇数开间组合的中心轴。
- 窗、门和 Solid Wall 只能替换同宽开间。
- 1.5 m 双门需要专用 1.5 m 开口，不允许缩放 1 m 单门获得。
- 长墙可以用 2/3/5 m 优化模块替代连续 1 m 模块，但 Snap 结果必须一致。

### 10.3 屋面规则

- 屋顶跨度必须与建筑进深匹配：3 m 或 5 m 系列不可无记录拉伸。
- Roof_Ridge 长度等于建筑主体长度。
- Roof_Eave 沿立面连续拼接，Corner 使用专用收边。
- 马头墙位于允许的山墙端或院落轮廓节点，不按每开间重复。
- 屋面、山墙与檐口组合必须通过穿插检查。

### 10.4 院落与转角

- L/U/院落组合由直墙、Corner_Wall 和正式屋面转折模块构成。
- 转角必须先解决墙体，再解决石基和檐口。
- 内院宽度、建筑间距和消防/排水逻辑不由本 D 级模块规则自动认定为历史真实。
- 在获得真实街巷与院落资料前，程序化组合只用于原型和非特定建筑填充。

### 10.5 变体控制

允许的变体：

- Solid/Window/Door 开间替换；
- 3/5 m 屋面跨度；
- Small/Medium/Large 马头墙轮廓；
- 低饱和材质轻微变化；
- 门窗 LOD 和关闭/开启状态。

禁止的随机化：

- 随机改变层高、屋面跨度和门窗尺度；
- 随机生成不受支持的历史纹样；
- 高饱和彩色装饰；
- 现代建筑构件；
- 用负缩放制造左右模块；
- 让石基、墙、檐口或屋脊产生无法解释的断裂。

## 11. 模块 manifest 最低要求

每个 GLB 模块必须登记：

```text
id
category
moduleName
source
confidence
gameAdjustment
dimensionsMetres
pivot
axisConvention
snapPoints
compatibleModules
materials
vertexCount
triangleCount
lod
sha256
license
historicalEvidenceStatus
```

设计参考生成的首轮模块统一标记：

```json
{
  "confidence": "D",
  "gameAdjustment": true,
  "historicalEvidenceStatus": "modular_game_design_reference_only"
}
```

## 12. 首轮资产生产建议

下一阶段先制作低模 Blockout，不做最终纹理：

1. `Stone_Base_1m`
2. `White_Plaster_Wall_Solid_1m`
3. `White_Plaster_Wall_Window_1m`
4. `White_Plaster_Wall_Door_1m`
5. `Corner_Wall_1m`
6. `Black_Tile_Roof_Small_3m`
7. `Roof_Ridge_1m`
8. `Roof_Eave_1m`
9. `HorseHeadWall_Small`
10. `Wooden_Window_Small`
11. `Wooden_Door_Single`

用这些模块只建立一个隔离的 3 开间测试组合，验证尺寸、Pivot、Snap、GLB 和 R3F 可加载性。测试通过后再扩展 5/7 开间、双门、长屋面和中大型马头墙。

当前阶段不创建 Blender 文件、GLB、程序化生成器或 React 组件。

