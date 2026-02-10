using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's mask inventory and current mask state, allowing masks to be added, removed, and changed
/// </summary>
public class MaskController : MonoBehaviour
{
    [SerializeField] private MaskObject currentMask;
    [SerializeField] private SpriteRenderer VisualMaskUpper;
    [SerializeField] private SpriteRenderer VisualMaskLower;
    [Space(10)]
    [SerializeField] private List<MaskObject> maskInventory = new();

    private void Awake()
    {
        SelectStartingMask();
    }

    private void Start()
    {
        UpdateMaskVisual();
    }

    #region GETTERS
    public MaskObject CurrentMask => currentMask;
    public List<MaskObject> MasksInventory => maskInventory;
    
    public bool HasMask(MaskObject mask)
    {
        return maskInventory.Contains(mask);
    }

    public List<MaskPart> GetUpperPartsInventory()
    {
        List<MaskPart> upperPartsList = new();
        foreach (MaskObject maskObject in maskInventory)
        {
            upperPartsList.Add(maskObject.UpperPart());
        }
        return upperPartsList;
    }

    public List<MaskPart> GetLowerPartsInventory()
    {
        List<MaskPart> lowerPartsList = new();
        foreach (MaskObject maskObject in maskInventory)
        {
            lowerPartsList.Add(maskObject.LowerPart());
        }
        return lowerPartsList;
    }
    #endregion

    #region CHANGE MASK
    private void SelectStartingMask()
    {
        if (maskInventory != null && maskInventory.Count > 0)
        {
            currentMask.SetLowerPart(maskInventory[0].LowerPart());
            currentMask.SetUpperPart(maskInventory[0].UpperPart());
            UpdateMaskVisual();
        }
        else
        {
            Debug.LogError("ERROR : Mask inventory is empty");
        }
    }

    public void ChangeCurrentMask(MaskPart lower, MaskPart upper)
    {
        currentMask.SetLowerPart(lower);
        currentMask.SetUpperPart(upper);
        UpdateMaskVisual();
    }

    private void UpdateMaskVisual()
    {
        VisualMaskUpper.sprite = currentMask.UpperPart().MaskSprite;
        VisualMaskLower.sprite = currentMask.LowerPart().MaskSprite;
    }
    #endregion

    #region ADD AND REMOVE MASK
    public void AddMaskToInventory(MaskObject mask)
    {
        if (!HasMask(mask)) maskInventory.Add(mask);
    }

    public void RemoveMaskFromInventory(MaskObject mask)
    {
        if (maskInventory.Contains(mask))
        {
            maskInventory.Remove(mask);

            //Current mask removed, change to the first mask in inventory
            if (mask.LowerPart() == currentMask.LowerPart() ||
                mask.UpperPart() == currentMask.UpperPart())
            {
                ChangeCurrentMask(maskInventory[0].LowerPart(), 
                                  maskInventory[0].UpperPart());
            }
        }
    }

    public void RemoveMaskFromInventory(int maskIndex)
    {
        maskInventory.Remove(maskInventory[maskIndex]);

        //Current mask removed, change to the first mask in inventory
        if (maskInventory[maskIndex].LowerPart() == currentMask.LowerPart() ||
            maskInventory[maskIndex].UpperPart() == currentMask.UpperPart())
        {
            ChangeCurrentMask(maskInventory[0].LowerPart(),
                              maskInventory[0].UpperPart());
        }
    }
    #endregion

    public bool VerifyMaskRequirements(List<MaskProperty> upperMaskReqs, List<MaskProperty> lowerMaskReqs)
    {
        foreach (MaskProperty property in upperMaskReqs)
        {
            if (!currentMask.UpperPart().MaskProperties.Contains(property))
            {
                return false;
            }
        }

        foreach (MaskProperty property in lowerMaskReqs)
        {
            if (!currentMask.LowerPart().MaskProperties.Contains(property))
            {
                return false;
            }
        }
        return true;
    }
}
