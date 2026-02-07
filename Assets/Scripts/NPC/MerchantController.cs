using UnityEngine;

[RequireComponent(typeof(HoverSprite))]
public class MerchantController : MonoBehaviour
{
    private HoverSprite _hoverSprite;

    private void Awake()
    {
        _hoverSprite = GetComponent<HoverSprite>();
    }

    public void SelectMerchant()
    {
        _hoverSprite.Appear(true);
    }

    public void DeselectMerchant()
    {
        _hoverSprite.Hide();
    }
}
