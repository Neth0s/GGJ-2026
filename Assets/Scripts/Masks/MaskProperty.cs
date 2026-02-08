using UnityEngine;

public enum PROPERTY_TYPE
{
    FAMILY,
    COLOR
}

public abstract class MaskProperty : ScriptableObject
{
    public PROPERTY_TYPE PropertyType;
}
