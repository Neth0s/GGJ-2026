using UnityEngine;

/// <summary>
/// Script of the Group
/// </summary>
public class GroupController : MonoBehaviour
{
    #region VARIABLES
    [Header("Group Data")]
    [SerializeField] private GroupData _data;
    public GroupData Data => _data;
    #endregion

    public void TriggerGroupSelection()
    {
        print("Group "+gameObject.name+" is selected !");
    }
}
