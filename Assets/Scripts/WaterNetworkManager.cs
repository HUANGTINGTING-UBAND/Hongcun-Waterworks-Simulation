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
    }

    private float CalculateEdgeFlow(WaterEdge edge)
    {
        if (edge.fromNode.currentVolume <= 0 && edge.fromNode.type != NodeType.RiverSource) return 0f;
        float deltaH = edge.fromNode.elevation - edge.toNode.elevation;
        if (deltaH <= 0) return 0f; 
        float potentialFlow = deltaH * 15f * (edge.width * 10f); 
        return Mathf.Min(potentialFlow, edge.maxFlowRate) * edge.gateOpenRatio;
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
}
