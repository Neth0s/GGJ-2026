using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that handle the mask of the player
/// </summary>
public class PlayerMaskController : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] public MaskObject currentMask;
    [SerializeField] public SpriteRenderer playerVisualMaskUpper;
    [SerializeField] public SpriteRenderer playerVisualMaskLower;
    #endregion

    private void Start()
    {
        UpdateMaskVisual();
    }


    public void ChangeCurrentMask(MaskPart lower, MaskPart upper)
    {
        currentMask.SetLowerPart(lower);
        currentMask.SetUpperPart(upper);
        UpdateMaskVisual();
    }

    private void UpdateMaskVisual(){
        playerVisualMaskUpper.sprite = currentMask.GetUpperPart().MaskSprite;
        playerVisualMaskLower.sprite = currentMask.GetLowerPart().MaskSprite;

    }

    public bool ComparePlayerGroupMask(List<MaskProperty> upperMaskReqs, List<MaskProperty> lowerMaskReqs)
    {
        //By the grace of Shaddam IV of House Corino, I swear on the ten gods : there are ways to modularise this shit
        //We start by comparing upper mask part :
        foreach (MaskProperty groupProp in upperMaskReqs)
        {
            bool isInMaskPart = false;
            foreach(MaskProperty playerMaskPartProp in currentMask.GetUpperPart().MaskProperties)
            {
                if(playerMaskPartProp == groupProp)
                {
                    isInMaskPart = true; 
                    break; 
                }
            }
            if (!isInMaskPart)
            {
                Debug.Log("MASKS DO NOT MATCH !!!!");
                return false;
            }
        }
        //We continue by comparing lower mask part :
        foreach (MaskProperty groupProp in lowerMaskReqs)
        {
            bool isInMaskPart = false;
            foreach (MaskProperty playerMaskPartProp in currentMask.GetLowerPart().MaskProperties)
            {
                if (playerMaskPartProp == groupProp)
                {
                    isInMaskPart = true;
                    break;
                }
            }
            if (!isInMaskPart)
            {
                Debug.Log("MASKS DO NOT MATCH !!!!");
                return false;
            }
        }
        return true;
    }
}
