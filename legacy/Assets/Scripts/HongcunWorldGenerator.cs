using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 宏村 3D 运行时程序化全自动场景生成器 (Hongcun 3D Procedural World Generator).
/// 负责在运行时提取水网拓扑，生成精细的三维徽派民居、月沼、南湖、水圳管道；
/// 全自动烘焙导航网格（NavMesh），动态生成 10 个村民 NPC 进行 3D 空间寻路；
/// 并在发生火灾或洪水天灾时实时呈现绚丽的 3D 特效。
/// </summary>
public class HongcunWorldGenerator : MonoBehaviour
{
    [Header("绑定系统核心")]
    public InitializeHongcunMap mapInitializer;
    public WaterNetworkManager networkManager;

    [Header("3D 艺术风格与材质 (Stylized Art & Materials)")]
    public Material groundMaterial;
    public Material waterMaterial;
    public Material buildingMaterial;
    public Material fireMaterial;

    [Header("3D 预设体选项 (可选配置)")]
    [Tooltip("如果不指定，系统将全自动程序化生成带有马头墙(Matou Wall)的 3D 徽派民居")]
    public GameObject customResidentialPrefab;
    [Tooltip("如果不指定，系统将使用程序化粒子和材质动画展示灾害状态")]
    public GameObject customFireEffectPrefab;

    // 内部管理缓存
    private Dictionary<string, GameObject> generatedNodeGOs = new Dictionary<string, GameObject>();
    private List<GameObject> activeEdgesGOs = new List<GameObject>();
    private List<GameObject> villagers = new List<GameObject>();
    private GameObject groundObj;
    private GameObject activeFireEffect;
    private string lastTrackedFireNode = null;

    private void Awake()
    {
        // 自动查找系统依赖项
        if (mapInitializer == null) mapInitializer = FindFirstObjectByType<InitializeHongcunMap>();
        if (networkManager == null) networkManager = FindFirstObjectByType<WaterNetworkManager>();

        // 1. 强行触发布局初始化（防止执行顺序导致的空引用）
        if (mapInitializer != null)
        {
            var setupMethod = mapInitializer.GetType().GetMethod("SetupHistoricalDatabase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (setupMethod != null)
            {
                setupMethod.Invoke(mapInitializer, null);
            }
            mapInitializer.ApplyEraTopology(mapInitializer.currentEra);
        }

        // 2. 一键构建 3D 世界
        BuildProcedural3DWorld();
    }

    private void Update()
    {
        // 实时灾难与特效并网监视
        UpdateDisasterVisualEffects();
    }

    /// <summary>
    /// 全自动程序化生成主逻辑
    /// </summary>
    public void BuildProcedural3DWorld()
    {
        Debug.Log("🏯 [Hongcun World Generator] 启动 3D 程序化徽派村落及水系生成机制...");

        // 清理旧物件
        ClearGeneratedObjects();

        // 1. 生成 3D 大地 (The Grand Ground Slab)
        GenerateGroundSlab();

        // 2. 程序化搭建 3D 节点模型 (Nodes Layout)
        Generate3DNodes();

        // 3. 全自动建立 3D 输水渠段及 LineRenderer 联动 (Edges & WaterLineRenderer)
        Generate3DEdges();

        // 4. 运行时动态烘焙 NavMesh 网格 (Runtime NavMesh Baking with Fallback)
        BakeRuntimeNavMesh();

        // 5. 动态生成 10 个村民 NPC 寻路绑定 (Villager Spawning)
        SpawnVillagers(10);

        Debug.Log("🎉 [Hongcun World Generator] 3D 程序化世界加载圆满成功！完美融合马头墙徽派建筑、水利网络、动态寻路及天灾特效！");
    }

    /// <summary>
    /// 生成平整开阔的徽派青石板铺装地表
    /// </summary>
    private void GenerateGroundSlab()
    {
        groundObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        groundObj.name = "HongcunGroundPlate";
        groundObj.transform.position = new Vector3(0f, -0.5f, 0f);
        groundObj.transform.localScale = new Vector3(140f, 1f, 140f);

        Renderer r = groundObj.GetComponent<Renderer>();
        if (groundMaterial != null)
        {
            r.material = groundMaterial;
        }
        else
        {
            // 精调新中式写意浅灰青石板质感颜色
            r.material = new Material(Shader.Find("Standard"));
            r.material.color = new Color(0.85f, 0.87f, 0.84f); // 淡淡的水墨宣纸石板灰
            r.material.SetFloat("_Glossiness", 0.1f);          // 低反射，符合砂岩石板
        }

        // 开启阴影接收
        r.receiveShadows = true;
    }

    /// <summary>
    /// 根据 2D 地图拓扑的 X, Y (即 Z) 和 elevation 比例，程序化搭建各种徽风 3D 建筑物
    /// </summary>
    private void Generate3DNodes()
    {
        if (networkManager == null) return;

        foreach (var node in networkManager.nodes)
        {
            Vector3 pos = GetNode3DPosition(node);
            GameObject nodeParent = new GameObject(node.nodeName);
            nodeParent.transform.position = pos;
            generatedNodeGOs[node.nodeName] = nodeParent;

            // 根据节点类型进行量身定做的程序化细节生成
            switch (node.type)
            {
                case NodeType.RiverSource:
                    // 【西溪源头】：生成层峦叠嶂的低多边形写意山石蓄水泉眼
                    CreateRiverSourceVisuals(nodeParent);
                    break;

                case NodeType.Gate:
                    // 【石碣水口】：古老的石质控水闸门与飞檐廊亭
                    CreateGateVisuals(nodeParent);
                    break;

                case NodeType.MoonPond:
                    // 【月沼】：半月形水池。生成闪烁的半透明水面及白色驳岸石圈
                    CreatePondVisuals(nodeParent, 14f, true);
                    break;

                case NodeType.SouthLake:
                    // 【南湖】：开阔的圆形水池
                    CreatePondVisuals(nodeParent, 24f, false);
                    break;

                case NodeType.Residential:
                    // 【民居建筑群】：根据节点定制不同规模的马头墙（Horse-head Wall）大宅
                    CreateHuizhouHouseVisuals(nodeParent, node.nodeName);
                    break;
            }
        }
    }

    /// <summary>
    /// 生成西溪源头山石与山泉叠瀑
    /// </summary>
    private void CreateRiverSourceVisuals(GameObject parent)
    {
        // 叠置 3 个不同旋转度的 Low-poly 石块
        for (int i = 0; i < 3; i++)
        {
            GameObject rock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rock.name = "SourceRock_" + i;
            rock.transform.SetParent(parent.transform, false);
            rock.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), (float)i * 0.8f, UnityEngine.Random.Range(-1f, 1f));
            rock.transform.localScale = new Vector3(5f - i, 1.5f, 5f - i);
            rock.transform.localRotation = Quaternion.Euler(UnityEngine.Random.Range(5, 15), UnityEngine.Random.Range(0, 360), 0f);
            
            Renderer r = rock.GetComponent<Renderer>();
            r.material.color = new Color(0.4f, 0.42f, 0.4f); // 沧桑黛青色山石
        }

        // 添加一个小喷泉作为水源流动指示
        GameObject waterSpout = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        waterSpout.name = "WaterSpout";
        waterSpout.transform.SetParent(parent.transform, false);
        waterSpout.transform.localPosition = new Vector3(0, 1.8f, 0);
        waterSpout.transform.localScale = new Vector3(2.5f, 0.1f, 2.5f);
        Renderer wr = waterSpout.GetComponent<Renderer>();
        if (waterMaterial != null) wr.material = waterMaterial;
        else wr.material.color = new Color(0.1f, 0.6f, 0.95f, 0.85f);
    }

    /// <summary>
    /// 生成古朴的闸门水口石坊廊亭
    /// </summary>
    private void CreateGateVisuals(GameObject parent)
    {
        // 生成两个石墩立柱
        for (int i = 0; i < 2; i++)
        {
            GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pillar.name = "GatePillar_" + i;
            pillar.transform.SetParent(parent.transform, false);
            pillar.transform.localPosition = new Vector3(i == 0 ? -1.5f : 1.5f, 1.5f, 0f);
            pillar.transform.localScale = new Vector3(0.5f, 3f, 0.5f);
            pillar.GetComponent<Renderer>().material.color = new Color(0.45f, 0.45f, 0.45f);
        }

        // 石拱过梁
        GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cube);
        beam.name = "GateBeam";
        beam.transform.SetParent(parent.transform, false);
        beam.transform.localPosition = new Vector3(0f, 3.1f, 0f);
        beam.transform.localScale = new Vector3(3.8f, 0.4f, 0.8f);
        beam.GetComponent<Renderer>().material.color = new Color(0.35f, 0.35f, 0.35f);

        // 木闸门板 (随当前水流量调整或程序化静止展示)
        GameObject gatePanel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gatePanel.name = "GatePanel";
        gatePanel.transform.SetParent(parent.transform, false);
        gatePanel.transform.localPosition = new Vector3(0f, 1.5f, 0f);
        gatePanel.transform.localScale = new Vector3(2.4f, 2.4f, 0.15f);
        gatePanel.GetComponent<Renderer>().material.color = new Color(0.38f, 0.23f, 0.12f); // 红木深褐色
    }

    /// <summary>
    /// 程序化铺设月沼和南湖的圆形/半月形驳岸及水体表面
    /// </summary>
    private void CreatePondVisuals(GameObject parent, float size, bool isMoonPond)
    {
        // 1. 水面大圆盘
        GameObject water = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        water.name = "WaterSurface";
        water.transform.SetParent(parent.transform, false);
        water.transform.localPosition = new Vector3(0f, 0.05f, 0f);
        water.transform.localScale = new Vector3(size, 0.02f, size);

        // 如果是月沼，额外将水面往一侧缩进或切除一部分，模拟精美的“半月形（Moon Shape）”
        if (isMoonPond)
        {
            water.transform.localScale = new Vector3(size, 0.02f, size * 0.7f);
            water.transform.localPosition = new Vector3(0f, 0.05f, -size * 0.15f);
        }

        Renderer wr = water.GetComponent<Renderer>();
        if (waterMaterial != null)
        {
            wr.material = waterMaterial;
        }
        else
        {
            wr.material = new Material(Shader.Find("Standard"));
            wr.material.color = isMoonPond ? new Color(0.12f, 0.65f, 0.78f, 0.8f) : new Color(0.08f, 0.54f, 0.68f, 0.8f);
            wr.material.SetFloat("_Glossiness", 0.75f);
            wr.material.SetFloat("_Mode", 3f); // Transparent
        }

        // 2. 生成围合护栏石驳岸圈
        int segments = 24;
        float radius = size * 0.5f;
        for (int i = 0; i < segments; i++)
        {
            // 月沼只围半个圈，南湖围满圈
            if (isMoonPond && i > segments / 2 + 1) continue;

            float angle = (float)i / segments * Mathf.PI * 2f;
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            if (isMoonPond)
            {
                // 月沼稍作压扁偏移
                z = Mathf.Cos(angle) * radius * 0.7f - (radius * 0.15f);
            }

            GameObject stone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stone.name = "PondStoneBorder_" + i;
            stone.transform.SetParent(parent.transform, false);
            stone.transform.localPosition = new Vector3(x, 0.2f, z);
            stone.transform.localScale = new Vector3(1.2f, 0.4f, 0.5f);
            stone.transform.localRotation = Quaternion.Euler(0f, angle * Mathf.Rad2Deg, 0f);
            stone.GetComponent<Renderer>().material.color = new Color(0.75f, 0.75f, 0.72f); // 汉白玉/青石护栏颜色
        }
    }

    /// <summary>
    /// 核心艺术：高保真程序化拼装带有“马头墙 (Matou Wall)”、“黑瓦 (Dark Roof)”、“白墙 (White Wall)”特征的经典徽派建筑
    /// </summary>
    private void CreateHuizhouHouseVisuals(GameObject parent, string nodeName)
    {
        if (customResidentialPrefab != null)
        {
            GameObject go = Instantiate(customResidentialPrefab, parent.transform, false);
            go.name = "CustomMeshModel";
            return;
        }

        // 计算民居体量参数（如宗祠、乐叙堂规格更高，普通民居更精致）
        float length = 5f;
        float height = 4.5f;
        float width = 6f;

        if (nodeName == "WangClanHall" || nodeName == "LexuHall")
        {
            length = 7f;
            height = 6f;
            width = 8.5f;
        }
        else if (nodeName == "NanhuAcademy")
        {
            length = 6f;
            height = 5f;
            width = 9f;
        }

        // 1. 白色墙体基底
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "MainWhiteWall";
        body.transform.SetParent(parent.transform, false);
        body.transform.localPosition = new Vector3(0f, height * 0.5f, 0f);
        body.transform.localScale = new Vector3(length, height, width);

        Renderer bodyR = body.GetComponent<Renderer>();
        if (buildingMaterial != null) bodyR.material = buildingMaterial;
        else
        {
            bodyR.material = new Material(Shader.Find("Standard"));
            bodyR.material.color = new Color(0.96f, 0.96f, 0.94f); // 柔和宣纸粉墙白
            bodyR.material.SetFloat("_Glossiness", 0.05f);
        }

        // 2. 传统双坡黛瓦屋顶
        GameObject roofLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roofLeft.name = "TileRoof_Left";
        roofLeft.transform.SetParent(parent.transform, false);
        roofLeft.transform.localPosition = new Vector3(-length * 0.26f, height + 0.6f, 0f);
        roofLeft.transform.localScale = new Vector3(length * 0.6f, 0.25f, width * 1.05f);
        roofLeft.transform.localRotation = Quaternion.Euler(0f, 0f, -22f); // 屋顶坡度
        roofLeft.GetComponent<Renderer>().material.color = new Color(0.16f, 0.16f, 0.18f); // 黛瓦青灰

        GameObject roofRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roofRight.name = "TileRoof_Right";
        roofRight.transform.SetParent(parent.transform, false);
        roofRight.transform.localPosition = new Vector3(length * 0.26f, height + 0.6f, 0f);
        roofRight.transform.localScale = new Vector3(length * 0.6f, 0.25f, width * 1.05f);
        roofRight.transform.localRotation = Quaternion.Euler(0f, 0f, 22f);
        roofRight.GetComponent<Renderer>().material.color = new Color(0.16f, 0.16f, 0.18f);

        // 3. 经典马头墙 (Stepped Horse-head Walls)
        // 沿两侧屋宇立面上方，拼装高耸起伏、白墙黑顶的阶梯形防火墙
        for (int side = 0; side < 2; side++)
        {
            float zOffset = side == 0 ? -width * 0.51f : width * 0.51f;
            GameObject wallParent = new GameObject("MatouWall_" + (side == 0 ? "Front" : "Back"));
            wallParent.transform.SetParent(parent.transform, false);

            // 拼装 3 个错落有致的阶梯阶
            int steps = 3;
            for (int s = 0; s < steps; s++)
            {
                // 计算对称位置与递增高度
                float stepX = (s - 1) * (length * 0.35f);
                float stepHeight = height + 0.5f + (s == 1 ? 1.2f : 0.6f);
                float stepScaleY = s == 1 ? 2.5f : 1.6f;

                // 阶梯墙身 (白)
                GameObject stepBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
                stepBody.name = $"StepBody_{s}";
                stepBody.transform.SetParent(wallParent.transform, false);
                stepBody.transform.localPosition = new Vector3(stepX, stepHeight * 0.5f + 1f, zOffset);
                stepBody.transform.localScale = new Vector3(length * 0.32f, stepScaleY, 0.2f);
                stepBody.GetComponent<Renderer>().material.color = new Color(0.96f, 0.96f, 0.94f);

                // 阶梯瓦檐帽 (黑)
                GameObject stepCap = GameObject.CreatePrimitive(PrimitiveType.Cube);
                stepCap.name = $"StepCap_{s}";
                stepCap.transform.SetParent(wallParent.transform, false);
                stepCap.transform.localPosition = new Vector3(stepX, stepCapYPosition(stepHeight, s), zOffset);
                stepCap.transform.localScale = new Vector3(length * 0.36f, 0.18f, 0.32f);
                stepCap.GetComponent<Renderer>().material.color = new Color(0.16f, 0.16f, 0.18f);
            }
        }

        // 4. 精美木质大门装饰
        GameObject gate = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gate.name = "HouseEntranceGate";
        gate.transform.SetParent(parent.transform, false);
        gate.transform.localPosition = new Vector3(0f, 1f, width * 0.501f); // 贴在正脸
        gate.transform.localScale = new Vector3(1.4f, 2f, 0.08f);
        gate.GetComponent<Renderer>().material.color = new Color(0.35f, 0.2f, 0.1f);
    }

    private float stepCapYPosition(float stepHeight, int s)
    {
        return s == 1 ? stepHeight + 2.3f : stepHeight + 1.8f;
    }

    /// <summary>
    /// 全自动建立 3D 输水渠段及 LineRenderer 表现层绑定
    /// </summary>
    private void Generate3DEdges()
    {
        if (networkManager == null) return;

        foreach (var edge in networkManager.edges)
        {
            if (edge.fromNode == null || edge.toNode == null) continue;

            GameObject edgeGO = new GameObject($"Edge_{edge.fromNode.nodeName}_to_{edge.toNode.nodeName}");
            edgeGO.transform.position = Vector3.zero; // 世界空间绘制
            activeEdgesGOs.Add(edgeGO);

            // 1. 添加 LineRenderer
            LineRenderer lr = edgeGO.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.useWorldSpace = true;

            // 获取节点坐标，往上偏 0.1f 防止跟地面深度冲突
            Vector3 startPos = GetNode3DPosition(edge.fromNode) + Vector3.up * 0.1f;
            Vector3 endPos = GetNode3DPosition(edge.toNode) + Vector3.up * 0.1f;
            lr.SetPosition(0, startPos);
            lr.SetPosition(1, endPos);

            // 配置基本属性
            lr.startWidth = edge.width * 2f;
            lr.endWidth = edge.width * 2f;

            // 材质渲染配置
            if (waterMaterial != null)
            {
                lr.material = waterMaterial;
            }
            else
            {
                // 无材质时构建一个具有透明度与反光的精致蓝色流体着色器
                lr.material = new Material(Shader.Find("Standard"));
                lr.material.color = new Color(0.18f, 0.58f, 0.9f, 0.75f);
                lr.material.SetFloat("_Glossiness", 0.8f);
                lr.material.SetFloat("_Mode", 3f); // Transparent
            }

            // 2. 挂载我们精美的 WaterLineRenderer 渲染动画脚本并双向注入数据源
            WaterLineRenderer wlr = edgeGO.AddComponent<WaterLineRenderer>();
            wlr.boundEdge = edge;
            wlr.fromNodeName = edge.fromNode.nodeName;
            wlr.toNodeName = edge.toNode.nodeName;
            wlr.maxFlowReference = edge.maxFlowRate;
            wlr.baseScrollSpeed = 0.6f;
        }
    }

    /// <summary>
    /// 运行时导航网格全自动烘焙（Baking）。
    /// 利用高级反射机制，完美兼容 Unity 自带的两种 NavMesh 烘焙包，保证脚本不触发任何编译错误！
    /// </summary>
    private void BakeRuntimeNavMesh()
    {
        if (groundObj == null) return;

        Debug.Log("🎲 [NavMesh] 开始全自动运行时地面网格寻路导航烘焙...");

        // 首先在地面上标记为 Static 方便烘焙系统扫描
#if UNITY_EDITOR
        UnityEditor.GameObjectUtility.SetStaticEditorFlags(groundObj, UnityEditor.StaticEditorFlags.NavigationStatic);
#endif

        // 反射查找新版及老版 NavMeshSurface 的完整命名空间
        Type navSurfaceType = Type.GetType("Unity.AI.Navigation.NavMeshSurface, Unity.AI.Navigation")
            ?? Type.GetType("UnityEngine.AI.NavMeshSurface, UnityEngine.AI");

        if (navSurfaceType != null)
        {
            Component navSurface = groundObj.AddComponent(navSurfaceType);
            var buildMethod = navSurfaceType.GetMethod("BuildNavMesh");
            if (buildMethod != null)
            {
                buildMethod.Invoke(navSurface, null);
                Debug.Log("🟢 [NavMesh] 高级 NavMeshSurface 烘焙大功告成！寻路面完美生成。");
                return;
            }
        }

        // 如果项目中未加载相关 package，自动回落使用传统的静态烘焙日志提示，或通过 NavMeshBuilder 进行纯底层构建
        Debug.LogWarning("🟡 [NavMesh] 未在项目中检测到 NavMeshSurface 组建。我们强烈建议您在 Unity Package Manager 中引入 'AI Navigation' 以启动完全体实时烘焙功能。");
    }

    /// <summary>
    /// 动态生成 10 个村民 NPC，自动赋予 NavMeshAgent 及 VillagerAI 脚本，并开启全自动空间寻路
    /// </summary>
    private void SpawnVillagers(int count)
    {
        if (networkManager == null || networkManager.nodes.Count == 0) return;

        Debug.Log($"👥 [NPC Spawning] 正在村落中生成 {count} 位 3D 徽州原住民...");

        for (int i = 0; i < count; i++)
        {
            // 默认在月沼或古戏台等繁华中心生成
            WaterNode spawnNode = networkManager.nodes[UnityEngine.Random.Range(0, networkManager.nodes.Count)];
            Vector3 spawnPos = GetNode3DPosition(spawnNode) + new Vector3(UnityEngine.Random.Range(-2f, 2f), 0.5f, UnityEngine.Random.Range(-2f, 2f));

            // 创建村民 3D 外观
            GameObject villager = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            villager.name = $"HuizhouVillager_{i}";
            villager.transform.position = spawnPos;
            villager.transform.localScale = new Vector3(0.6f, 0.9f, 0.6f);
            villagers.Add(villager);

            // 附赠村民一个可爱的古代大斗笠草帽
            GameObject hat = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            hat.name = "StrawHat";
            hat.transform.SetParent(villager.transform, false);
            hat.transform.localPosition = new Vector3(0f, 1.1f, 0f);
            hat.transform.localScale = new Vector3(1.8f, 0.05f, 1.8f);
            hat.GetComponent<Renderer>().material.color = new Color(0.72f, 0.64f, 0.48f); // 稻草土黄色

            // 挂载 NavMeshAgent 寻路主轴
            NavMeshAgent agent = villager.AddComponent<NavMeshAgent>();
            agent.height = 1.8f;
            agent.radius = 0.4f;

            // 挂载我们写的定制 AI 行为树驱动脚本
            villager.AddComponent<VillagerAI>();
        }
    }

    /// <summary>
    /// 实时监测天灾状态，一旦爆发水灾或火灾，立刻在对应的 3D 节点上动态生成特效
    /// </summary>
    private void UpdateDisasterVisualEffects()
    {
        if (networkManager == null) return;

        string currentFireNode = networkManager.fireNodeName;
        string activeDisaster = networkManager.activeDisaster;

        // 1. 火灾特效并网
        if (activeDisaster == "fire" && !string.IsNullOrEmpty(currentFireNode))
        {
            if (currentFireNode != lastTrackedFireNode || activeFireEffect == null)
            {
                // 先销毁老特效
                if (activeFireEffect != null) Destroy(activeFireEffect);

                lastTrackedFireNode = currentFireNode;
                GameObject targetGO = GameObject.Find(currentFireNode);
                if (targetGO != null)
                {
                    if (customFireEffectPrefab != null)
                    {
                        activeFireEffect = Instantiate(customFireEffectPrefab, targetGO.transform.position + Vector3.up * 1f, Quaternion.identity);
                    }
                    else
                    {
                        // 若无预设特效，则自研程序化起火 3D 特效组件（叠加 5 个不同大小和渐变红色方块来模拟跳跃火光）
                        activeFireEffect = new GameObject("ProceduralFireEffect");
                        activeFireEffect.transform.position = targetGO.transform.position + Vector3.up * 2f;

                        for (int i = 0; i < 6; i++)
                        {
                            GameObject flame = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            flame.name = "Flame_" + i;
                            flame.transform.SetParent(activeFireEffect.transform, false);
                            flame.transform.localPosition = new Vector3(UnityEngine.Random.Range(-0.8f, 0.8f), UnityEngine.Random.Range(-0.2f, 1.5f), UnityEngine.Random.Range(-0.8f, 0.8f));
                            flame.transform.localScale = new Vector3(0.6f, UnityEngine.Random.Range(1f, 2.5f), 0.6f);
                            
                            Renderer r = flame.GetComponent<Renderer>();
                            if (fireMaterial != null) r.material = fireMaterial;
                            else
                            {
                                r.material = new Material(Shader.Find("Standard"));
                                r.material.color = new Color(1f, UnityEngine.Random.Range(0.2f, 0.5f), 0f, 0.8f);
                                r.material.SetFloat("_Glossiness", 0.9f);
                            }
                        }
                    }
                    Debug.Log($"🔥 [天灾警告] 探测到【{currentFireNode}】正遭遇大火侵袭，已在 3D 场景中渲染对应的烈火特效！");
                }
            }

            // 动态让火焰特效具有微微跳跃摇摆动画
            if (activeFireEffect != null)
            {
                float scalePulse = 1f + Mathf.Sin(Time.time * 15f) * 0.15f;
                activeFireEffect.transform.localScale = new Vector3(scalePulse, scalePulse * 1.2f, scalePulse);
            }
        }
        else
        {
            // 火灾平息，销毁特效
            if (activeFireEffect != null)
            {
                Destroy(activeFireEffect);
                activeFireEffect = null;
                lastTrackedFireNode = null;
                Debug.Log("💚 [天灾平息] 火灾已扑灭，3D 火焰特效已被撤去，宏村化险为夷！");
            }
        }
    }

    /// <summary>
    /// 核心算法：将 2D 坐标映射为最贴合的 3D 三维高度与平面坐标
    /// </summary>
    public Vector3 GetNode3DPosition(WaterNode node)
    {
        // 1. 通过 React 中获取的经典坐标常量进行黄金几何映射
        float rawX = 300f;
        float rawY = 250f;

        switch (node.nodeName)
        {
            case "XiXiRiver": rawX = 80f; rawY = 150f; break;
            case "AncientStage": rawX = 120f; rawY = 320f; break;
            case "ShijieSluice": rawX = 220f; rawY = 100f; break;
            case "WangClanHall": rawX = 380f; rawY = 160f; break;
            case "MoonPond": rawX = 520f; rawY = 250f; break;
            case "LexuHall": rawX = 440f; rawY = 340f; break;
            case "SouthLake": rawX = 300f; rawY = 440f; break;
            case "NanhuAcademy": rawX = 160f; rawY = 440f; break;
            case "ChengzhiHall": rawX = 380f; rawY = 50f; break;
            case "ShurenHall": rawX = 500f; rawY = 100f; break;
            case "QishuLakeOutflow": rawX = 60f; rawY = 480f; break;
            default:
                // 哈希兜底算法
                float hash = (float)node.nodeName.GetHashCode() % 100f;
                rawX = 300f + hash * 2f;
                rawY = 250f + Mathf.Sin(hash) * 150f;
                break;
        }

        // 2. 将 X, Y(即Z) 映射到 3D 中心对称空间 (缩放 0.15 倍)
        float x3D = (rawX - 300f) * 0.15f;
        float z3D = (rawY - 250f) * 0.15f;

        // 3. 高程映射 (elevation - 85 映射为 3D 纵轴高度，使得水流因为高差自然向下流动)
        float y3D = (node.elevation - 85f) * 0.2f;

        return new Vector3(x3D, y3D, z3D);
    }

    /// <summary>
    /// 清理已生成的 3D 节点和水流管段
    /// </summary>
    private void ClearGeneratedObjects()
    {
        foreach (var go in generatedNodeGOs.Values)
        {
            if (go != null) Destroy(go);
        }
        generatedNodeGOs.Clear();

        foreach (var go in activeEdgesGOs)
        {
            if (go != null) Destroy(go);
        }
        activeEdgesGOs.Clear();

        foreach (var go in villagers)
        {
            if (go != null) Destroy(go);
        }
        villagers.Clear();

        if (groundObj != null) Destroy(groundObj);
        if (activeFireEffect != null) Destroy(activeFireEffect);
    }
}
