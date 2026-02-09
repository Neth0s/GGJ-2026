using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MerchantUIController : MonoBehaviour
{
    #region VARIABLES
    [Header("Player Choice UI elements")]
    [SerializeField] private GameObject _playerChoicePage;
    [SerializeField] private Transform _playerMaskButtonsParent;
    [SerializeField] private GameObject _playerMaskButton;

    [Header("Merchant UI elements")]
    [SerializeField] private GameObject _merchantChoicePage;
    [SerializeField] private Transform _merchantButtonsParent;
    [SerializeField] private GameObject _merchantButton;

    private MaskController _maskController;
    private MerchantController _merchantController;
    #endregion

    /// <summary>
    /// Function that will display the Player choice page or the merchant choice page
    /// </summary>
    /// <param name="page">0 for player choice page, 1 for merchant choice page</param>
    private void DisplayChoicePage(int page)
    {
        if (page == 0)
        {
            _playerChoicePage.SetActive(true);
            _merchantChoicePage.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_playerMaskButtonsParent.GetChild(0).gameObject);
        }
        else
        {
            _playerChoicePage.SetActive(false);
            _merchantChoicePage.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_merchantButtonsParent.GetChild(0).gameObject);
        }
    }

    public void InitializeUI(MaskController maskController, MerchantController merchantController)
    {
        _maskController = maskController;
        _merchantController = merchantController;

        List<MaskObject> _playerInventory = _maskController.MasksInventory;
        List<MaskObject> _merchantInventory = _merchantController.Inventory;

        //Remove previous buttons
        foreach (Transform child in _playerMaskButtonsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _merchantButtonsParent)
        {
            Destroy(child.gameObject);
        }

        //Setup buttons
        for (int i = 0; i < _playerInventory.Count; i++)
        {
            GameObject button = Instantiate(_playerMaskButton.gameObject, _playerMaskButtonsParent);
            MaskButtonMerchant merchantButton = button.GetComponent<MaskButtonMerchant>();
            merchantButton.Setup(this, true);
            merchantButton.UpdateMask(_playerInventory[i]);
        }

        for (int i = 0; i < _merchantInventory.Count; i++)
        {
            GameObject button = Instantiate(_merchantButton.gameObject, _merchantButtonsParent);
            MaskButtonMerchant merchantButton = button.GetComponent<MaskButtonMerchant>();
            merchantButton.Setup(this, false);
            merchantButton.UpdateMask(_merchantInventory[i]);
        }

        DisplayChoicePage(0);
    }

    public void OnPlayerButtonSelected(MaskObject selectedMask)
    {
        _maskController.RemoveMaskFromInventory(selectedMask);
        _merchantController.AddToInventory(selectedMask);

        DisplayChoicePage(1);
    }

    public void OnMerchantButtonSelected(MaskObject selectedMask)
    {
        _maskController.AddMaskToInventory(selectedMask);
        _merchantController.RemoveFromInventory(selectedMask);

        UIManager.Instance.CloseMerchantUI();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LeaveMerchant();
    }
}
