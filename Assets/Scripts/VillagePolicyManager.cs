using System.Collections.Generic;
using UnityEngine;

public enum PolicyType { MorningDrinkEveningWash, DitchSiltClearing, RaiseRiverTurtles, FieldsFloodDiverting }

public class VillagePolicyManager : MonoBehaviour
{
    private WaterNetworkManager networkManager;
    public float clanPrestige = 100f;  
    public float silverCoins = 500f;   
    private HashSet<PolicyType> activePolicies = new HashSet<PolicyType>();
    private Dictionary<WaterEdge, float> ditchSiltLevels = new Dictionary<WaterEdge, float>();
    private HashSet<WaterNode> turtleHabitats = new HashSet<WaterNode>();

    void Start()
    {
        networkManager = GetComponent<WaterNetworkManager>();
        foreach (var edge in networkManager.edges) ditchSiltLevels[edge] = 0f;
    }

    public void UpdatePolicyEffects(float deltaTime)
    {
        HandleSiltAccumulation(deltaTime);
        HandleTurtleEcosystem(deltaTime);
        if (activePolicies.Contains(PolicyType.MorningDrinkEveningWash) && networkManager.currentPeriod == "Morning")
        {
            foreach (var node in networkManager.nodes) if (node.type == NodeType.Residential) node.localPollutionRate = 0.1f;
        }
    }

    private void HandleSiltAccumulation(float deltaTime)
    {
        foreach (var edge in networkManager.edges)
        {
            float flow = edge.maxFlowRate * edge.gateOpenRatio;
            float rainModifier = networkManager.rainIntensity > 0 ? 2.0f : 1.0f;
            ditchSiltLevels[edge] += (flow < edge.maxFlowRate * 0.3f) ? (0.005f * rainModifier * deltaTime) : (-0.002f * deltaTime);
            ditchSiltLevels[edge] = Mathf.Clamp01(ditchSiltLevels[edge]);
            edge.maxFlowRate = 100f * (1f - ditchSiltLevels[edge] * 0.7f);
        }
    }

    public void ExecuteDitchClearing()
    {
        if (silverCoins >= 150f) { silverCoins -= 150f; List<WaterEdge> keys = new List<WaterEdge>(ditchSiltLevels.Keys); foreach (var edge in keys) ditchSiltLevels[edge] = 0f; }
    }

    public void DeployTurtleToNode(WaterNode node)
    {
        if (silverCoins >= 30f) { silverCoins -= 30f; turtleHabitats.Add(node); }
    }

    private void HandleTurtleEcosystem(float deltaTime)
    {
        foreach (var node in turtleHabitats) foreach (var edge in networkManager.edges)
            if ((edge.toNode == node || edge.fromNode == node) && ditchSiltLevels.ContainsKey(edge))
                ditchSiltLevels[edge] = Mathf.Max(0f, ditchSiltLevels[edge] - 0.01f * deltaTime);
    }

    public float GetSiltLevel(WaterEdge edge)
    {
        if (ditchSiltLevels.ContainsKey(edge)) return ditchSiltLevels[edge];
        return 0f;
    }
}
