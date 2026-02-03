using UnityEngine;

/// <summary>
/// Automatically sets up the Investigation scene components at runtime.
/// </summary>
public class InvestigationAutoSetup : MonoBehaviour
{
    private void Start()
    {
        SetupScene();
    }

    public void SetupScene()
    {
        Debug.Log("Starting Investigation Auto-Setup...");

        // 1. Ensure DetectiveManager exists
        if (DetectiveManager.Instance == null)
        {
            GameObject detectiveObj = new GameObject("DetectiveManager");
            detectiveObj.AddComponent<DetectiveManager>();
            Debug.Log("Created DetectiveManager.");
        }

        // 2. Find Player (InvestigationPlayer)
        Player player = Object.FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogError("Setup Error: No InvestigationPlayer found in scene!");
            return;
        }

        // 3. Find all Groups for patrol
        GroupController[] groups = Object.FindObjectsByType<GroupController>(FindObjectsSortMode.None);
        if (groups.Length == 0)
        {
            Debug.LogError("Setup Error: No GroupControllers found in scene!");
            return;
        }

        // 4. Spawn Guard if not present
        GuardController existingGuard = Object.FindFirstObjectByType<GuardController>();
        if (existingGuard == null)
        {
            GameObject guardObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            guardObj.name = "Auto-Spawned Guard";
            guardObj.transform.position = groups[0].transform.position + new Vector3(2, 0, 2);
            
            // Add GuardController
            GuardController guard = guardObj.AddComponent<GuardController>();
            
            // Use NavMeshAgent for intelligent pathfinding
            UnityEngine.AI.NavMeshAgent agent = guardObj.AddComponent<UnityEngine.AI.NavMeshAgent>();
            agent.speed = 3.5f;
            agent.acceleration = 8f;
            agent.angularSpeed = 120f;
            agent.stoppingDistance = 0.6f;

            // Link references
            guard.Setup(player, new System.Collections.Generic.List<GroupController>(groups));
            
            Debug.Log("Created Guard with NavMeshAgent for pathfinding.");
        }
        else
        {
            Debug.Log("Guard already exists in scene.");
        }
    }
}
