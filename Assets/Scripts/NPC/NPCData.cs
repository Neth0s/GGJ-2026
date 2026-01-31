using UnityEngine;

/// <summary>
/// Scriptable object of the data for the NPC
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NPC/NPC Data", fileName ="NPC Data")]
public class NPCData : ScriptableObject
{
    [SerializeField, Tooltip("True if this NPC is the one to find.")] public bool IsCulprit;
    [SerializeField, Tooltip("The feature this NPC's mask has (e.g., 'Red', 'Blue', 'Feathers').")] public string MaskFeature;
    [SerializeField, Tooltip("The clue this NPC gives about the culprit.")] public string ClueText;
}
