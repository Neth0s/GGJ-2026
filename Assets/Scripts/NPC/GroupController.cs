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
        print("Mask upper :");
        foreach(var req in _data.MaskRequirementsUpper)
        {
            print(req.name);
        }
        print("Mask lower :");
        foreach (var req in _data.MaskRequirementsLower)
        {
            print(req.name);
        }
    }
}
