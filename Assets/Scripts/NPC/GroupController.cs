using NUnit.Framework;
using System.Collections.Generic;
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

    public List<MaskProperty> GetUpperMaskRequirements()
    {
        return _data.MaskRequirementsUpper;
    }

    public List<MaskProperty> GetLowerMaskRequirements()
    {
        return _data.MaskRequirementsLower;
    }
}
