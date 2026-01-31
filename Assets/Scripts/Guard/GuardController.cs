using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Guard's suspicion behavior.
/// </summary>
public class GuardController : MonoBehaviour
{
    [Header("Roaming Settings")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float waitTimeAtGroup = 3.0f;
    [SerializeField] private float detectionRadius = 5.0f;
    [SerializeField] private float suspicionIncreaseRate = 1.0f;
    [SerializeField] private float suspicionDecreaseRate = 2.0f;

    [Header("References")]
    [SerializeField] private InvestigationPlayer player; 
    [SerializeField] private List<GroupController> patrolGroups = new List<GroupController>(); 

    private int _targetGroupIndex = 0;
    private float _waitTimer = 0f;
    private float _currentSuspicion = 0f;
    private bool _isChecking = false;

    public void Setup(InvestigationPlayer playerRef, List<GroupController> groups)
    {
        player = playerRef;
        patrolGroups = groups;
    }

    private void Update()
    {
        if (player == null || patrolGroups == null || patrolGroups.Count == 0) return;

        if (_isChecking)
        {
            UpdateCheckingState();
        }
        else
        {
            UpdateRoamingState();
        }
    }

    private void UpdateRoamingState()
    {
        GroupController target = patrolGroups[_targetGroupIndex];
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget < 0.5f)
        {
            _isChecking = true;
            _waitTimer = waitTimeAtGroup;
            Debug.Log($"Guard reached {target.name}, checking group...");
        }
        else
        {
            // Move towards target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            
            // Basic rotation towards target
            Vector3 direction = (target.transform.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
            }
        }
    }

    private void UpdateCheckingState()
    {
        _waitTimer -= Time.deltaTime;

        // In this state, we check if the player is in the territory of the CURRENT group we are visiting
        GroupController currentGroup = patrolGroups[_targetGroupIndex];

        if (IsPlayerInTerritory(currentGroup))
        {
            if (IsPlayerSuspicious(currentGroup))
            {
                _currentSuspicion += Time.deltaTime * suspicionIncreaseRate;
                Debug.Log($"Guard is suspicious! Current Suspicion: {_currentSuspicion}");

                if (_currentSuspicion >= 3.0f) 
                {
                    DetectiveManager.Instance.CaughtByGuard();
                }
            }
            else
            {
                _currentSuspicion = Mathf.Max(0, _currentSuspicion - Time.deltaTime * suspicionDecreaseRate);
            }
        }
        else
        {
            _currentSuspicion = Mathf.Max(0, _currentSuspicion - Time.deltaTime * suspicionDecreaseRate);
        }

        if (_waitTimer <= 0)
        {
            _isChecking = false;
            _targetGroupIndex = (_targetGroupIndex + 1) % patrolGroups.Count;
            Debug.Log($"Guard moving to next group: {patrolGroups[_targetGroupIndex].name}");
        }
    }

    private bool IsPlayerInTerritory(GroupController group)
    {
        if (group == null) return false;
        return Vector3.Distance(group.transform.position, player.transform.position) < detectionRadius;
    }

    private bool IsPlayerSuspicious(GroupController group)
    {
        string playerMask = player.CurrentMaskFeature;
        if (group == null || group.Data == null) return false;

        return playerMask != group.Data.CommonFeature;
    }
}
