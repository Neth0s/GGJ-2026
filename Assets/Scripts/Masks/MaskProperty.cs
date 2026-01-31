using UnityEngine;

public enum MASK_TYPE
{
    FAMILY,
    COLOR
}
/// <summary>
/// Mask property
/// </summary>
public abstract class MaskProperty : ScriptableObject
{
    public MASK_TYPE MaskType;
}
