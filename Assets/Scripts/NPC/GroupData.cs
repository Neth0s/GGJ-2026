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
}
