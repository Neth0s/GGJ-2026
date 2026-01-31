using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object of the data for the Group
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NPC/Group Data", fileName ="Group Data")]
public class GroupData : ScriptableObject
{
    //[SerializeField] public ??? Requirements; //Liste d'enums qui va dicter si le masque du joueur est valable ou non
    [SerializeField] private List<string> indices;

}
