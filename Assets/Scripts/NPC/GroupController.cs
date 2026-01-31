
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
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

    //dialog
    [SerializeField] private GameObject bubbleDialog;
    private int currentDialogIndex = 0;


    public void TriggerGroupSelection()
    {
        print("Group "+gameObject.name+" is selected !");    
    }

    private void Awake()
    {
        bubbleDialog.SetActive(false);
    }

    public bool DisplayBubble()
    {
        TextMeshPro textBubble = bubbleDialog.GetComponentInChildren<TextMeshPro>();
        if (currentDialogIndex < _data.indices.Count)
        {
            textBubble.text = _data.indices[currentDialogIndex];
            currentDialogIndex++;
            bubbleDialog.SetActive(true);
            return true;
        }
        else
        {
            bubbleDialog.SetActive(false);
            currentDialogIndex = 0;
            return false;
        }        
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
