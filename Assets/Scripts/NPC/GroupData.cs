using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object of the data for the Group
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NPC/Group Data", fileName ="Group Data")]
public class GroupData : ScriptableObject
{
    [Header("Dialogues")]
    [Tooltip("Default dialogue when interacting with the group")]
    public List<string> baseDialogue = new();
    [Tooltip("Dialogue accessible only when player fulfills the mask requirements of the group")]
    public List<string> hiddenDialogue = new();

    [Header("Rewards")]
    [Tooltip("Clue given with base dialogue")] public string baseClue;
    [Tooltip("Clue given with hidden dialogue")] public string hiddenClue;
    [Space(10)]
    [Tooltip("Mask given with base dialogue")] public MaskObject baseMask;
    [Tooltip("Mask given with hidden dialogue")] public MaskObject hiddenMask;

    [Header("Requirements")]
    public List<MaskProperty> MaskRequirementsUpper = new();
    public List<MaskProperty> MaskRequirementsLower = new();

#if UNITY_EDITOR
    /// <summary>
    /// Serves only to ensure that mask requirements are correctly defined
    /// SHOULD BE REMOVED eventually
    /// </summary>
    private void OnValidate()
    {
        List<PROPERTY_TYPE> maskTypes = new();
        foreach(MaskProperty property in MaskRequirementsUpper)
        {
            if(property != null)
            {
                if (maskTypes.Contains(property.PropertyType))
                {
                    Debug.LogError("ERROR : Mask Requirements Upper can only have one MaskProperty of each type ! Currently, there are two (or more) masks of type "+property.PropertyType);
                    break;
                }
                else
                {
                    maskTypes.Add(property.PropertyType);
                }
            }
        }

        maskTypes = new List<PROPERTY_TYPE>();
        foreach (MaskProperty property in MaskRequirementsLower)
        {
            if (property != null)
            {
                if (maskTypes.Contains(property.PropertyType))
                {
                    Debug.LogError("ERROR : Mask Requirements Lower can only have one MaskProperty of each type ! Currently, there are two (or more) masks of type " + property.PropertyType);
                    break;
                }
                else
                {
                    maskTypes.Add(property.PropertyType);
                }
            }
        }
    }
#endif
}
