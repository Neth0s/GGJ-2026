
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

    //dialog
    [SerializeField] private GameObject bubbleDialog;
    private TextMeshPro textBubble;
    private int currentDialogIndex = 0;

    private bool _isAccusing = false;

    private bool _hasWon = false;
    #endregion


    public void TriggerGroupSelection()
    {
        print("Group "+gameObject.name+" is selected !");    
    }

    private void Awake()
    {
        bubbleDialog.SetActive(false);
        textBubble = bubbleDialog.GetComponentInChildren<TextMeshPro>();
    }

    public bool DisplayBubble()
    {
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

    /// <summary>
    /// Will display the accusation bubble for a specific NPC
    /// </summary>
    /// <param name="selectedNPC">Currently selected NPC</param>
    /// <returns>Boolean (true if accusation is correct) </returns>
    public bool DisplayBubbleAccusation(NPCController selectedNPC)
    {
        if (_isAccusing)
        {
            _isAccusing = false;
            bubbleDialog.SetActive(false);
            if (_hasWon)
            {
                GameManager.Instance.WinGame();
            }
            return false;
        }
        else
        {
            if (selectedNPC.IsNPCBadGuy())
            {
                textBubble.text = GameParameters.ACCUSATIONS_CORRECT[Random.Range(0, GameParameters.ACCUSATIONS_CORRECT.Length)];
                _hasWon = true;
                print("------ I WON !!!! WOOP WOOP !!!! ------");
            }
            else 
            {
                textBubble.text = GameParameters.ACCUSATIONS_INCORRECT[Random.Range(0, GameParameters.ACCUSATIONS_INCORRECT.Length)];
                Timer.Instance.RemoveTime(60f);
            }
            _isAccusing = true;
            bubbleDialog.SetActive(true);
            return true;
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
