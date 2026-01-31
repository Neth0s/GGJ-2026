using UnityEngine;

/// <summary>
/// Scriptable object of the data for the NPC
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NPC/NPC Data", fileName ="NPC Data")]
public class NPCData : ScriptableObject
{
    [SerializeField, Tooltip("Self-explanatory")] public bool IsBadGuy;
}
