using System;
using UnityEngine;

/// <summary>
/// 挂载在每个代表水渠（WaterEdge）的 GameObject 上的表现层渲染脚本。
/// 负责每帧读取对应的 WaterEdge 数据，并动态更新 LineRenderer 的粗细、颜色和流速动画。
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class WaterLineRenderer : MonoBehaviour
{
    [Header("数据绑定 (Data Binding)")]
    [Tooltip("当前渲染对应的水网边数据。可通过外部初始化脚本进行绑定。")]
    public WaterEdge boundEdge;

    [Tooltip("用于在没有外部绑定时，尝试根据起始/终止节点名称在管理器中查找。")]
    public string fromNodeName;
    public string toNodeName;

    [Header("宽度配置 (Width Settings)")]
    [Tooltip("无流量或极低流量时的线条宽度")]
    [Range(0.01f, 1f)]
    public float minWidth = 0.05f;

    [Tooltip("达到最大流量阈值时的线条宽度")]
    [Range(0.01f, 2f)]
    public float maxWidth = 0.4f;

    [Tooltip("最大流量参考值，用于宽度和流速的归一化映射")]
    public float maxFlowReference = 50f;

    [Header("颜色与水质配置 (Color & Quality Settings)")]
    [Tooltip("高水质 (接近 100) 时的清澈亮蓝色")]
    public Color cleanColor = new Color(0.1f, 0.6f, 1f, 1f);

    [Tooltip("中等水质 (约 70) 时的过渡色")]
    public Color mediumColor = new Color(0.3f, 0.5f, 0.7f, 1f);

    [Tooltip("低水质 (跌破 40) 时的浑浊灰褐色")]
    public Color murkyColor = new Color(0.45f, 0.4f, 0.35f, 1f);

    [Tooltip("是否使用渐变色。若为 true，LineRenderer 的两端将分别呈现 fromNode 和 toNode 的水质颜色")]
    public bool enableFlowGradient = true;

    [Header("水流纹理动画 (Texture Animation)")]
    [Tooltip("是否通过平移材质贴图偏移 (Texture Offset) 来模拟水流动画")]
    public bool animateFlow = true;

    [Tooltip("水流动画基础速度")]
    public float baseScrollSpeed = 0.5f;

    [Tooltip("纹理平铺 (Tiling) 重复率")]
    public float textureTilingMultiplier = 1f;

    // 私有组件缓存
    private LineRenderer lineRenderer;
    private Material lineMaterial;
    private float currentTextureOffset = 0f;

    private void Awake()
    {
        // 缓存组件，避免每帧 GetComponent 造成性能损耗
        lineRenderer = GetComponent<LineRenderer>();
        
        // 获取材质实例。注意：在编辑器中运行或频繁实例化时，请确保材质支持 _MainTex 偏移
        if (lineRenderer != null)
        {
            lineMaterial = lineRenderer.material;
            
            // 确保 LineRenderer 使用世界空间坐标（根据项目需求，通常水网画线使用世界坐标）
            lineRenderer.useWorldSpace = true;
        }
    }

    private void Start()
    {
        // 尝试自动查找绑定的数据源（如果尚未由外部管理器 Bind）
        if (boundEdge == null)
        {
            TryFindEdgeInManager();
        }
        
        // 动态设置 Tiling 确保拉伸比例正常
        UpdateLineTiling();
    }

    private void Update()
    {
        if (lineRenderer == null || boundEdge == null) return;

        // 1. 获取实时数据
        float currentFlow = GetCurrentFlowRate();
        float fromQuality = boundEdge.fromNode != null ? boundEdge.fromNode.waterQuality : 100f;
        float toQuality = boundEdge.toNode != null ? boundEdge.toNode.waterQuality : 100f;

        // 2. 动态调整线条粗细 (Width)
        UpdateLineWidth(currentFlow);

        // 3. 动态调整线条颜色 (Color/Gradient)
        UpdateLineColors(fromQuality, toQuality);

        // 4. 模拟水流动画 (Texture Scrolling)
        if (animateFlow && currentFlow > 0.01f)
        {
            AnimateWaterFlow(currentFlow);
        }
    }

    /// <summary>
    /// 外部初始化绑定接口，方便在生成水网时直接注入数据源
    /// </summary>
    public void Bind(WaterEdge edge)
    {
        boundEdge = edge;
        UpdateLineTiling();
    }

    /// <summary>
    /// 获取当前边的实时流量。
    /// 由于 WaterEdge 本身可能没有存储 currentFlow 状态，这里通过计算或查询管理器获得。
    /// </summary>
    private float GetCurrentFlowRate()
    {
        if (boundEdge == null) return 0f;

        // 方法 A：如果 WaterEdge 中已经由管理器写入了实时流量字段，直接读取 (推荐)
        // return boundEdge.currentFlowRate;

        // 方法 B：如果使用的是原始 of WaterNetworkManager 演算逻辑：
        // 流量 = Mathf.Min(potentialFlow, edge.maxFlowRate) * edge.gateOpenRatio;
        if (boundEdge.fromNode == null || boundEdge.toNode == null) return 0f;
        
        // 如果上游没有水，流量为0
        if (boundEdge.fromNode.currentVolume <= 0 && boundEdge.fromNode.type != NodeType.RiverSource) 
            return 0f;

        float deltaH = boundEdge.fromNode.elevation - boundEdge.toNode.elevation;
        if (deltaH <= 0) return 0f;

        float potentialFlow = deltaH * 15f * (boundEdge.width * 10f);
        float calculatedFlow = Mathf.Min(potentialFlow, boundEdge.maxFlowRate) * boundEdge.gateOpenRatio;
        
        return calculatedFlow;
    }

    /// <summary>
    /// 根据当前流量在 [0, maxFlowReference] 范围内的比例，线性插值设置线条宽度。
    /// </summary>
    private void UpdateLineWidth(float flowRate)
    {
        // 归一化比例 (0 到 1)
        float t = Mathf.Clamp01(flowRate / maxFlowReference);
        
        // 使用平滑插值 (Lerp) 获得平滑过渡的宽度
        float targetWidth = Mathf.Lerp(minWidth, maxWidth, t);

        // 动态设置 LineRenderer 的首尾宽度
        lineRenderer.startWidth = targetWidth;
        lineRenderer.endWidth = targetWidth;
    }

    /// <summary>
    /// 根据水质动态更新 LineRenderer 的颜色渐变。
    /// 水质在 [0, 100] 区间。接近 100 为亮蓝色，中等为浅蓝，低于 40 时平滑过渡到灰褐色。
    /// </summary>
    private void UpdateLineColors(float fromQuality, float toQuality)
    {
        if (enableFlowGradient)
        {
            // 如果启用渐变，LineRenderer 两端颜色分别由上游和下游节点的水质决定
            Color colorStart = EvaluateColorByQuality(fromQuality);
            Color colorEnd = EvaluateColorByQuality(toQuality);

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(colorStart, 0.0f), 
                    new GradientColorKey(colorEnd, 1.0f) 
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(colorStart.a, 0.0f), 
                    new GradientAlphaKey(colorEnd.a, 1.0f) 
                }
            );
            lineRenderer.colorGradient = gradient;
        }
        else
        {
            // 若不启用渐变，则整体使用上游节点的水质颜色
            Color mainColor = EvaluateColorByQuality(fromQuality);
            lineRenderer.startColor = mainColor;
            lineRenderer.endColor = mainColor;
        }
    }

    /// <summary>
    /// 核心辅助算法：根据单点水质评定对应的渲染颜色。
    /// 水质 [100 -> 70]: 清澈亮蓝平滑过渡到中等偏灰蓝
    /// 水质 [70 -> 40 -> 0]: 从中等偏灰蓝，加速跌落并平滑过渡到浑浊灰褐色
    /// </summary>
    private Color EvaluateColorByQuality(float quality)
    {
        quality = Mathf.Clamp(quality, 0f, 100f);

        if (quality >= 70f)
        {
            // 水质良好区间：在 [70, 100] 之间插值 (Clean -> Medium)
            float t = (quality - 70f) / 30f; // 映射至 [0, 1]
            return Color.Lerp(mediumColor, cleanColor, t);
        }
        else if (quality >= 40f)
        {
            // 水质洗涤跌落区间：在 [40, 70] 之间插值 (Murky -> Medium)
            float t = (quality - 40f) / 30f; // 映射至 [0, 1]
            return Color.Lerp(murkyColor, mediumColor, t);
        }
        else
        {
            // 水质严重恶化区间 (< 40)：完全呈现浑浊色，可随水质进一步探底而略微加深
            float t = quality / 40f; // 映射至 [0, 1]
            // 让极度污浊的水带有一点更深暗的色调
            Color superMurkyColor = murkyColor * 0.7f;
            superMurkyColor.a = murkyColor.a; // 保持透明度
            return Color.Lerp(superMurkyColor, murkyColor, t);
        }
    }

    /// <summary>
    /// 动态流动动画。根据当前实时流量，改变材质的贴图偏移。
    /// 流量越大，流动速度越快，带给玩家极其直观的视觉反馈。
    /// </summary>
    private void AnimateWaterFlow(float flowRate)
    {
        if (lineMaterial == null) return;

        // 流速与当前流量成正比。若流量为0，则不流动
        float speedFactor = flowRate / maxFlowReference;
        float currentSpeed = baseScrollSpeed * speedFactor;

        // 累加时间偏移，避免负数溢出
        currentTextureOffset -= currentSpeed * Time.deltaTime;
        currentTextureOffset %= 1f;

        // 设置纹理主贴图偏移 (标准 Unity Shader 中的 _MainTex 属性)
        // 提示：若使用的是 URP Line Shader，请确保对应的属性名称正确（如 "_BaseMap"）
        if (lineMaterial.HasProperty("_MainTex"))
        {
            lineMaterial.SetTextureOffset("_MainTex", new Vector2(currentTextureOffset, 0f));
        }
        else if (lineMaterial.HasProperty("_BaseMap"))
        {
            lineMaterial.SetTextureOffset("_BaseMap", new Vector2(currentTextureOffset, 0f));
        }
    }

    /// <summary>
    /// 自动调整 Tiling 平铺比例，防止水渠拉长时贴图被严重拉伸。
    /// </summary>
    private void UpdateLineTiling()
    {
        if (lineRenderer == null || lineMaterial == null) return;

        // 计算 LineRenderer 的总长度
        float length = 0f;
        int positionCount = lineRenderer.positionCount;
        if (positionCount < 2) return;

        for (int i = 0; i < positionCount - 1; i++)
        {
            length += Vector3.Distance(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1));
        }

        // 默认让贴图横向平铺，长度越长，Tiling 越大
        float tilingX = length * textureTilingMultiplier;
        Vector2 scale = new Vector2(tilingX, 1f);

        if (lineMaterial.HasProperty("_MainTex"))
        {
            lineMaterial.SetTextureScale("_MainTex", scale);
        }
        else if (lineMaterial.HasProperty("_BaseMap"))
        {
            lineMaterial.SetTextureScale("_BaseMap", scale);
        }
    }

    /// <summary>
    /// 容错机制：当未通过代码直接绑定时，尝试在场景中寻找 WaterNetworkManager 
    /// 并根据节点名称字符串匹配并绑定对应的 WaterEdge 数据源。
    /// </summary>
    private void TryFindEdgeInManager()
    {
        WaterNetworkManager manager = FindFirstObjectByType<WaterNetworkManager>();
        if (manager == null) return;

        foreach (var edge in manager.edges)
        {
            if (edge.fromNode != null && edge.toNode != null)
            {
                if (edge.fromNode.nodeName == fromNodeName && edge.toNode.nodeName == toNodeName)
                {
                    Bind(edge);
                    break;
                }
            }
        }
    }
}
