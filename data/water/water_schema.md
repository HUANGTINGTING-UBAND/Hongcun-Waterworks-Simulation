# 宏村水脉 Project Bible v2.1

# DATA_SCHEMA_02_Water.md

版本：
Project Bible v2.1

文档类型：
Data Schema Design Document

用途：

定义《宏村水脉》水利数据库结构，为水流模拟、水圳系统、月沼、南湖、洪水和维护系统提供统一数据规范。

---

# 1. 系统定位

Water系统是项目核心数据层。

宏村形成逻辑：

```
地形

↓

水源

↓

工程

↓

水圳

↓

月沼

↓

南湖

↓

村庄生活
```

水不是视觉效果，而是驱动整个世界运行的系统。

---

# 2. 水系统模型

采用：

Node + Edge 水网络模型。

```
西溪

↓

引水口

↓

堨坝

↓

主水圳

↓

支流水圳

↓

月沼

↓

南湖

↓

农田
```

---

# 3. Water Node 数据结构

关键水利位置定义为节点。

示例：

```json
{
"id":"xixi_intake",
"type":"water_source",
"elevation":125.6,
"capacity":1000,
"historicalStage":"waterworks",
"confidence":"A"
}
```

字段：

id：
节点编号。

type：
节点类型。

elevation：
高程。

capacity：
容量。

confidence：
历史可信等级。

---

# 4. 节点类型

包括：

```
water_source
river
dam
gate
canal
pond
lake
house_connection
farmland
```

---

# 5. Water Edge 数据结构

表示水流连接关系.

示例：

```json
{
"id":"canal_main_001",
"from":"dam_001",
"to":"moon_pond",
"length":1268,
"slope":0.003,
"width":1.2
}
```

参数：

length：
渠道长度。

slope：
坡度。

width：
渠宽。

---

# 6. 水流模拟规则

基础原则：

水流遵循自然高程。

计算因素：

- 高程差；
- 坡度；
- 渠道宽度；
- 水量；
- 闸门状态。

逻辑：

```
高程差

+

渠道参数

+

水量

=

流动状态
```

---

# 7. 月沼模型

月沼属于调蓄节点。

功能：

- 储水；
- 调节流量；
- 防洪；
- 公共生活。

示例：

```json
{
"id":"moon_pond",
"type":"reservoir",
"capacity":3000,
"waterLevel":0.8,
"quality":90
}
```

---

# 8. 南湖模型

南湖属于后期公共工程。

示例：

```json
{
"id":"south_lake",
"type":"lake",
"unlockCondition":{
"population":500,
"wealth":80,
"socialTrust":70
}
}
```

---

# 9. 水质系统

水质随时间变化。

流程：

```
上游水源

↓

水圳流动

↓

沉淀

↓

居民使用

↓

污染

↓

清淤恢复
```

数据：

```json
{
"quality":90,
"sediment":20,
"pollution":5
}
```

---

# 10. 洪水系统

暴雨事件：

↓

水量增加

↓

检测：

- 水圳容量；
- 月沼储量；
- 南湖调节能力。

结果：

合理设计：

降低灾害。

错误设计：

村庄受损。

---

# 11. 清淤维护

每条水路拥有维护周期。

示例：

```json
{
"id":"canal001",
"maintenanceCycle":30,
"lastCleanYear":1550
}
```

---

# 12. 数据目录

```
data/water/

├── water_nodes.json
├── water_edges.json
├── reservoirs.json
├── water_quality.json
├── maintenance.json
└── disaster_water.json
```

---

# 13. 程序接口

```
src/water/

├── WaterNode.ts
├── WaterGraph.ts
├── FlowEngine.ts
├── QualityEngine.ts
├── FloodSimulation.ts
└── MaintenanceSystem.ts
```

---

# 14. Codex任务拆分

TASK-WATER-001

创建水节点数据库。


TASK-WATER-002

创建水网络拓扑。


TASK-WATER-003

实现高程驱动水流。


TASK-WATER-004

实现水质变化。


TASK-WATER-005

实现洪水模拟。


TASK-WATER-006

实现清淤维护。


---

# 15. 验收标准

系统能够：

- 加载西溪；
- 创建引水点；
- 模拟堨坝调节；
- 运行水圳；
- 显示月沼蓄水；
- 扩展南湖；
- 模拟洪水和维护。

---

# 16. 设计总结

Water数据库定义：

宏村如何利用自然水源，通过工程和规则形成持续运行的生命系统。

END
