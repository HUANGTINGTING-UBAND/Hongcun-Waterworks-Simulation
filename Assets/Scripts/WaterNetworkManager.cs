using System;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType { RiverSource, Gate, NormalDitch, MoonPond, SouthLake, Residential }

public class WaterNode
{
    public string nodeName;
    public NodeType type;
    public float currentVolume;    
    public float maxCapacity;      
    public float elevation;        
    public float waterQuality = 100f; 
    public float localPollutionRate = 0f; 
    public float lastTickInflow = 0f;
    public float lastTickOutflow = 0f;
    public bool IsFlooding => currentVolume > maxCapacity;
}

public class WaterEdge
{
    public WaterNode fromNode;
    public WaterNode toNode;
    public float length;           
    public float width;            
    public float maxFlowRate;      
    public float gateOpenRatio = 1f; 
}

public class WaterNetworkManager : MonoBehaviour
{
    public List<WaterNode> nodes = new List<WaterNode>();
    public List<WaterEdge> edges = new List<WaterEdge>();
    public float rainIntensity = 0f; 
    public string currentPeriod = "Morning"; 

    [Header("Disaster Simulation System")]
    public string activeDisaster = null; // "flood", "fire", or null
    public string fireNodeName = null;
    public float fireIntensity = 100f;
    public float disasterTimeLeft = 0f;
    public bool disasterCompleted = false;
    public string disasterResult = ""; // "success" or "failure"

    public void SimulationTick(float deltaTime)
    {
        UpdateNodeEnvironmentInputs(deltaTime);
        Dictionary<WaterEdge, float> calculatedFlows = new Dictionary<WaterEdge, float>();
        Dictionary<WaterEdge, float> calculatedQualities = new Dictionary<WaterEdge, float>();

        foreach (var edge in edges)
        {
            float flow = CalculateEdgeFlow(edge);
            calculatedFlows[edge] = flow;
            calculatedQualities[edge] = edge.fromNode.waterQuality;
        }

        ApplyFlowAndQualityChanges(calculatedFlows, calculatedQualities, deltaTime);
        ProcessNodeSpecialLogics(deltaTime);
        UpdateDisasterSimulation(calculatedFlows, deltaTime);
    }

    private float CalculateEdgeFlow(WaterEdge edge)
    {
        if (edge.fromNode.currentVolume <= 0 && edge.fromNode.type != NodeType.RiverSource) return 0f;
        float deltaH = edge.fromNode.elevation - edge.toNode.elevation;
        if (deltaH <= 0) return 0f; 
        float potentialFlow = deltaH * 15f * (edge.width * 10f); 
        float flow = Mathf.Min(potentialFlow, edge.maxFlowRate) * edge.gateOpenRatio;

        // 梅雨季山洪：西溪源头进水量飙升至 300%
        if (activeDisaster == "flood" && edge.fromNode.type == NodeType.RiverSource)
        {
            flow *= 3.0f;
        }

        return flow;
    }

    private void UpdateNodeEnvironmentInputs(float deltaTime)
    {
        foreach (var node in nodes)
        {
            node.lastTickInflow = 0f; node.lastTickOutflow = 0f;
            if (node.type != NodeType.Gate) node.currentVolume += rainIntensity * deltaTime;

            if (node.type == NodeType.Residential)
            {
                if (currentPeriod == "Morning") { node.currentVolume -= 5f * deltaTime; node.localPollutionRate = 2f; }
                else if (currentPeriod == "Evening") { node.localPollutionRate = 35f; }
                else { node.localPollutionRate = 0.5f; }
            }
        }
    }

    private void ApplyFlowAndQualityChanges(Dictionary<WaterEdge, float> flows, Dictionary<WaterEdge, float> qualities, float deltaTime)
    {
        Dictionary<WaterNode, float> nextWaterQualities = new Dictionary<WaterNode, float>();
        foreach (var node in nodes)
        {
            float totalInflow = 0f; float totalOutflow = 0f; float mixedQualityInflowProduct = 0f;
            foreach (var edge in edges)
            {
                if (edge.toNode == node) { float inflow = flows[edge] * deltaTime; totalInflow += inflow; mixedQualityInflowProduct += inflow * qualities[edge]; }
                if (edge.fromNode == node) { totalOutflow += flows[edge] * deltaTime; }
            }
            node.currentVolume += (totalInflow - totalOutflow);
            if (node.currentVolume < 0) node.currentVolume = 0;
            node.lastTickInflow = totalInflow / deltaTime; node.lastTickOutflow = totalOutflow / deltaTime;

            if (totalInflow > 0)
            {
                float existingVolume = Mathf.Max(0, node.currentVolume - totalInflow);
                float newQuality = ((existingVolume * node.waterQuality) + mixedQualityInflowProduct) / Mathf.Max(0.1f, node.currentVolume);
                nextWaterQualities[node] = Mathf.Clamp(newQuality - node.localPollutionRate * deltaTime, 0f, 100f);
            }
            else { nextWaterQualities[node] = Mathf.Clamp(node.waterQuality - (node.localPollutionRate * deltaTime), 0f, 100f); }
        }
        foreach (var node in nodes) if (nextWaterQualities.ContainsKey(node)) node.waterQuality = nextWaterQualities[node];
    }

    private void ProcessNodeSpecialLogics(float deltaTime)
    {
        foreach (var node in nodes)
        {
            if (node.type == NodeType.MoonPond && node.currentVolume > 0 && node.lastTickInflow < node.maxCapacity * 0.1f)
                node.waterQuality = Mathf.MoveTowards(node.waterQuality, 95f, 2f * deltaTime);
            if (node.IsFlooding) { float overflow = node.currentVolume - node.maxCapacity; node.currentVolume = node.maxCapacity; Debug.LogWarning($"【漫溢】{node.nodeName}: {overflow} L"); }
        }
    }

    private void UpdateDisasterSimulation(Dictionary<WaterEdge, float> flows, float deltaTime)
    {
        if (string.IsNullOrEmpty(activeDisaster)) return;

        // 倒计时
        if (disasterTimeLeft > 0)
        {
            disasterTimeLeft = Mathf.Max(0f, disasterTimeLeft - deltaTime);
            if (disasterTimeLeft <= 0)
            {
                EvaluateDisasterOutcome();
                return;
            }
        }

        if (activeDisaster == "fire" && !string.IsNullOrEmpty(fireNodeName))
        {
            WaterNode fNode = nodes.Find(n => n.nodeName == fireNodeName);
            if (fNode != null)
            {
                // 寻找流入该火灾节点的所有水渠
                List<WaterEdge> incomingEdges = edges.FindAll(e => e.toNode == fNode);
                float totalInflow = 0f;
                float totalSilt = 0f;
                
                var policyManager = GetComponent<VillagePolicyManager>();

                foreach (var edge in incomingEdges)
                {
                    totalInflow += flows.ContainsKey(edge) ? flows[edge] : 0f;
                    if (policyManager != null)
                    {
                        totalSilt += policyManager.GetSiltLevel(edge);
                    }
                }

                float avgSilt = incomingEdges.Count > 0 ? totalSilt / incomingEdges.Count : 0f;

                // 抽水灭火
                float waterDrawn = Mathf.Min(fNode.currentVolume, 12f * deltaTime);
                fNode.currentVolume -= waterDrawn;

                if (totalInflow > 3.0f)
                {
                    // 灭火速率，受总流入流量影响，且受泥沙疏淤折减
                    float extRate = Mathf.Max(1.5f, totalInflow * 0.18f * (1.0f - avgSilt * 0.75f));
                    fireIntensity = Mathf.Max(0f, fireIntensity - extRate * deltaTime);
                    if (fireIntensity <= 0f)
                    {
                        fireNodeName = null; // 成功扑灭
                    }
                }
                else
                {
                    // 流量过低火势蔓延
                    fireIntensity = Mathf.Min(100f, fireIntensity + 1.5f * deltaTime);
                    
                    // 白银、声望随火灾暴跌
                    if (policyManager != null)
                    {
                        policyManager.silverCoins = Mathf.Max(0f, policyManager.silverCoins - 12f * deltaTime);
                        policyManager.clanPrestige = Mathf.Max(10f, policyManager.clanPrestige - 3f * deltaTime);
                    }
                }
            }
        }
    }

    private void EvaluateDisasterOutcome()
    {
        disasterCompleted = true;
        
        // 评估条件：无漫溢、居民水质合格、火扑灭
        bool noFlooding = true;
        foreach (var node in nodes)
        {
            if (node.IsFlooding)
            {
                noFlooding = false;
                break;
            }
        }

        bool allWaterSafe = true;
        foreach (var node in nodes)
        {
            if (node.type == NodeType.Residential && node.waterQuality < 65f)
            {
                allWaterSafe = false;
                break;
            }
        }

        bool fireExtinguished = activeDisaster == "fire" ? (string.IsNullOrEmpty(fireNodeName) || fireIntensity <= 0f) : true;

        var policyManager = GetComponent<VillagePolicyManager>();

        if (noFlooding && allWaterSafe && fireExtinguished)
        {
            disasterResult = "success";
            if (policyManager != null)
            {
                policyManager.silverCoins += 200f;
                policyManager.clanPrestige = Mathf.Min(150f, policyManager.clanPrestige + 30f);
            }
            Debug.Log("【天灾挑战成功】族长夫人胡重对你的治水成果大加赞赏，汪氏基业得以保全！");
        }
        else
        {
            disasterResult = "failure";
            Debug.LogWarning("【天灾挑战失败】治水失败，水火无情！请调节闸门、疏浚水圳后再试一次。");
        }

        activeDisaster = null;
        fireNodeName = null;
    }
}
