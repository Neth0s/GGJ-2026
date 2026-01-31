using UnityEngine;

/// <summary>
/// Controls the Guard's suspicion behavior.
/// </summary>
public class GuardController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float suspicionIncreaseRate = 1.0f; // Seconds to get caught
    [SerializeField] private float suspicionDecreaseRate = 2.0f;

    [Header("References")]
    [SerializeField] private InvestigationPlayer player; 
    [SerializeField] private GroupController currentGroup; 

    private float _currentSuspicion = 0f;

    public void Setup(InvestigationPlayer playerRef, GroupController groupRef)
    {
        player = playerRef;
        currentGroup = groupRef;
    }

    private void Update()
    {
        if (player == null) return; 

        if (IsPlayerInTerritory())
        {
            if (IsPlayerSuspicious())
            {
                _currentSuspicion += Time.deltaTime * suspicionIncreaseRate;
                Debug.Log($"Suspicion: {_currentSuspicion}"); // TODO: Connect to UI

                if (_currentSuspicion >= 3.0f) 
                {
                    DetectiveManager.Instance.CaughtByGuard();
                }
            }
            else
            {
                _currentSuspicion -= Time.deltaTime * suspicionDecreaseRate;
                if (_currentSuspicion < 0) _currentSuspicion = 0;
            }
        }
        else
        {
            _currentSuspicion = 0;
        }
    }

    private bool IsPlayerInTerritory()
    {
        // Simple distance check for MVP
        if (currentGroup == null) return false;
        return Vector3.Distance(currentGroup.transform.position, player.transform.position) < 5.0f;
    }

    private bool IsPlayerSuspicious()
    {
        // Verify if player mask matches the group requirement
        string playerMask = player.CurrentMaskFeature;
        
        if (currentGroup == null) return false;

        // If player has NO mask or WRONG mask, they are suspicious
        // Assuming CommonFeature is the required mask feature
        return playerMask != currentGroup.Data.CommonFeature;
    }
}
