using System.Collections.Generic;
using UnityEngine;
using Enums;

/// <summary>
/// Scriptable object of the data for the Group
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NPC/Group Data", fileName ="Group Data")]
public class GroupData : ScriptableObject
{
    [SerializeField] private Enums.MaskType commonMask;
    [SerializeField] private List<string> clues;

    public Enums.MaskType CommonMask => commonMask;
    public List<string> Clues => clues;

}
