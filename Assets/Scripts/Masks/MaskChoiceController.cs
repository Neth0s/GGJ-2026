using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Governs the window allowing the player to change masks
/// </summary>
public class MaskChoiceController : MonoBehaviour
{
    #region VARIABLES
    [Header("Mask Choice Elements")]
    [SerializeField] private Image upperImage;
    [SerializeField] private Image lowerImage;
    [SerializeField] private GameObject _startButton;

    [Header("Player Inventory Display")]
    [SerializeField] private List<MaskUIElementController> _playerInventoryDisplay = new(); //TODO : make this dynamic in case the player can have more than 3 masks

    private MaskPart currentDisplayUpperPart;
    private MaskPart currentDisplayLowerPart;

    private int currentIndexUpperPart;
    private int currentIndexLowerPart;

    private MaskController _maskController;
    private List<MaskObject> _inventory;
    private List<MaskPart> _upperParts;
    private List<MaskPart> _lowerParts;
    #endregion

    /// <summary>
    /// This function will be called when UI must be initialized.
    /// </summary>
    public void InitializeUI(MaskController maskController)
    {
        _maskController = maskController;
        _inventory = _maskController.MasksInventory;
        _upperParts = _maskController.GetUpperPartsInventory();
        _lowerParts = _maskController.GetLowerPartsInventory();
        
        UpdateInventoryDisplay();
        EventSystem.current.SetSelectedGameObject(_startButton);

        currentDisplayUpperPart = _maskController.CurrentMask.UpperPart();
        upperImage.sprite = currentDisplayUpperPart.MaskSprite;
        currentIndexUpperPart = _upperParts.IndexOf(currentDisplayUpperPart);

        currentDisplayLowerPart = _maskController.CurrentMask.LowerPart();
        lowerImage.sprite = currentDisplayLowerPart.MaskSprite;
        currentIndexUpperPart = _lowerParts.IndexOf(currentDisplayLowerPart);
    }

    private void UpdateInventoryDisplay()
    {
        for(int i = 0; i < _inventory.Count; i++)
        {
            _playerInventoryDisplay[i].UpdateMaskVisual(_inventory[i]);
        }
    }

    #region BUTTON FUNCTIONS
    public void UpperLeftButton()
    {
        if (currentIndexUpperPart > 0) currentIndexUpperPart--;
        else currentIndexUpperPart = _upperParts.Count - 1;

        MaskPart newUpperPart = _upperParts[currentIndexUpperPart];
        upperImage.sprite = newUpperPart.MaskSprite;

        _maskController.ChangeCurrentMask(_maskController.CurrentMask.LowerPart(), newUpperPart);
    }

    public void UpperRightButton()
    {
        if (currentIndexUpperPart < _upperParts.Count - 1) currentIndexUpperPart++;
        else currentIndexUpperPart = 0;

        MaskPart newUpperPart = _upperParts[currentIndexUpperPart];
        upperImage.sprite = newUpperPart.MaskSprite;

        _maskController.ChangeCurrentMask(_maskController.CurrentMask.LowerPart(), newUpperPart);
    }

    public void LowerLeftButton()
    {
        if (currentIndexLowerPart > 0) currentIndexLowerPart--;
        else currentIndexLowerPart = _lowerParts.Count - 1;

        MaskPart newLowerPart = _lowerParts[currentIndexLowerPart];
        lowerImage.sprite = newLowerPart.MaskSprite;

        _maskController.ChangeCurrentMask(newLowerPart, _maskController.CurrentMask.UpperPart());
    }

    public void LowerRightButton()
    {
        if (currentIndexLowerPart < _lowerParts.Count - 1) currentIndexLowerPart++;
        else currentIndexLowerPart = 0;

        MaskPart newLowerPart = _lowerParts[currentIndexLowerPart];
        lowerImage.sprite = newLowerPart.MaskSprite;

        _maskController.ChangeCurrentMask(newLowerPart, _maskController.CurrentMask.UpperPart());
    }
    #endregion
}
