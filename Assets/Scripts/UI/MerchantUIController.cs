using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This script will govern the UI-side of the merchant
/// </summary>
public class MerchantUIController : MonoBehaviour
{
    #region VARIABLES
    [Header("Inventory")]
    [SerializeField] private List<MaskObject> _merchantInventory = new List<MaskObject>();
    [Header("Player Choice UI elements")]
    [SerializeField] private GameObject _playerChoicePage;
    [SerializeField] private List<MaskButtonMerchant> _playerInventoryButtons = new List<MaskButtonMerchant>();
    [Header("Merchant UI elements")]
    [SerializeField] private GameObject _merchantChoicePage;
    [SerializeField] private List<MaskButtonMerchant> _merchantButtons = new List<MaskButtonMerchant>();

    private PlayerInventory _playerInventory;
    #endregion

    private void Awake()
    {
        if(_merchantButtons.Count < 1)
        {
            Debug.LogWarning("WARNING : Merchant button elements list must not be null");
        }
        if (_playerInventoryButtons.Count < 1)
        {
            Debug.LogWarning("WARNING : Player inventory buttons must not be null");
        }
        if (_merchantInventory.Count < 1)
        {
            Debug.LogWarning("WARNING : Merchant inventory is empty");
        }
    }

    /// <summary>
    /// Function that will display the Player choice page or the merchant choice page
    /// </summary>
    /// <param name="status">True to display player choice, false for merchant choice page</param>
    private void ActivatePlayerChoicePage(bool status)
    {
        if (status)
        {
            _playerChoicePage.SetActive(true);
            _merchantChoicePage.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_playerInventoryButtons[0].gameObject);
        }
        else
        {
            _playerChoicePage.SetActive(false);
            _merchantChoicePage.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_merchantButtons[0].gameObject);
        }
    }

    /// <summary>
    /// Will setup the UI of the Merchant UI Controller, setting the masks for each buttons
    /// </summary>
    public void InitializeUI()
    {
        //First we begin by setting, behind the scenes, the various buttons :
        // The player choice buttons :
        List<MaskObject> playerInventoryMasks = PlayerInventory.Instance.GetMasksInInventory();
        for (int ite = 0; ite < _playerInventoryButtons.Count; ite++)
        {
            _playerInventoryButtons[ite].UpdateMask(playerInventoryMasks[ite]);
        }
        // The merchant choice buttons :
        List<MaskObject> masksNotInPlayerInventory = ObtainListMasksNotInPlayerInventory();
        if(masksNotInPlayerInventory.Count != _merchantButtons.Count)
        {
            Debug.LogError("ERROR : This shouldn't be possible RIGHT NOW. However, if number of masks/buttons increase, this will need to change.");
        }
        for (int ite=0; ite < _merchantButtons.Count; ite++)
        {
            _merchantButtons[ite].UpdateMask(masksNotInPlayerInventory[ite]);
        }

        //Next, we start the UI of the player choice's page first :
        ActivatePlayerChoicePage(true);
    }

    /// <summary>
    /// Will return the list of masks that are absent from the player's inventory
    /// </summary>
    /// <returns></returns>
    private List<MaskObject> ObtainListMasksNotInPlayerInventory()
    {
        List<MaskObject> playerInventoryMasks = PlayerInventory.Instance.GetMasksInInventory();
        List<MaskObject> listReturn = new (_merchantInventory);
        
        foreach(var mask in playerInventoryMasks)
        {
            listReturn.Remove(mask);
        }
        return listReturn;
    }

    /// <summary>
    /// Function called by the MaskButtonMerchant buttons, when a player inventory mask has been selected
    /// </summary>
    /// <param name="selectedMask">Mask selected by the player</param>
    public void OnPlayerInventoryButtonSelected(MaskObject selectedMask)
    {
        print("Selected mask for deletion : " + selectedMask.name);
        PlayerInventory.Instance.RemoveMaskFromInventory(selectedMask);
        ActivatePlayerChoicePage(false);
    }

    /// <summary>
    /// Function called by the MaskButtonMerchant buttons, when a merchant inventory mask has been selected
    /// </summary>
    /// <param name="selectedMask"></param>
    public void OnMerchantInventoryButtonSelected(MaskObject selectedMask)
    {
        print("Selected mask for purchase : " + selectedMask.name);
        PlayerInventory.Instance.AddMaskToInventory(selectedMask);
        UIManager.Instance.DisplayMerchantUI(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LeaveMerchant();
    }
}
