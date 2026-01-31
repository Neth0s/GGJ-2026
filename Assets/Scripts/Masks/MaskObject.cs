using UnityEngine;

/// <summary>
/// Scriptable Object that will control the mask
/// </summary>
[CreateAssetMenu(menuName ="ScriptableObjects/Masks/Mask", fileName ="Mask")]
public class MaskObject : ScriptableObject
{
    [SerializeField] private MaskPart _superiorPart;
    [SerializeField] private MaskPart _inferiorPart;

    public MaskPart GetSuperiorPart() {  return _superiorPart; }
    public MaskPart GetInferiorPart() { return _inferiorPart; }
}
