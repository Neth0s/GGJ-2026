using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will govern the UI-side of the merchant
/// </summary>
public class MerchantUIController : MonoBehaviour
{
    #region VARIABLES
    [Header("Inventory")]
    [SerializeField] private List<MaskObject> _merchantInventory = new List<MaskObject>();
    [Header("Merchant UI elements")]
    [SerializeField] private MaskButtonMerchant _merchantButton;
    #endregion

    private void Awake()
    {
        if(_merchantButton)
    }
    public void InitializeUI()
    {

    }
}
