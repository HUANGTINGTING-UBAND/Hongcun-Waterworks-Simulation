using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 3D NPC Villager AI class for Hongcun simulation.
/// Directs villagers to navigate from their current position to random water nodes (like MoonPond or SouthLake) to wash or fetch water.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class VillagerAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private WaterNetworkManager networkManager;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private string currentTargetNodeName = "";

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        networkManager = FindFirstObjectByType<WaterNetworkManager>();

        // Set NavMeshAgent movement characteristics for comfortable pedestrian feel
        if (agent != null)
        {
            agent.speed = Random.Range(1.5f, 3.5f);
            agent.acceleration = 8f;
            agent.angularSpeed = 120f;
            agent.stoppingDistance = 1.2f;
        }

        // Color villager uniquely for aesthetic variety
        Renderer r = GetComponentInChildren<Renderer>();
        if (r != null)
        {
            r.material.color = new Color(Random.Range(0.2f, 0.6f), Random.Range(0.2f, 0.6f), Random.Range(0.5f, 0.9f));
        }

        ChooseNewTarget();
    }

    private void Update()
    {
        if (agent == null || networkManager == null || networkManager.nodes.Count == 0) return;

        // If we are at the target, wait for a short duration (to simulate water collection or washing) and then move on
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = Random.Range(3f, 8f);
            }
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    isWaiting = false;
                    ChooseNewTarget();
                }
            }
        }
    }

    private void ChooseNewTarget()
    {
        if (networkManager == null || networkManager.nodes.Count == 0) return;

        // Select a random node that is active in current era
        int attempts = 0;
        WaterNode selectedNode = null;
        while (attempts < 10)
        {
            int index = Random.Range(0, networkManager.nodes.Count);
            WaterNode temp = networkManager.nodes[index];
            if (temp.nodeName != currentTargetNodeName)
            {
                selectedNode = temp;
                break;
            }
            attempts++;
        }

        if (selectedNode == null)
        {
            selectedNode = networkManager.nodes[0];
        }

        currentTargetNodeName = selectedNode.nodeName;

        // Locate corresponding 3D GameObject in scene
        GameObject nodeGO = GameObject.Find(selectedNode.nodeName);
        if (nodeGO != null && agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(nodeGO.transform.position);
        }
    }
}
