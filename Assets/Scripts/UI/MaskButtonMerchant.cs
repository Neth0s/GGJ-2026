using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script will govern a button in the merchant's UI
/// </summary>
[RequireComponent(typeof(Button))]
public class MaskButtonMerchant : MonoBehaviour
{
    [SerializeField] private Image upperImage;
    [SerializeField] private Image lowerImage;

    private MerchantUIController _merchantUIController;
    private MaskObject _maskObject;
    private bool isPlayerButton;

    public void Setup(MerchantUIController merchantUIController, bool playerButton)
    {
        _merchantUIController = merchantUIController;
        isPlayerButton = playerButton;
    }

    public void UpdateMask(MaskObject mask)
    {
        _maskObject = mask;
        upperImage.sprite = _maskObject.UpperPart().MaskSprite;
        lowerImage.sprite = _maskObject.LowerPart().MaskSprite;
    }

    public void OnClick()
    {
        if (isPlayerButton) _merchantUIController.OnPlayerButtonSelected(_maskObject);
        else _merchantUIController.OnMerchantButtonSelected(_maskObject);
    }
}
