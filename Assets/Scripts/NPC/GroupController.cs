using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(HoverSprite))]
public class GroupController : MonoBehaviour
{
    #region VARIABLES
    [Header("Group Data")]
    [SerializeField] private GroupData _data;
    [SerializeField] private GameObject bubbleDialog;

    private HoverSprite _hoverSprite;
    private TextMeshPro textBubble;
    private int currentDialogIndex = 0;

    private bool _isAccusing = false;
    private bool _hasWon = false;

    #endregion

    public void SelectGroup(bool maskOk)
    {
        _hoverSprite.Appear(maskOk);
    }

    public void DeselectGroup()
    {
        _hoverSprite.Hide();
    }

    private void Awake()
    {
        bubbleDialog.SetActive(false);
        textBubble = bubbleDialog.GetComponentInChildren<TextMeshPro>();
        _hoverSprite = GetComponent<HoverSprite>();
    }

    public bool DisplayBubble()
    {
        bubbleDialog.SetActive(true);

        //Text between < and > corresponds to clues that will be displayed in red
        if (currentDialogIndex < _data.dialogs.Count)
        {           
            textBubble.text = _data.dialogs[currentDialogIndex]
                .Replace("<", "<color=red*")
                .Replace(">", @"</color*")
                .Replace("*", ">");
            currentDialogIndex++;
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

            if (_hasWon) GameManager.Instance.WinGame();
            return false;
        }
        else
        {
            MusicManager.Instance.PlayAccuseAndLoop();
            if (selectedNPC.IsNPCBadGuy())
            {
                textBubble.text = GameParameters.ACCUSATIONS_CORRECT[Random.Range(0, GameParameters.ACCUSATIONS_CORRECT.Length)];
                _hasWon = true;
            }
            else 
            {
                textBubble.text = GameParameters.ACCUSATIONS_INCORRECT[Random.Range(0, GameParameters.ACCUSATIONS_INCORRECT.Length)];
                Timer.Instance.RemoveTime(GameParameters.DEFAULT_TIME_FAIL_ACCUSATION);
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

    public string GetGroupIndice()
    {
        return _data.indice;
    }

    public void ForceStopDialog()
    {
        currentDialogIndex = 0;
        bubbleDialog.SetActive(false);
    }
}
