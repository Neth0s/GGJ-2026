using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls the Guard's suspicion behavior.
/// </summary>
public class GuardController : MonoBehaviour
{
    [Header("Roaming Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float waitTimeAtGroup = 3.0f;
    [Header("Detection Settings")]
    [SerializeField] private float visionRange = 7.0f;
    [SerializeField] private float detectionRadius = 5.0f;
    [SerializeField] private float timePenaltyPerSecond = 5.0f;
    [SerializeField] private float suspicionIncreaseRate = 1.0f;
    [SerializeField] private float suspicionDecreaseRate = 2.0f;

    [Header("References")]
    [SerializeField] private Player player; 
    [SerializeField] private List<GroupController> patrolGroups = new List<GroupController>(); 

    private int _targetGroupIndex = 0;
    private float _waitTimer = 0f;
    private float _currentSuspicion = 0f;
    private bool _isChecking = false;

    private NavMeshAgent _agent;

    public void Setup(Player playerRef, List<GroupController> groups)
    {
        player = playerRef;
        patrolGroups = groups;
        _agent = GetComponent<NavMeshAgent>();
        if (_agent != null) _agent.speed = moveSpeed;
    }

    private void Update()
    {
        if (player == null || patrolGroups == null || patrolGroups.Count == 0) return;
        
        if (_agent == null) 
        {
            _agent = GetComponent<NavMeshAgent>();
            if (_agent != null) _agent.speed = moveSpeed;
        }

        // 1. Core State Machine (Roaming/Checking)
        if (_isChecking)
        {
            UpdateCheckingState();
        }
        else
        {
            UpdateRoamingState();
        }

        // 2. Proximity Detection (Always active)
        CheckForProximityPenalty();
    }

    private void CheckForProximityPenalty()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < visionRange)
        {
            if (!IsPlayerSafelyBlended())
            {
                Debug.Log("Guard sees you! Losing time...");
                DetectiveManager.Instance.ApplyTimePenalty(timePenaltyPerSecond * Time.deltaTime);
            }
        }
    }

    private bool IsPlayerSafelyBlended()
    {
        // Player is only safe if they are inside a group's territory AND have the matching mask
        foreach (var group in patrolGroups)
        {
            if (IsPlayerInTerritory(group))
            {
                if (!IsPlayerSuspicious(group))
                {
                    return true; // Safely blended!
                }
            }
        }
        return false;
    }

    private void UpdateRoamingState()
    {
        GroupController target = patrolGroups[_targetGroupIndex];
        
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.SetDestination(target.transform.position);

            // Check if we arrived
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _isChecking = true;
                _waitTimer = waitTimeAtGroup;
                Debug.Log($"Guard reached {target.name}, checking group...");
            }
        }
        else
        {
            // Fallback to direct movement if NavMesh fails
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget < 0.5f)
            {
                _isChecking = true;
                _waitTimer = waitTimeAtGroup;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
                Vector3 direction = (target.transform.position - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
                }
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
        /*
        //string playerMask = player.CurrentMaskFeature;
        if (group == null || group.Data == null) return false;

        return playerMask != group.Data.CommonFeature;*
        */
        return false;
    }
}
