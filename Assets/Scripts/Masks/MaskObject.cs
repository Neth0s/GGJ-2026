using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Masks/Mask", fileName ="Mask")]
public class MaskObject : ScriptableObject
{
    [SerializeField] private MaskPart _upperPart;
    [SerializeField] private MaskPart _lowerPart;

    public MaskPart UpperPart() {  return _upperPart; }
    public MaskPart LowerPart() { return _lowerPart; }

    public void SetUpperPart(MaskPart upperPart) { _upperPart = upperPart; }
    public void SetLowerPart(MaskPart lowerPart) { _lowerPart = lowerPart; }
}
