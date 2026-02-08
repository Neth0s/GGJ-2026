using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will govern the detection by the player towards NPCs and other shits like that
/// </summary>
[RequireComponent(typeof(MaskController))]
[RequireComponent(typeof(SpriteDarkener))]
public class DetectController : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private float _detectRadius = 1f;

    private NPCController _currentNPC = null; //CAN BE NULL
    private GroupController _currentGroup = null; //CAN BE NULL
    private MerchantController _currentMerchant = null; //CAN BE NULL

    private MaskController _playerMask;
    private SpriteDarkener _playerDark;
    private Player player;

    private bool masksAreOK = false;
    #endregion

    #region GETTERS AND SETTERS
    public NPCController GetCurrentNPC() { return _currentNPC; }
    public GroupController GetCurrentGroup() { return _currentGroup; }
    public bool MasksAreOK => masksAreOK;
    #endregion

    private void Awake()
    {
        _playerMask = GetComponent<MaskController>();
        _playerDark = GetComponent<SpriteDarkener>();
        player = GetComponent<Player>();
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
        if (closestNPC != _currentNPC) 
        {
            if (_currentNPC != null) _currentNPC.Deselect();

            _currentNPC = closestNPC;
            if (closestNPC != null)
            {
                closestNPC.Select();
                player.EnableAccusation(closestNPC);
            }
            else player.DisableAccusation();
        }

        GroupController groupController = GetGroupInRange();

        if(_currentGroup != groupController)
        {
            if (_currentGroup != null) _currentGroup.DeselectGroup();
            _currentGroup = groupController; //Group CAN be null

            if (_currentGroup != null)
            {
                player.EnableInteraction(_currentGroup);

                List<MaskProperty> upperMaskReq = _currentGroup.GetUpperMaskRequirements();
                List<MaskProperty> lowerMaskReq = _currentGroup.GetLowerMaskRequirements();
                masksAreOK = _playerMask.VerifyMaskRequirements(upperMaskReq, lowerMaskReq);
                
                if (masksAreOK) _playerDark.Darken();
                _currentGroup.SelectGroup(masksAreOK);
            }
            else 
            { 
                masksAreOK = false;
                player.DisableInteraction();
                _playerDark.Brighten();
            }
        }

        MerchantController merchantController = GetMerchantInRange();
        if (_currentMerchant != merchantController)
        {
            _currentMerchant = merchantController;
            if (_currentMerchant != null)
            {
                player.EnableInteraction(_currentMerchant);
            }
            else
            {
                player.DisableMerchantInteraction();
            }
        }
    }

    #region DETECT NPCs
    /// <summary>
    /// Obtain the list of NPC Controllers in range of the detect radius
    /// </summary>
    /// <returns>List of NPC Controllers</returns>
    private List<NPCController> GetNPCsInRange()
    {
        Collider[] collidersDetected = Physics.OverlapSphere(gameObject.transform.position, _detectRadius);
        List<NPCController> npcControllerList = new();

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
    /// Function that returns the closest NPC Controller detected by the player, can return null
    /// </summary>
    /// <returns></returns>
    private NPCController GetClosestNPC()
    {
        List<NPCController> npcDetected = GetNPCsInRange();
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
    private GroupController GetGroupInRange()
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
    private MerchantController GetMerchantInRange()
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
