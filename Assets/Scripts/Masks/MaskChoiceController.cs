using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskChoiceController : MonoBehaviour
{
    //hello

    [SerializeField] private List<MaskPart> upperParts = new List<MaskPart>();
    [SerializeField] private Image upperImage;
    [SerializeField] private List<MaskPart> lowerParts = new List<MaskPart>();
    [SerializeField] private Image lowerImage;

    private MaskPart currentDisplayUpperPart;
    private int currentIndexUpperPart;
    private MaskPart currentDisplayLowerPart;
    private int currentIndexLowerPart;

    private PlayerMaskController playerMaskController;

    public void InitializeUI()
    {
        playerMaskController = GameObject.FindWithTag("Player").GetComponent<PlayerMaskController>(); //rip in peace le code

        currentDisplayUpperPart = playerMaskController.currentMask.GetUpperPart();
        upperImage.sprite = currentDisplayUpperPart.MaskSprite;
        currentIndexUpperPart = upperParts.IndexOf(currentDisplayUpperPart);
    }

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
}
