# 宏村水脉 Project Bible v2.1

# DATA_SCHEMA_03_Building.md

版本：
Project Bible v2.1

文档类型：
Data Schema Design Document

用途：
定义《宏村水脉》建筑、民居、天井、街巷和聚落扩展的数据结构。

---

# 1. 系统定位

Building系统用于模拟宏村建筑如何在地形、水系和社会需求共同作用下形成。

核心逻辑：

```
Terrain
   ↓
Water
   ↓
Population
   ↓
Building
   ↓
Settlement
```

建筑不是独立资产，而是村庄生命系统的一部分。

---

# 2. 建筑类型

支持：

```
residence        民居
ancestral_hall   宗祠
public_building 公共建筑
bridge           桥梁
water_facility   水利设施
workshop         作坊
```

---

# 3. 民居数据结构

示例：

```json
{
"id":"house_001",
"type":"residence",
"yearBuilt":1520,
"location":[x,y,z],
"waterAccess":"canal",
"hasTianjing":true,
"fireResistance":80
}
```

字段：

- id：建筑编号
- type：建筑类型
- yearBuilt：建造时间
- location：空间位置
- waterAccess：水源连接方式
- hasTianjing：是否有天井
- fireResistance：消防能力

---

# 4. 水连接系统

建筑水源：

```
canal
pond
rainwater
none
```

影响：

- 日常用水
- 消防能力
- 居民满意度

---

# 5. 天井系统

天井模拟传统建筑内部水循环：

```
屋顶降雨

↓

天井收集

↓

储水

↓

家庭使用

↓

排水
```

数据：

```json
{
"id":"tianjing_001",
"type":"rain_collection",
"storage":500,
"connectedWater":true
}
```

---

# 6. 街巷系统

街巷受到：

- 地形
- 水圳
- 建筑关系

影响。

示例：

```json
{
"id":"street001",
"connect":["house001","house002"],
"width":2.5
}
```

---

# 7. 聚落扩展规则

新增建筑需要：

```
人口

+

资源

+

土地

+

水源
```

流程：

```
人口增长

↓

住房需求

↓

选择土地

↓

连接水系统

↓

形成新区
```

---

# 8. 灾害关联

建筑参数连接：

## 洪水

- 地基高度
- 距水距离

## 火灾

- 建筑密度
- 巷道宽度
- 水源距离

---

# 9. 数据目录

```
data/buildings/

building_schema.json

houses.json

streets.json

courtyards.json

bridges.json

settlement_growth.json
```

---

# 10. 程序接口

```
src/building/

BuildingLoader.ts

HouseGenerator.ts

TianjingSystem.ts

SettlementGrowth.ts

FireProtection.ts
```

---

# 11. Codex任务

TASK-BUILDING-001：
创建建筑数据结构。

TASK-BUILDING-002：
实现民居加载。

TASK-BUILDING-003：
实现天井系统。

TASK-BUILDING-004：
实现聚落扩展。

TASK-BUILDING-005：
实现建筑消防参数。

---

# 12. 验收标准

系统能够：

- 加载宏村民居；
- 根据水系统生成建筑；
- 模拟天井储水；
- 扩展聚落；
- 计算灾害风险。

---

# 13. 设计总结

Building数据库定义：

水如何塑造人的居住方式。

宏村不是建筑包围水，

而是水决定建筑，建筑适应水。

END
