# 宏村水脉 Project Bible v2.1

# DATA_SCHEMA_05_Timeline.md

版本：
Project Bible v2.1

文档类型：
Data Schema Design Document

用途：

定义《宏村水脉》历史时间线、阶段演化、事件触发和世界状态变化的数据结构。

---

# 1. 系统定位

Timeline系统负责控制：

- 地图变化；
- 水系变化；
- 建筑变化；
- NPC出现；
- 玩家章节推进。

核心思想：

时间不是背景，而是游戏机制。

---

# 2. 历史演化模型

宏村世界按照阶段演化：

```
自然环境

↓

村落形成

↓

水利规划

↓

水圳与月沼

↓

聚落扩张

↓

南湖建设

↓

完整宏村
```

---

# 3. 历史阶段定义

## Stage 0：自然环境阶段

状态：

- 雷岗山；
- 天然溪流；
- 原始地形。

目标：

让玩家理解选址基础。

---

## Stage 1：村落形成阶段

变化：

- 人口进入；
- 村址确定；
- 初始建筑形成。

解锁：

基础村庄地图。

---

## Stage 2：水利规划阶段

内容：

- 地形观察；
- 水源分析；
- 工程设计。

解锁：

水利设计方案。

---

## Stage 3：水圳与月沼阶段

核心：

```
引水

↓

堨坝

↓

水圳

↓

月沼
```

变化：

- 人工水网形成；
- 民居围绕水系发展。

---

## Stage 4：聚落扩展阶段

变化：

- 人口增加；
- 新住宅建设；
- 支圳增加。

解锁：

- 天井系统；
- 消防系统；
- 社会治理。

---

## Stage 5：南湖阶段

解锁条件：

```
人口

+

财富

+

社会组织能力

+

水利需求
```

完成：

南湖工程。

---

## Stage 6：完整宏村阶段

展示：

- 完整水系；
- 古建筑群；
- 四季变化；
- 八景系统。

---

# 4. Timeline数据结构

示例：

```json
{
"id":"stage_waterworks",
"yearStart":1400,
"type":"historical_stage",
"events":[
"build_canal",
"create_moonpond"
],
"unlock":[
"water_system"
]
}
```

---

# 5. 历史事件 Event

示例：

```json
{
"id":"event_001",
"type":"construction",
"year":1407,
"actor":[
"hu_zhong"
],
"result":[
"moon_pond_unlocked"
]
}
```

---

# 6. 世界状态 World State

记录某一时期宏村状态。

示例：

```json
{
"year":1500,
"waterSystem":true,
"moonPond":true,
"southLake":false,
"population":300
}
```

---

# 7. 系统连接

## 地图

Timeline

↓

Terrain State

↓

地图变化


## 水系统

Timeline

↓

Water Event

↓

水圳生成


## NPC

Timeline

↓

NPC Active

↓

任务开启


## 建筑

Timeline

↓

Building Unlock

↓

聚落变化

---

# 8. 数据目录

```
data/timeline/

├── timeline_schema.md

├── historical_events.json

├── stage_definitions.json

└── world_states.json
```

---

# 9. 程序接口

```
src/timeline/

├── TimelineEngine.ts

├── EventManager.ts

├── WorldState.ts

└── StageController.ts
```

---

# 10. Codex任务拆分

TASK-TIMELINE-001：

创建时间线结构。


TASK-TIMELINE-002：

实现阶段切换。


TASK-TIMELINE-003：

连接地图变化。


TASK-TIMELINE-004：

连接任务解锁。


TASK-TIMELINE-005：

创建历史事件系统。

---

# 11. 验收标准

系统能够：

- 切换历史阶段；
- 改变地图状态；
- 解锁水利工程；
- 控制NPC出现；
- 推进玩家章节。

---

# 12. 设计总结

Timeline数据库定义：

一条水如何在数百年中改变一个村庄。

它连接：

历史真实性

与

游戏互动体验。

END
