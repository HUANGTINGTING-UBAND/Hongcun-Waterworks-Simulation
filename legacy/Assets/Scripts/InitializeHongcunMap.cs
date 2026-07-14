using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 徽州宏村历史时代枚举
/// </summary>
public enum HongcunEra
{
    Era0_PreMing = 0,    // 序章/自然村落：仅西溪河与极古老建筑（如古戏台、古银杏树），无人工水网
    Era1_Yongle = 1,     // 明永乐·蓝图奠基：开凿石碣水口、主水圳、月沼。解锁汪氏宗祠、乐叙堂
    Era2_Wanli = 2       // 明万历·全貌圆满：集资开凿南湖，解锁南湖书院、承志堂等，打通全面外排奇墅湖拓扑
}

/// <summary>
/// 带有时代控制属性的节点定义包装（用于在 Inspector 中配置，或由代码动态生成）
/// </summary>
[System.Serializable]
public class EraNodeData
{
    public string nodeName;
    public NodeType type;
    public float maxCapacity;
    public float elevation;
    public float initialVolume = 0f;
    public float initialQuality = 100f;
    public float localPollutionRate = 0f;
    
    [Tooltip("该节点解锁的时代。在低于此时代时，该节点处于迷雾或隐藏锁定状态，不参与水循环。")]
    public HongcunEra unlockedEra;
}

/// <summary>
/// 带有时代控制属性的边定义包装
/// </summary>
[System.Serializable]
public class EraEdgeData
{
    public string fromNodeName;
    public string toNodeName;
    public float length;
    public float width;
    public float maxFlowRate;
    public float initialGateRatio = 1f;

    [Tooltip("该渠段（水流边）解锁的时代。在低于此时代时，该渠段处于迷雾中，水流无法流经。")]
    public HongcunEra unlockedEra;
}

/// <summary>
/// 宏村水脉历史时代地图初始化与拓扑管理器。
/// 负责管理 序章(Era 0) -> 永乐(Era 1) -> 万历(Era 2) 的动态水利网络变迁，
/// 动态过滤节点与水道边，并同步到 WaterNetworkManager 的实时演算池中。
/// </summary>
[RequireComponent(typeof(WaterNetworkManager))]
public class InitializeHongcunMap : MonoBehaviour
{
    [Header("时代状态 (Era State)")]
    [Tooltip("当前的宏村水利演化时代。切换此值可动态重构整个宏村的水系拓扑。")]
    public HongcunEra currentEra = HongcunEra.Era1_Yongle;

    [Header("预置节点库 (Node Database)")]
    [Tooltip("宏村完整的历史完全体节点定义")]
    public List<EraNodeData> rawNodesDatabase = new List<EraNodeData>();

    [Header("预置水道库 (Edge Database)")]
    [Tooltip("宏村完整的历史完全体水道（明渠/暗渠）连接定义")]
    public List<EraEdgeData> rawEdgesDatabase = new List<EraEdgeData>();

    [Header("表现层联动配置")]
    [Tooltip("是否在时代解锁时播放全屏徽风迷雾消散粒子特效或摄像机切镜")]
    public bool playEraTransitionEffect = true;

    // 运行期缓存
    private WaterNetworkManager networkManager;
    private Dictionary<string, WaterNode> createdNodesDict = new Dictionary<string, WaterNode>();

    // 为方便在代码中直接使用，我们显式声明一个扩展机制，为 WaterNode/WaterEdge 提供 unlockedEra 的字典查询
    // 这样不需要强行破坏或覆写您已写好的 WaterNetworkManager.cs 底层演算代码
    private Dictionary<WaterNode, HongcunEra> nodeEraRegistry = new Dictionary<WaterNode, HongcunEra>();
    private Dictionary<WaterEdge, HongcunEra> edgeEraRegistry = new Dictionary<WaterEdge, HongcunEra>();

    private void Awake()
    {
        networkManager = GetComponent<WaterNetworkManager>();
    }

    private void Start()
    {
        // 1. 如果在编辑器中没有配置 rawNodesDatabase / rawEdgesDatabase，则自动使用经典史实拓扑进行代码硬编码初始化
        if (rawNodesDatabase.Count == 0 || rawEdgesDatabase.Count == 0)
        {
            SetupHistoricalDatabase();
        }

        // 2. 根据当前设定的时代，执行水利图论拓扑的动态加载与渲染过滤
        ApplyEraTopology(currentEra);
    }

    /// <summary>
    /// 获取某个节点或边的解锁时代。方便外部表现层（如迷雾、遮罩、WaterLineRenderer）查询状态。
    /// </summary>
    public HongcunEra GetNodeUnlockedEra(WaterNode node)
    {
        if (nodeEraRegistry.TryGetValue(node, out HongcunEra era)) return era;
        return HongcunEra.Era0_PreMing;
    }

    public HongcunEra GetEdgeUnlockedEra(WaterEdge edge)
    {
        if (edgeEraRegistry.TryGetValue(edge, out HongcunEra era)) return era;
        return HongcunEra.Era0_PreMing;
    }

    /// <summary>
    /// 核心方法：动态重构指定时代的水网拓扑。
    /// 过滤掉尚未解锁的节点与渠段，防止水流在未开发的土地上计算。
    /// </summary>
    public void ApplyEraTopology(HongcunEra targetEra)
    {
        currentEra = targetEra;
        createdNodesDict.Clear();
        nodeEraRegistry.Clear();
        edgeEraRegistry.Clear();

        // 准备传给 WaterNetworkManager 的实时激活列表
        List<WaterNode> activeNodes = new List<WaterNode>();
        List<WaterEdge> activeEdges = new List<WaterEdge>();

        // 1. 过滤并创建节点
        foreach (var nodeData in rawNodesDatabase)
        {
            // 只有当节点解锁时代 <= 当前时代，节点才被判定为“已解锁（可见）”
            if (nodeData.unlockedEra <= targetEra)
            {
                WaterNode node = new WaterNode
                {
                    nodeName = nodeData.nodeName,
                    type = nodeData.type,
                    maxCapacity = nodeData.maxCapacity,
                    elevation = nodeData.elevation,
                    currentVolume = nodeData.initialVolume,
                    waterQuality = nodeData.initialQuality,
                    localPollutionRate = nodeData.localPollutionRate
                };

                activeNodes.addRange(new WaterNode[] { node }); // 临时兼容
                activeNodes.Add(node);
                createdNodesDict[node.nodeName] = node;
                nodeEraRegistry[node] = nodeData.unlockedEra;
            }
            else
            {
                // 未解锁的节点在地图上作为“隐藏良田”
                Debug.Log($"[时代控制] 节点【{nodeData.nodeName}】在当前时代 {targetEra} 尚未解锁，处于隐藏状态。");
            }
        }

        // 2. 过滤并创建连接边（水渠）
        foreach (var edgeData in rawEdgesDatabase)
        {
            // 只有当水道本身解锁时代 <= 当前时代，且其上游、下游节点均已在当前时代创建，渠段才会被解锁
            if (edgeData.unlockedEra <= targetEra)
            {
                if (createdNodesDict.ContainsKey(edgeData.fromNodeName) && createdNodesDict.ContainsKey(edgeData.toNodeName))
                {
                    WaterNode fromNode = createdNodesDict[edgeData.fromNodeName];
                    WaterNode toNode = createdNodesDict[edgeData.toNodeName];

                    WaterEdge edge = new WaterEdge
                    {
                        fromNode = fromNode,
                        toNode = toNode,
                        length = edgeData.length,
                        width = edgeData.width,
                        maxFlowRate = edgeData.maxFlowRate,
                        gateOpenRatio = edgeData.initialGateRatio
                    };

                    activeEdges.Add(edge);
                    edgeEraRegistry[edge] = edgeData.unlockedEra;
                }
                else
                {
                    Debug.LogWarning($"[时代控制] 渠段【{edgeData.fromNodeName} -> {edgeData.toNodeName}】的端点未全部解锁，水道被迫处于截流阻断状态。");
                }
            }
            else
            {
                Debug.Log($"[时代控制] 水道【{edgeData.fromNodeName} -> {edgeData.toNodeName}】在当前时代 {targetEra} 处于封锁中。");
            }
        }

        // 3. 将过滤后的活跃水利拓扑图热重载到底层 WaterNetworkManager 中进行即时流体计算
        networkManager.nodes = activeNodes;
        networkManager.edges = activeEdges;

        // 4. 触发场景内所有 LineRenderer 的热重载与迷雾遮罩重绘
        NotifySceneRenderers();

        Debug.Log($"<color=emerald>【宏村水脉时代切换】成功转型至时代：{targetEra}！已激活 {activeNodes.Count} 个主节点，{activeEdges.Count} 条干流水圳渠段。</color>");
    }

    /// <summary>
    /// 触发整个场景中挂载在 Edge 上的表现层组件重新查找数据源
    /// </summary>
    private void NotifySceneRenderers()
    {
        WaterLineRenderer[] renderers = FindObjectsByType<WaterLineRenderer>(FindObjectsSortMode.None);
        foreach (var r in renderers)
        {
            // 清理已失效的 edge 数据源
            r.boundEdge = null;
            
            // 触发重新匹配寻找
            r.Invoke("Start", 0.05f);
        }
    }

    /// <summary>
    /// 核心数据库：严格按照宏村历史规划与发展演化进行“完全体”硬编码注册
    /// </summary>
    private void SetupHistoricalDatabase()
    {
        rawNodesDatabase.Clear();
        rawEdgesDatabase.Clear();

        // ====================================================================
        // ERA 0 (序章/自然村落：西溪河与古戏台)
        // ====================================================================
        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "XiXiRiver", // 西溪河（大同村方向天然水源）
            type = NodeType.RiverSource,
            maxCapacity = 99999f,
            elevation = 120f,
            initialVolume = 50000f,
            initialQuality = 100f,
            unlockedEra = HongcunEra.Era0_PreMing
        });

        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "AncientStage", // 古戏台老民居（在西溪旁）
            type = NodeType.Residential,
            maxCapacity = 100f,
            elevation = 110f,
            initialVolume = 10f,
            initialQuality = 100f,
            localPollutionRate = 1.5f,
            unlockedEra = HongcunEra.Era0_PreMing
        });

        // ====================================================================
        // ERA 1 (明永乐·蓝图奠基：石碣水口、月沼、汪氏宗祠、乐叙堂及早期水圳)
        // ====================================================================
        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "ShijieSluice", // 石碣水口（明渠引水总闸）
            type = NodeType.Gate,
            maxCapacity = 1000f,
            elevation = 115f,
            initialVolume = 0f,
            initialQuality = 100f,
            unlockedEra = HongcunEra.Era1_Yongle
        });

        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "WangClanHall", // 汪氏宗祠（核心上游）
            type = NodeType.Residential,
            maxCapacity = 200f,
            elevation = 112f,
            initialVolume = 0f,
            initialQuality = 100f,
            localPollutionRate = 1.0f,
            unlockedEra = HongcunEra.Era1_Yongle
        });

        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "MoonPond", // 月沼（牛胃：村中心巨型中继蓄水半月池）
            type = NodeType.MoonPond,
            maxCapacity = 3000f,
            elevation = 106f,
            initialVolume = 800f,
            initialQuality = 95f, // 具备自净，初始良好
            unlockedEra = HongcunEra.Era1_Yongle
        });

        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "LexuHall", // 乐叙堂（月沼旁的主体徽派公堂）
            type = NodeType.Residential,
            maxCapacity = 150f,
            elevation = 105f,
            initialVolume = 0f,
            initialQuality = 95f,
            localPollutionRate = 2.0f,
            unlockedEra = HongcunEra.Era1_Yongle
        });

        // ====================================================================
        // ERA 2 (明万历·全貌圆满：南湖、南湖书院、承志堂、树人堂、奇墅湖终极外排)
        // ====================================================================
        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "SouthLake", // 南湖（牛肚：村南宏伟蓄洪湖泊）
            type = NodeType.SouthLake,
            maxCapacity = 25000f,
            elevation = 98f,
            initialVolume = 5000f,
            initialQuality = 90f,
            unlockedEra = HongcunEra.Era2_Wanli
        });

        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "NanhuAcademy", // 南湖书院（万历开凿南湖后，在湖畔建立的高雅学堂）
            type = NodeType.Residential,
            maxCapacity = 150f,
            elevation = 99f,
            initialVolume = 0f,
            initialQuality = 90f,
            localPollutionRate = 0.5f, // 书院排污率极低，保持文化雅致
            unlockedEra = HongcunEra.Era2_Wanli
        });

        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "ChengzhiHall", // 承志堂（民间精雕大宅，万历拓扑延伸民居）
            type = NodeType.Residential,
            maxCapacity = 250f,
            elevation = 108f,
            initialVolume = 0f,
            initialQuality = 100f,
            localPollutionRate = 3.0f,
            unlockedEra = HongcunEra.Era2_Wanli
        });

        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "ShurenHall", // 树人堂（另一座精美徽派大宅）
            type = NodeType.Residential,
            maxCapacity = 180f,
            elevation = 107f,
            initialVolume = 0f,
            initialQuality = 100f,
            localPollutionRate = 2.5f,
            unlockedEra = HongcunEra.Era2_Wanli
        });

        rawNodesDatabase.Add(new EraNodeData {
            nodeName = "QishuLakeOutflow", // 奇墅湖（最终宣泄大湖，外排终点）
            type = NodeType.SouthLake,
            maxCapacity = 999999f,
            elevation = 85f,
            initialVolume = 10000f,
            initialQuality = 85f,
            unlockedEra = HongcunEra.Era2_Wanli
        });


        // ====================================================================
        // 水道边拓扑定义 (Edges Databases with Historical Eras)
        // ====================================================================
        
        // Era 0 仅连接西溪到戏台（原生态浅渠）
        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "XiXiRiver",
            toNodeName = "AncientStage",
            length = 50f,
            width = 0.15f,
            maxFlowRate = 10f,
            unlockedEra = HongcunEra.Era0_PreMing
        });

        // Era 1 奠基大凿：石碣引水口 -> 汪氏宗祠 -> 月沼 -> 乐叙堂
        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "XiXiRiver",
            toNodeName = "ShijieSluice",
            length = 80f,
            width = 0.45f,
            maxFlowRate = 120f,
            unlockedEra = HongcunEra.Era1_Yongle
        });

        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "ShijieSluice",
            toNodeName = "WangClanHall",
            length = 120f,
            width = 0.35f,
            maxFlowRate = 80f,
            unlockedEra = HongcunEra.Era1_Yongle
        });

        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "WangClanHall",
            toNodeName = "MoonPond",
            length = 200f,
            width = 0.3f,
            maxFlowRate = 60f,
            unlockedEra = HongcunEra.Era1_Yongle
        });

        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "MoonPond",
            toNodeName = "LexuHall",
            length = 90f,
            width = 0.25f,
            maxFlowRate = 45f,
            unlockedEra = HongcunEra.Era1_Yongle
        });

        // Era 2 万历盛世拓扑：承志堂、树人堂接入月沼，乐叙堂出水流经南湖，外排奇墅湖
        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "ShijieSluice",
            toNodeName = "ChengzhiHall",
            length = 70f,
            width = 0.3f,
            maxFlowRate = 50f,
            unlockedEra = HongcunEra.Era2_Wanli
        });

        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "ChengzhiHall",
            toNodeName = "ShurenHall",
            length = 60f,
            width = 0.28f,
            maxFlowRate = 45f,
            unlockedEra = HongcunEra.Era2_Wanli
        });

        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "ShurenHall",
            toNodeName = "MoonPond",
            length = 110f,
            width = 0.25f,
            maxFlowRate = 40f,
            unlockedEra = HongcunEra.Era2_Wanli
        });

        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "LexuHall",
            toNodeName = "SouthLake",
            length = 280f,
            width = 0.4f,
            maxFlowRate = 90f,
            unlockedEra = HongcunEra.Era2_Wanli
        });

        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "SouthLake",
            toNodeName = "NanhuAcademy",
            length = 80f,
            width = 0.35f,
            maxFlowRate = 50f,
            unlockedEra = HongcunEra.Era2_Wanli
        });

        rawEdgesDatabase.Add(new EraEdgeData {
            fromNodeName = "NanhuAcademy",
            toNodeName = "QishuLakeOutflow",
            length = 350f,
            width = 0.5f,
            maxFlowRate = 180f,
            unlockedEra = HongcunEra.Era2_Wanli
        });
    }
}
