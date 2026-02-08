using UnityEngine;
using UnityEngine.UI;

public class MaskUIElementController : MonoBehaviour
{
    [Header("Mask UI Elements")]
    [SerializeField] private Image _upperSprite;
    [SerializeField] private Image _lowerSprite;

    public void UpdateMaskVisual(MaskObject mask)
    {
        _upperSprite.sprite = mask.UpperPart().MaskSprite;
        _lowerSprite.sprite = mask.LowerPart().MaskSprite;
    }

    public void UpdateMaskVisual(MaskPart upperPart, MaskPart lowerPart)
    {
        _upperSprite.sprite = upperPart.MaskSprite;
        _lowerSprite.sprite = lowerPart.MaskSprite;
    }
}
