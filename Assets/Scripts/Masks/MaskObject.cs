using UnityEngine;

/// <summary>
/// Scriptable Object that will control the mask
/// </summary>
[CreateAssetMenu(menuName ="ScriptableObjects/Masks/Mask", fileName ="Mask")]
public class MaskObject : ScriptableObject
{
    [SerializeField] private MaskPart _upperPart;
    [SerializeField] private MaskPart _lowerPart;

    public MaskPart GetUpperPart() {  return _upperPart; }
    public MaskPart GetLowerPart() { return _lowerPart; }

    public void SetUpperPart(MaskPart upperPart) { _upperPart = upperPart; }
    public void SetLowerPart(MaskPart lowerPart) { _lowerPart = lowerPart; }
}
