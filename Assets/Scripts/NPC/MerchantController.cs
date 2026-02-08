using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HoverSprite))]
public class MerchantController : MonoBehaviour
{
    [SerializeField] private List<MaskObject> _inventory = new();

    public List<MaskObject> Inventory => _inventory;
    private HoverSprite _hoverSprite;

    private void Awake()
    {
        _hoverSprite = GetComponent<HoverSprite>();

        if (_inventory.Count < 1)
        {
            Debug.LogWarning("WARNING : Merchant inventory is empty");
        }
    }

    public void SelectMerchant()
    {
        _hoverSprite.Appear(true);
    }

    public void DeselectMerchant()
    {
        _hoverSprite.Hide();
    }

    public void AddToInventory(MaskObject mask)
    {
        _inventory.Add(mask);
    }

    public void RemoveFromInventory(MaskObject mask)
    {
        _inventory.Remove(mask);
    }
}
