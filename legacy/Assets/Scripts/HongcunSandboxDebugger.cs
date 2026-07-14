using System;
using UnityEngine;

/// <summary>
/// 宏村水利沙盒运行期 Debugger 调试器。
/// 使用 Unity 内置 OnGUI 系统，在屏幕左侧提供时代热重载、降雨量调节及晨洗暮饮时段切换功能。
/// 并在屏幕上节点的三维空间坐标上方绘制浮动的实时数据（水量、水质）。
/// </summary>
public class HongcunSandboxDebugger : MonoBehaviour
{
    [Header("绑定系统组件")]
    public WaterNetworkManager networkManager;
    public InitializeHongcunMap mapInitializer;

    [Header("调试期浮动文字样式")]
    public GUIStyle labelStyle;
    public Color textNormalColor = Color.white;
    public Color textWarningColor = Color.yellow;

    // 内部运行状态
    private float updateTimer = 0f;
    private const float simulationStep = 0.05f; // 20 帧每秒进行一次仿真更新

    private void Start()
    {
        // 自动寻源绑定
        if (networkManager == null) networkManager = FindFirstObjectByType<WaterNetworkManager>();
        if (mapInitializer == null) mapInitializer = FindFirstObjectByType<InitializeHongcunMap>();

        // 配置默认浮动文字样式
        if (labelStyle == null || labelStyle.fontSize == 0)
        {
            labelStyle = new GUIStyle();
            labelStyle.fontSize = 11;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.normal.textColor = textNormalColor;
        }
    }

    private void Update()
    {
        if (networkManager == null) return;

        // 驱动底层流体物理引擎进行时间迭代 (Tick)
        updateTimer += Time.deltaTime;
        if (updateTimer >= simulationStep)
        {
            networkManager.SimulationTick(simulationStep);
            updateTimer = 0f;
        }
    }

    private void OnGUI()
    {
        if (networkManager == null || mapInitializer == null)
        {
            GUILayout.BeginArea(new Rect(20, 20, 300, 100));
            GUILayout.Box("⚠️ [宏村水脉] 错误：未绑定 Manager/Initializer！");
            GUILayout.EndArea();
            return;
        }

        // ====================================================================
        // 1. 左上角核心控制台 GUI 面板
        // ====================================================================
        Rect consoleRect = new Rect(20, 20, 320, 380);
        GUI.Box(consoleRect, "🏯 宏村水脉：沙盒测试调试面板 (Debug Panel)");

        GUILayout.BeginArea(new Rect(30, 50, 300, 340));

        // 显示当前时代信息
        GUILayout.Label($"<b>当前历史图层 (Current Era):</b> {mapInitializer.currentEra}", GUILayout.Height(20));

        // 按钮组：时代热重载切换
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Era 0\n(南宋·前民居)", GUILayout.Height(40)))
        {
            mapInitializer.ApplyEraTopology(HongcunEra.Era0_PreMing);
        }
        if (GUILayout.Button("Era 1\n(明永乐·月沼)", GUILayout.Height(40)))
        {
            mapInitializer.ApplyEraTopology(HongcunEra.Era1_Yongle);
        }
        if (GUILayout.Button("Era 2\n(明万历·南湖)", GUILayout.Height(40)))
        {
            mapInitializer.ApplyEraTopology(HongcunEra.Era2_Wanli);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        // 环境参数滑块：降雨量
        GUILayout.Label($"🌧️ <b>全局降雨强度 (Rain Intensity):</b> {networkManager.rainIntensity:F1} L/s");
        networkManager.rainIntensity = GUILayout.HorizontalSlider(networkManager.rainIntensity, 0f, 100f);

        GUILayout.Space(15);

        // 环境参数单选：时段演变
        GUILayout.Label($"⏰ <b>当前生活时段 (Period):</b> {networkManager.currentPeriod}");
        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(networkManager.currentPeriod == "Morning", "☀️ 清晨饮水", "Button", GUILayout.Height(25)))
        {
            networkManager.currentPeriod = "Morning";
        }
        if (GUILayout.Toggle(networkManager.currentPeriod == "Evening", "🧺 傍晚洗涤", "Button", GUILayout.Height(25)))
        {
            networkManager.currentPeriod = "Evening";
        }
        if (GUILayout.Toggle(networkManager.currentPeriod == "Night", "🌙 深夜静流", "Button", GUILayout.Height(25)))
        {
            networkManager.currentPeriod = "Night";
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        // 当前仿真统计
        GUILayout.Box($"<b>当前仿真统计 (Live Stats):</b>\n" +
                    $"• 活跃水节点数: {networkManager.nodes.Count}\n" +
                    $"• 活跃水道边数: {networkManager.edges.Count}\n" +
                    $"• 降雨系数: {(networkManager.rainIntensity > 0 ? "汛期" : "枯水")}");

        GUILayout.EndArea();

        // ====================================================================
        // 2. 空间节点浮动文字绘制 (3D World Space -> 2D Screen Space)
        // ====================================================================
        if (Camera.main == null) return;

        foreach (var node in networkManager.nodes)
        {
            // 在 Unity 模拟中，在 3D 的 Node 位置上方绘制文字
            Vector3 worldPos = GetNodeWorldPosition(node);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            // 只有在摄像机前方的节点才绘制
            if (screenPos.z > 0)
            {
                // GUI 的 Y 轴零点在上方，而 ScreenPoint 的 Y 轴零点在下方，需要转换
                float guiY = Screen.height - screenPos.y;

                // 悬浮框底色和尺寸
                Rect labelRect = new Rect(screenPos.x - 75, guiY - 45, 150, 42);

                // 准备文本内容
                string nodeTypeText = GetNodeTypeLabel(node.type);
                string statsText = $"{node.nodeName} ({nodeTypeText})\n" +
                                   $"水量: {node.currentVolume:F1}/{node.maxCapacity:F0} L\n" +
                                   $"水质: {node.waterQuality:F1}%";

                // 如果水体溢出或水质低于 40，使用警示配色
                if (node.IsFlooding)
                {
                    GUI.backgroundColor = Color.red;
                    labelStyle.normal.textColor = textWarningColor;
                }
                else if (node.waterQuality < 40f)
                {
                    GUI.backgroundColor = new Color(0.6f, 0.4f, 0.3f);
                    labelStyle.normal.textColor = textWarningColor;
                }
                else
                {
                    GUI.backgroundColor = new Color(0.1f, 0.2f, 0.3f, 0.85f);
                    labelStyle.normal.textColor = textNormalColor;
                }

                GUI.Box(labelRect, statsText, labelStyle);
            }
        }
    }

    /// <summary>
    /// 辅助方法：将节点类型转换为简短标识
    /// </summary>
    private string GetNodeTypeLabel(NodeType type)
    {
        switch (type)
        {
            case NodeType.RiverSource: return "西溪源头";
            case NodeType.Gate: return "石碣水口";
            case NodeType.NormalDitch: return "明暗水圳";
            case NodeType.MoonPond: return "月沼(牛胃)";
            case NodeType.SouthLake: return "南湖(牛肚)";
            case NodeType.Residential: return "民居水点";
            default: return "未定义";
        }
    }

    /// <summary>
    /// 寻找场景中同名的 GameObject。如果找不到，则利用节点的 elevation 高程自动生成一个 3D 三维坐标
    /// </summary>
    private Vector3 GetNodeWorldPosition(WaterNode node)
    {
        GameObject nodeGO = GameObject.Find(node.nodeName);
        if (nodeGO != null)
        {
            // 如果场景中存在该节点名称的 GameObject，使用它的位置并往上偏
            return nodeGO.transform.position + Vector3.up * 1.5f;
        }

        // 如果找不到 GameObject，根据 elevation 伪造一个平铺排列的 3D 坐标
        float hash = (float)node.nodeName.GetHashCode() % 100f;
        float x = hash * 0.15f;
        float y = (node.elevation - 80f) * 0.2f; // 高程映射
        float z = Mathf.Sin(hash) * 5f;

        return new Vector3(x, y, z);
    }
}
