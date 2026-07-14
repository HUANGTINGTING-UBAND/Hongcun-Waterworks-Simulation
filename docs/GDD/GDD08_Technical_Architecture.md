# 宏村水脉
# Hongcun Water Pulse

# GDD08_Technical_Architecture.md

版本：
Project Bible v2.1

文档类型：
Game Design Document

用途：

定义《宏村水脉》的技术架构、程序模块、数据驱动方式以及网页游戏实现规范。

---

# 1. 技术目标

建立一个：

数据驱动、可扩展、适合网页运行的历史模拟系统。

目标：

- 支持三维宏村场景；
- 支持历史阶段演化；
- 支持水利模拟；
- 支持NPC任务系统；
- 支持历史数据库扩展。

---

# 2. 总体技术架构

```
用户界面

↓

React Application

↓

Game State Management

↓

Terrain / Water / Building / NPC / Quest

↓

Data Layer

↓

Historical Database + JSON + 3D Assets
```

---

# 3. 前端技术栈

## React

负责：

- 页面结构；
- UI组件；
- 信息面板；
- 用户交互。

---

## TypeScript

负责：

- 类型安全；
- 数据接口；
- 模块通信。

---

## React Three Fiber

负责：

- 三维场景；
- 地形显示；
- 水体表现；
- 建筑加载。

---

## Three.js

负责：

- WebGL渲染；
- 动画；
- 3D对象管理。

---

# 4. 三维资产流程

```
GIS数据

↓

DEM高程

↓

Blender地形模型

↓

GLB资产

↓

React Three Fiber

↓

网页展示
```

---

# 5. 数据驱动架构

核心原则：

代码不保存历史。

代码读取数据。

错误：

```javascript
if(year===1607){
 buildMoonPond()
}
```

正确：

```json
{
"year":1607,
"event":"moon_pond_complete"
}
```

---

# 6. 数据目录设计

```
data/

├── terrain/

├── water/

├── buildings/

├── npc/

├── quests/

├── timeline/

├── economy/

└── disasters/
```

---

# 7. 核心程序模块

## Terrain Engine

负责：

- 地形加载；
- 高程计算；
- 可建设区域。

目录：

```
src/terrain/
```

---

## Water Engine

负责：

- 水流；
- 水位；
- 水质；
- 洪水模拟。

目录：

```
src/water/
```

---

## Building Engine

负责：

- 民居；
- 天井；
- 聚落增长。

目录：

```
src/building/
```

---

## NPC Engine

负责：

- NPC状态；
- 对话；
- 事件。

目录：

```
src/npc/
```

---

## Quest Engine

负责：

- 任务；
- 阶段；
- 解锁。

目录：

```
src/quest/
```

---

# 8. 游戏状态管理

统一管理：

Game State。

示例：

```typescript
interface GameState {

year:number;

stage:string;

population:number;

waterLevel:number;

completedTasks:string[];

}
```

---

# 9. 时间系统

采用：

历史阶段驱动。

不是单纯实时年份推进。

示例：

```json
{
"stage":"waterworks",
"unlock":[
"canal",
"moonpond"
]
}
```

---

# 10. 保存系统

保存：

- 当前年份；
- 工程状态；
- 村庄状态；
- 玩家选择；
- 任务进度。

格式：

JSON。

---

# 11. 性能策略

网页环境需要考虑：

- 模型数量；
- 贴图大小；
- 加载速度。

采用：

- LOD；
- 分块加载；
- 数据懒加载。

---

# 12. GitHub项目结构

```
Hongcun-Waterworks-Simulation/

src/

data/

assets/

docs/

historical_database/

tests/
```

---

# 13. Codex开发流程

每个开发任务：

1. 阅读项目文档；
2. 确认数据结构；
3. 制定修改计划；
4. 编写代码；
5. 执行测试；
6. 更新文档。

---

# 14. 开发阶段

## Phase 0

基础框架：

- React工程；
- 数据结构；
- 文档体系。

---

## Phase 1

数字宏村：

- 地形；
- 河流；
- 三维场景。

---

## Phase 2

水系统：

- 水圳；
- 月沼；
- 水流模拟。

---

## Phase 3

历史模拟：

- NPC；
- 任务；
- 工程。

---

## Phase 4

完整体验：

- 聚落演化；
- 南湖；
- 四季系统；
- 摄影模式。

---

# 15. 验收标准

完成后：

系统能够：

- 加载宏村三维地图；
- 读取历史数据；
- 显示水系；
- 运算水流；
- 加载建筑；
- 运行NPC任务。

---

# 16. 设计总结

技术架构服务于历史体验。

最终目标：

建立一个真实、可扩展、可持续开发的宏村数字历史系统。

END
