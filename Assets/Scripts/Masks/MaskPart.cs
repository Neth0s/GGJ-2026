using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Masks/Mask Part", fileName ="Mask Part")]
public class MaskPart : ScriptableObject
{
    public List<MaskProperty> MaskProperties = new();
    public Sprite MaskSprite;
}
