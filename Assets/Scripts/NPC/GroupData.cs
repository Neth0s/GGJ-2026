using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object of the data for the Group
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NPC/Group Data", fileName ="Group Data")]
public class GroupData : ScriptableObject
{
    [SerializeField] private List<string> indices = new List<string>();

    [SerializeField] public List<MaskProperty> MaskRequirements = new List<MaskProperty>();


    /// <summary>
    /// Serves only to ensure that mask requirements are correctly defined
    /// </summary>
    private void OnValidate()
    {
        List<MASK_TYPE> maskTypes = new List<MASK_TYPE>();
        foreach(MaskProperty property in MaskRequirements)
        {
            if (maskTypes.Contains(property.MaskType))
            {
                Debug.LogError("ERROR : Mask Requirements can only have one MaskProperty of each type ! Currently, there are two (or more) masks of type "+property.MaskType);
                break;
            }
            else
            {
                maskTypes.Add(property.MaskType);
            }
        }

    }
}
