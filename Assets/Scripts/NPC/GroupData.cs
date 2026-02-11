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

    [Header("Clues")]
    [Tooltip("Clue given with base dialogue")] public string baseClue;
    [Tooltip("Clue given with hidden dialogue")] public string hiddenClue;

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
        List<PROPERTY_TYPE> properties = new();
        foreach(MaskProperty property in MaskRequirementsUpper)
        {
            if (property != null)
            {
                if (property.PropertyType == PROPERTY_TYPE.FAMILY &&
                    properties.Contains(PROPERTY_TYPE.FAMILY))
                {
                    Debug.LogWarning("Warning : Mask Requirements contains two family types, they will be impossible to meet");
                }
                properties.Add(property.PropertyType);
            }
        }

        properties = new List<PROPERTY_TYPE>();
        foreach (MaskProperty property in MaskRequirementsLower)
        {
            if (property != null)
            {
                if (property.PropertyType == PROPERTY_TYPE.FAMILY &&
                    properties.Contains(PROPERTY_TYPE.FAMILY))
                {
                    Debug.LogWarning("Warning : Mask Requirements contains two family types, they will be impossible to meet");
                }
                properties.Add(property.PropertyType);
            }
        }
    }
#endif
}
