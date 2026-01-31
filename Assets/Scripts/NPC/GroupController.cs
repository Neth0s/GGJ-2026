using UnityEngine;

/// <summary>
/// Script of the Group
/// </summary>
public class GroupController : MonoBehaviour
{
    #region VARIABLES
    [Header("Group Data")]
    [SerializeField] private GroupData _data;
    #endregion

    public void TriggerGroupSelection()
    {
        print("Group "+gameObject.name+" is selected !");
        print("Requirements are as follow :");
        foreach(var req in _data.MaskRequirements)
        {
            print(req.name);
        }
    }
}
