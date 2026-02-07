using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This script will govern a button in the merchnat's UI
/// </summary>
public class MaskButtonMerchant : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private MaskObject _maskObject;
    [SerializeField] private Image upperImage;
    [SerializeField] private Image lowerImage;

    [SerializeField] private MerchantUIController _merchantUIController;
    #endregion

    private void Awake()
    {
        if (_merchantUIController == null)
        {
            Debug.LogError("ERROR : Oi, fix this.");
        }
    }

    public void UpdateMask(MaskObject mask)
    {
        _maskObject = mask;
        upperImage.sprite = _maskObject.GetUpperPart().MaskSprite;
        lowerImage.sprite = _maskObject.GetLowerPart().MaskSprite;
    }

    public void OnPlayerInventoryButtonClicked()
    {
        _merchantUIController.OnPlayerInventoryButtonSelected(_maskObject);
    }

    public void OnMerchantInventoryButtonClicked()
    {
        _merchantUIController.OnMerchantInventoryButtonSelected(_maskObject);
    }
}
