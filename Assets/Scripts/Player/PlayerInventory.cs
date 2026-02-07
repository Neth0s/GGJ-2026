using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that will govern the player's inventory.
/// The player's inventory is always populated with 3 MaskObjects at maximum
/// The player can TRADE one of its mask with the merchant
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    #region SINGLETON DESIGN PATTERN
    public static PlayerInventory Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    #region VARIABLES
    [SerializeField] private List<MaskObject> _masksInInventory = new List<MaskObject>();
    #endregion

    public List<MaskObject> GetMasksInInventory()
    {
        return _masksInInventory; 
    }
    public List<MaskPart> GetListUpperPartsFromInventory()
    {
        List<MaskPart> upperPartsList = new();
        foreach (MaskObject maskObject in _masksInInventory)
        {
            upperPartsList.Add(maskObject.GetUpperPart());
        }
        return upperPartsList;
    }
    public List<MaskPart> GetListLowerPartsFromInventory()
    {
        List<MaskPart> lowerPartsList = new();
        foreach (MaskObject maskObject in _masksInInventory)
        {
            lowerPartsList.Add(maskObject.GetLowerPart());
        }
        return lowerPartsList;
    }

    #region ADD AND REMOVE FUNCTIONS
    /// <summary>
    /// Function that will attempt to add a Mask to the player's inventory. if inventory is full -> warning message
    /// </summary>
    /// <param name="mask"></param>
    public void AddMaskToInventory(MaskObject mask)
    {
        if (_masksInInventory.Count >= GameParameters.MAXIMUM_MASK_INVENTORY)
        {
            Debug.LogWarning("WARNING : Cannot add mask to inventory : inventory full");
            return;
        }
        _masksInInventory.Add(mask);
    }

    /// <summary>
    /// Removes a Mask from inventory
    /// </summary>
    /// <param name="mask">Mask passed in argument</param>
    public void RemoveMaskFromInventory(MaskObject mask)
    {
        if(_masksInInventory.Contains(mask))
        {
            _masksInInventory.Remove(mask);
            //next, if the mask removed has elements present in the currently held mask -> we take the first mask of the list
            if(mask.GetLowerPart() == PlayerMaskController.Instance.currentMask.GetLowerPart() || mask.GetUpperPart() == PlayerMaskController.Instance.currentMask.GetUpperPart())
            {
                PlayerMaskController.Instance.ChangeCurrentMask(_masksInInventory[0].GetLowerPart(), _masksInInventory[0].GetUpperPart());
            }
        }
    }
    /// <summary>
    /// Removes a Mask from inventory
    /// </summary>
    /// <param name="maskIndex">Position of mask in said inventory</param>
    public void RemoveMaskFromInventory(int maskIndex)
    {
        _masksInInventory.Remove(_masksInInventory[maskIndex]);
        //next, if the mask removed has elements present in the currently held mask -> we take the first mask of the list
        if (_masksInInventory[maskIndex].GetLowerPart() == PlayerMaskController.Instance.currentMask.GetLowerPart() || _masksInInventory[maskIndex].GetUpperPart() == PlayerMaskController.Instance.currentMask.GetUpperPart())
        {
            PlayerMaskController.Instance.ChangeCurrentMask(_masksInInventory[0].GetLowerPart(), _masksInInventory[0].GetUpperPart());
        }
    }
    #endregion
}
