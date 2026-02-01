using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will govern the player's inventory.
/// The player's inventory is always populated with 3 MaskObjects at maximum
/// The player can TRADE one of its mask with the merchant
/// </summary>
public class PlayerInventoryController : MonoBehaviour
{
    #region SINGLETON DESIGN PATTERN
    public static PlayerInventoryController Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    #region VARIABLES
    [SerializeField] private List<MaskObject> _masksInInventory = new List<MaskObject>();
    #endregion

    public List<MaskObject> GetListMasksInInventory()
    {
        return _masksInInventory; 
    }
    public List<MaskPart> GetListUpperPartsFromInventory()
    {
        List<MaskPart> upperPartsList = new List<MaskPart>();
        foreach (MaskObject maskObject in _masksInInventory)
        {
            upperPartsList.Add(maskObject.GetUpperPart());
        }
        return upperPartsList;
    }
    public List<MaskPart> GetListLowerPartsFromInventory()
    {
        List<MaskPart> lowerPartsList = new List<MaskPart>();
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
            Debug.Log("WARNING : Cannot add mask to inventory : inventory full");
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
        }
    }
    /// <summary>
    /// Removes a Mask from inventory
    /// </summary>
    /// <param name="maskIndex">Position of mask in said inventory</param>
    public void RemoveMaskFromInventory(int maskIndex)
    {
        _masksInInventory.Remove(_masksInInventory[maskIndex]);
    }
    #endregion
}
