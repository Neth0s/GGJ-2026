using UnityEngine;
using Enums;

/// <summary>
/// Scriptable object of the data for the NPC
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NPC/NPC Data", fileName ="NPC Data")]
public class NPCData : ScriptableObject
{
    [SerializeField] private MaskType mask;
    [SerializeField] private bool isCulprit;
    [SerializeField] private string specificClue;

    public MaskType Mask => mask;
    public bool IsCulprit => isCulprit;
    public string SpecificClue => specificClue;
}
