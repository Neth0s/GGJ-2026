using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Masks/Mask Part", fileName ="Mask Part")]
public class MaskPart : ScriptableObject
{
    [SerializeField] public List<MaskProperty> MaskProperties = new List<MaskProperty>();
    [SerializeField] public Sprite MaskSprite;


#if UNITY_EDITOR
    private void OnValidate()
    {
        List<MASK_TYPE> maskTypes = new List<MASK_TYPE>();
        foreach (MaskProperty property in MaskProperties)
        {
            if(property != null)
            {
                if (maskTypes.Contains(property.MaskType))
                {
                    Debug.LogError("ERROR : Mask Requirements Upper can only have one MaskProperty of each type ! Currently, there are two (or more) masks of type " + property.MaskType);
                    break;
                }
                else
                {
                    maskTypes.Add(property.MaskType);
                }
            }
        }
    }
#endif
}
