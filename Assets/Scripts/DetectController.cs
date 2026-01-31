using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will govern the detection by the player towards NPCs and other shits like that
/// </summary>
[RequireComponent(typeof(PlayerMaskController))]
public class DetectController : MonoBehaviour
{
    #region VARIABLES
    [Header("Detect Parameters")]
    [SerializeField] private float _detectRadius = 5f;

    [Header("Detected elements")]
    [SerializeField] private NPCController _currentlySelectedNPC = null; //CAN BE NULL
    [SerializeField] private GroupController _currentlySelectedGroup = null; //CAN BE NULL

    //internal variables
    private PlayerMaskController _playerMaskController;
    #endregion

    #region GETTERS AND SETTERS
    public NPCController GetCurrentlySelectedNPC() { return _currentlySelectedNPC; }
    public GroupController GetCurrentlySelectedGroup() { return _currentlySelectedGroup; }
    #endregion

    private void Awake()
    {
        _playerMaskController = GetComponent<PlayerMaskController>();
    }

    private void Update()
    {
        StartDetectScan();
    }

    /// <summary>
    /// Function that will trigger the scan of the surroundings of the player
    /// </summary>
    private void StartDetectScan()
    {
        NPCController closestNPC = GetClosestNPC();
        if (closestNPC != _currentlySelectedNPC) 
        {
            //We take into account null
            _currentlySelectedNPC?.AbandonHighlight();
            _currentlySelectedNPC = closestNPC;
            closestNPC?.TriggerHightlight();
        }

        GroupController groupController = ObtainGroupController();
        if(_currentlySelectedGroup != groupController)
        {
            _currentlySelectedGroup = groupController; //Group CAN be null
            _currentlySelectedGroup?.TriggerGroupSelection();
            if (_currentlySelectedGroup != null)
            {
                bool masksAreOK = _playerMaskController.ComparePlayerGroupMask(_currentlySelectedGroup.GetUpperMaskRequirements(), _currentlySelectedGroup.GetLowerMaskRequirements());
                print("Compare result : " + masksAreOK);
            }
        }
    }

    #region DETECT NPCs
    /// <summary>
    /// Obtain the list of NPC Controllers in range of the detect radius
    /// </summary>
    /// <returns>List of NPC Controllers</returns>
    private List<NPCController> ObtainListNPCControllersInRange()
    {
        Collider[] collidersDetected = Physics.OverlapSphere(gameObject.transform.position, _detectRadius);
        List<NPCController> npcControllerList = new List<NPCController>();
        foreach (Collider collider in collidersDetected)
        {
            if (collider.gameObject.GetComponent<NPCController>() != null)
            {
                npcControllerList.Add(collider.gameObject.GetComponent<NPCController>());
            }
        }
        return npcControllerList;
    }

    /// <summary>
    /// Function that returns the closest NPC Controller detected by the player
    /// </summary>
    /// <returns></returns>
    private NPCController GetClosestNPC()
    {
        List<NPCController> npcDetected = ObtainListNPCControllersInRange();
        if (npcDetected.Count < 1) //handle case where no NPCs are found
        {
            return null;
        }
        NPCController closestNPC = npcDetected[0];
        float minDistance = Vector3.Distance(closestNPC.gameObject.transform.position, gameObject.transform.position);
        for (int i = 0; i < npcDetected.Count; i++)
        {
            float calcDistance = Vector3.Distance(npcDetected[i].gameObject.transform.position, gameObject.transform.position);
            if (calcDistance < minDistance)
            {
                closestNPC = npcDetected[i];
                minDistance = calcDistance;
            }
        }
        return closestNPC;
    }
    #endregion

    #region DETECT GROUPS
    private GroupController ObtainGroupController()
    {
        Collider[] collidersDetected = Physics.OverlapSphere(gameObject.transform.position, _detectRadius);
        foreach (var collider in collidersDetected) 
        {
            if (collider.gameObject.GetComponent<GroupController>() != null)
            {
                return collider.gameObject.GetComponent<GroupController>();
            }
        }
        return null;
    }

    #endregion
}
