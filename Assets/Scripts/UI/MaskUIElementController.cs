using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script will govern the UI Mask display element
/// </summary>
public class MaskUIElementController : MonoBehaviour
{
    #region VARIABLES
    [Header("Mask UI Display Elements")]
    [SerializeField] private GameObject _upperPart;
    [SerializeField] private GameObject _lowerPart;

    private Image _upperSprite;
    private Image _lowerSprite;
    #endregion

    private void Awake()
    {
        if (_upperPart == null) Debug.LogError("ERROR : please assign upper part to MaskUIElementController.");
        if (_lowerPart == null) Debug.LogError("ERROR : please assign lower part to MaskUIElementController.");
        _upperSprite = _upperPart.GetComponent<Image>();    
        _lowerSprite = _lowerPart.GetComponent<Image>();
    }

    /// <summary>
    /// Will update a mask UI Display element
    /// </summary>
    /// <param name="mask">Inputted mask object</param>
    public void UpdateMaskVisual(MaskObject mask)
    {
        _upperSprite.sprite = mask.GetUpperPart().MaskSprite;
        _lowerSprite.sprite = mask.GetLowerPart().MaskSprite;
    }
    /// <summary>
    /// Will update a mask UI Display element
    /// </summary>
    /// <param name="upperPart">Inputted upper part</param>
    /// <param name="lowerPart">Inputted lower part</param>
    public void UpdateMaskVisual(MaskPart upperPart, MaskPart lowerPart)
    {
        _upperSprite.sprite = upperPart.MaskSprite;
        _lowerSprite.sprite = lowerPart.MaskSprite;
    }
}
