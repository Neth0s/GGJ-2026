using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will govern the detection by the player towards NPCs and other shits like that
/// </summary>
[RequireComponent(typeof(PlayerMaskController))]
[RequireComponent(typeof(DarkController))]
public class DetectController : MonoBehaviour
{
    #region VARIABLES
    [Header("Detect Parameters")]
    [SerializeField] private float _detectRadius = 1f;

    [Header("Detected elements")]
    [SerializeField] private NPCController _currentlySelectedNPC = null; //CAN BE NULL
    [SerializeField] private GroupController _currentlySelectedGroup = null; //CAN BE NULL
    [SerializeField] private MerchantController _currentlySelectedMerchant = null; //CAN BE NULL
    public bool masksAreOK = false;

    private PlayerMaskController _playerMaskController;
    private InvestigationPlayer player;
    private DarkController _playerDarkController;
    #endregion

    #region GETTERS AND SETTERS
    public NPCController GetCurrentlySelectedNPC() { return _currentlySelectedNPC; }
    public GroupController GetCurrentlySelectedGroup() { return _currentlySelectedGroup; }
    #endregion

    private void Awake()
    {
        _playerMaskController = GetComponent<PlayerMaskController>();
        _playerDarkController = GetComponent<DarkController>();
        player = GetComponent<InvestigationPlayer>();
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
            if (closestNPC)
            {
                player.EnableInteraction(closestNPC);
            }
            else
            {
                player.DisableAccusationInteraction();
            }
        }

        GroupController groupController = ObtainGroupController();
        if(_currentlySelectedGroup != groupController)
        {
            _currentlySelectedGroup?.GetAppearController().Hide(); //we're forced to put it here cause otherwise we'll lose the last group
            _currentlySelectedGroup = groupController; //Group CAN be null
            _currentlySelectedGroup?.TriggerGroupSelection();
            //TODO : regroup following ifs
            if (_currentlySelectedGroup != null)
            {
                masksAreOK = _playerMaskController.ComparePlayerGroupMask(_currentlySelectedGroup.GetUpperMaskRequirements(), _currentlySelectedGroup.GetLowerMaskRequirements());
                
                print("Do you fit in group with mask : " + masksAreOK);
                if (masksAreOK)
                {
                    _playerDarkController.Darken();
                }
                _playerDarkController.Darken();
                _currentlySelectedGroup.GetAppearController().Appear();
            }
            else 
            { 
                masksAreOK = false;
                _playerDarkController.Brighten();
            }

            if (_currentlySelectedGroup) player.EnableInteraction(_currentlySelectedGroup);
            else player.DisableInteraction();
        }

        MerchantController merchantController = ObtainMerchantController();
        if (_currentlySelectedMerchant != merchantController)
        {
            _currentlySelectedMerchant = merchantController;
            if (_currentlySelectedMerchant != null)
            {
                player.EnableInteraction(_currentlySelectedMerchant);
            }
            else
            {
                player.DisableInteraction();
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

    #region DETECT MERCHANT
    /// <summary>
    /// Function that will return the merchant controller if found
    /// </summary>
    /// <returns></returns>
    private MerchantController ObtainMerchantController()
    {
        Collider[] collidersDetected = Physics.OverlapSphere(gameObject.transform.position, _detectRadius);
        foreach (var collider in collidersDetected)
        {
            if (collider.gameObject.GetComponent<MerchantController>() != null)
            {
                return collider.gameObject.GetComponent<MerchantController>();
            }
        }
        return null;
    }
    #endregion
}
