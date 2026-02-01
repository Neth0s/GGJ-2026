using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object of the data for the Group
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NPC/Group Data", fileName ="Group Data")]
public class GroupData : ScriptableObject
{

    [SerializeField] public List<string> dialogs = new List<string>();
    [SerializeField] public string indice;

    [SerializeField] public List<MaskProperty> MaskRequirementsUpper = new List<MaskProperty>();
    [SerializeField] public List<MaskProperty> MaskRequirementsLower = new List<MaskProperty>();

#if UNITY_EDITOR
    /// <summary>
    /// Serves only to ensure that mask requirements are correctly defined
    /// SHOULD BE REMOVED eventually
    /// </summary>
    private void OnValidate()
    {
        List<MASK_TYPE> maskTypes = new List<MASK_TYPE>();
        foreach(MaskProperty property in MaskRequirementsUpper)
        {
            if(property != null)
            {
                if (maskTypes.Contains(property.MaskType))
                {
                    Debug.LogError("ERROR : Mask Requirements Upper can only have one MaskProperty of each type ! Currently, there are two (or more) masks of type "+property.MaskType);
                    break;
                }
                else
                {
                    maskTypes.Add(property.MaskType);
                }
            }
        }
        maskTypes = new List<MASK_TYPE>();
        foreach (MaskProperty property in MaskRequirementsLower)
        {
            if (property != null)
            {
                if (maskTypes.Contains(property.MaskType))
                {
                    Debug.LogError("ERROR : Mask Requirements Lower can only have one MaskProperty of each type ! Currently, there are two (or more) masks of type " + property.MaskType);
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
