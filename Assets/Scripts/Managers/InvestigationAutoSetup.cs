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
        InvestigationPlayer player = Object.FindFirstObjectByType<InvestigationPlayer>();
        if (player == null)
        {
            Debug.LogError("Setup Error: No InvestigationPlayer found in scene!");
            return;
        }

        // 3. Find a Group to monitor
        GroupController group = Object.FindFirstObjectByType<GroupController>();
        if (group == null)
        {
            Debug.LogError("Setup Error: No GroupController found in scene!");
            return;
        }

        // 4. Spawn Guard if not present
        GuardController existingGuard = Object.FindFirstObjectByType<GuardController>();
        if (existingGuard == null)
        {
            GameObject guardObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            guardObj.name = "Auto-Spawned Guard";
            guardObj.transform.position = group.transform.position + new Vector3(2, 0, 2);
            
            // Add GuardController
            GuardController guard = guardObj.AddComponent<GuardController>();
            
            // Link references
            // Note: We need to use Reflection or make fields public if we can't set them easily here.
            // I'll update GuardController to have a Setup method.
            guard.Setup(player, group);
            
            Debug.Log("Created Guard and linked to Player and Group.");
        }
        else
        {
            Debug.Log("Guard already exists in scene.");
        }
    }
}
