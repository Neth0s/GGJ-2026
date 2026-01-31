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
    //[SerializeField] private List<MaskPart> lowerParts = new List<MaskPart>(); //todo

    private MaskPart currentDisplayUpperPart;
    private int currentIndexUpperPart;
    //private MaskPart currentDisplayLowerPart;

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
        //todo
    }

    public void UpperRightButton()
    {
        //todo
    }
}
