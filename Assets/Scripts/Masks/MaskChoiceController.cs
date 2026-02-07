using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This script will govern the "make a mask" window 
/// </summary>
public class MaskChoiceController : MonoBehaviour
{
    #region VARIABLES
    [Header("Mask Choice elements")]
    [SerializeField] private List<MaskPart> upperParts = new List<MaskPart>(); //to change -> get this list on player inventory
    [SerializeField] private Image upperImage;
    [SerializeField] private List<MaskPart> lowerParts = new List<MaskPart>(); //to change -> get this list on player inventory
    [SerializeField] private Image lowerImage;
    [SerializeField] private GameObject _startButton;

    [Header("Player inventory display elements")]
    [SerializeField] private List<MaskUIElementController> _playerInventoryDisplays = new List<MaskUIElementController>();

    private MaskPart currentDisplayUpperPart;
    private int currentIndexUpperPart;
    private MaskPart currentDisplayLowerPart;
    private int currentIndexLowerPart;

    private PlayerMaskController playerMaskController;
    private PlayerInventory _playerInventoryController;
    #endregion

    /// <summary>
    /// Function that will recover the data from the player inventory
    /// </summary>
    private void RecoverDataFromPlayerInventory()
    {
        _playerInventoryController = PlayerInventory.Instance;
        upperParts = _playerInventoryController.GetListUpperPartsFromInventory();
        lowerParts = _playerInventoryController.GetListLowerPartsFromInventory();
    }

    /// <summary>
    /// This function will be called when UI must be initialized.
    /// </summary>
    public void InitializeUI()
    {
        playerMaskController = GameObject.FindWithTag("Player").GetComponent<PlayerMaskController>(); //rip in peace le code
        RecoverDataFromPlayerInventory(); //we recover (and update) the code 
        UpdatePlayerInventoryDisplay(); //we update the player inventory on the page

        EventSystem.current.SetSelectedGameObject(_startButton);

        currentDisplayUpperPart = playerMaskController.currentMask.GetUpperPart();
        upperImage.sprite = currentDisplayUpperPart.MaskSprite;
        currentIndexUpperPart = upperParts.IndexOf(currentDisplayUpperPart);

        currentDisplayLowerPart = playerMaskController.currentMask.GetLowerPart();
        lowerImage.sprite = currentDisplayLowerPart.MaskSprite;
        currentIndexUpperPart = upperParts.IndexOf(currentDisplayLowerPart);
    }

    /// <summary>
    /// Will update the visuals of the player's inventory
    /// </summary>
    private void UpdatePlayerInventoryDisplay()
    {
        _playerInventoryController = PlayerInventory.Instance;
        List<MaskObject> maskList = _playerInventoryController.GetMasksInInventory();
        if (maskList.Count != _playerInventoryDisplays.Count) { Debug.LogError("ERROR : this shouldn't happen right now."); }
        for(int ite=0; ite<maskList.Count; ite++)
        {
            _playerInventoryDisplays[ite].UpdateMaskVisual(maskList[ite]);
        }
    }

    #region BUTTON FUNCTIONS
    public void UpperLeftButton()
    {
        if (currentIndexUpperPart > 0) currentIndexUpperPart--;
        else currentIndexUpperPart = upperParts.Count - 1;

        MaskPart newUpperPart = upperParts[currentIndexUpperPart];
        upperImage.sprite = newUpperPart.MaskSprite;

        playerMaskController.ChangeCurrentMask(playerMaskController.currentMask.GetLowerPart(), newUpperPart);
    }

    public void UpperRightButton()
    {
       

        if (currentIndexUpperPart < upperParts.Count - 1) currentIndexUpperPart++;
        else currentIndexUpperPart = 0;

        MaskPart newUpperPart = upperParts[currentIndexUpperPart];
        upperImage.sprite = newUpperPart.MaskSprite;

        playerMaskController.ChangeCurrentMask(playerMaskController.currentMask.GetLowerPart(), newUpperPart);

    }

    public void LowerLeftButton()
    {
        if (currentIndexLowerPart > 0) currentIndexLowerPart--;
        else currentIndexLowerPart = lowerParts.Count - 1;

        MaskPart newLowerPart = lowerParts[currentIndexLowerPart];
        lowerImage.sprite = newLowerPart.MaskSprite;

        playerMaskController.ChangeCurrentMask(newLowerPart, playerMaskController.currentMask.GetUpperPart());
    }

    public void LowerRightButton()
    {
        if (currentIndexLowerPart < lowerParts.Count - 1) currentIndexLowerPart++;
        else currentIndexLowerPart = 0;

        MaskPart newLowerPart = lowerParts[currentIndexLowerPart];
        lowerImage.sprite = newLowerPart.MaskSprite;

        playerMaskController.ChangeCurrentMask(newLowerPart, playerMaskController.currentMask.GetUpperPart());

    }
    #endregion
}
