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

    [Header("Rewards")]
    [Tooltip("Events triggered when interacting with group")]
    [SerializeField] private UnityEngine.Events.UnityEvent baseRewards;
    [Tooltip("Events triggered when interacting with group with correct mask")]
    [SerializeField] private UnityEngine.Events.UnityEvent hiddenRewards;

    private HoverSprite _hoverSprite;
    private TextMeshPro textBubble;
    private int curDialogueIndex = 0;

    private bool _hasRequirements;
    private bool _maskOk = false;
    private bool _isAccusing = false;
    private bool _hasWon = false;
    private bool baseRewardsGiven = false;
    private bool hiddenRewardsGiven = false;
    #endregion

    public bool HasRequirements => _hasRequirements;
    public List<MaskProperty> MaskReqsUp => _data.MaskRequirementsUpper;
    public List<MaskProperty> MaskReqsLow => _data.MaskRequirementsLower;

    private void Awake()
    {
        bubbleDialog.SetActive(false);
        textBubble = bubbleDialog.GetComponentInChildren<TextMeshPro>();
        _hoverSprite = GetComponent<HoverSprite>();

        _hasRequirements = _data.MaskRequirementsLower.Count + _data.MaskRequirementsUpper.Count > 0;
    }

    public void SetData(GroupData data)
    {
        _data = data;
        _hasRequirements = _data.MaskRequirementsLower.Count + _data.MaskRequirementsUpper.Count > 0;
    }

    public void Select(bool maskOk)
    {
        //Hidden dialogue and rewards are not used if there are no requirements
        _maskOk = maskOk && _hasRequirements;
        _hoverSprite.Appear(maskOk);
    }

    public void Deselect()
    {
        _hoverSprite.Hide();
    }

    public bool DisplayBubble()
    {
        bubbleDialog.SetActive(true);
        int dialogueLength = _maskOk ? _data.hiddenDialogue.Count : 
                                       _data.baseDialogue.Count;

        //Text between < and > corresponds to clues that will be displayed in red
        if (curDialogueIndex < dialogueLength)
        {
            string dialogue = _maskOk ? _data.hiddenDialogue[curDialogueIndex] : 
                                        _data.baseDialogue[curDialogueIndex];

            textBubble.text = dialogue.Replace("<", "<color=red*")
                                      .Replace(">", @"</color*")
                                      .Replace("*", ">");
            curDialogueIndex++;
            return true;
        }
        else
        {
            bubbleDialog.SetActive(false);
            curDialogueIndex = 0;
            return false;
        }       
    }

    /// <summary>
    /// Displays the accusation bubble for a specific NPC
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
                Timer.Instance.RemoveTime(GameParameters.FAIL_ACCUSATION_PENALTY);
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

    public string GetGroupClue()
    {
        return _maskOk ? _data.hiddenClue : _data.baseClue;
    }

    public void GetRewards()
    {
        if (_maskOk && !hiddenRewardsGiven)
        {
            hiddenRewards.Invoke();
            hiddenRewardsGiven = true;
        }
        if (!_maskOk && !baseRewardsGiven)
        {
            baseRewards.Invoke();
            baseRewardsGiven = true;
        }
    }

    public void ForceStopDialog()
    {
        curDialogueIndex = 0;
        bubbleDialog.SetActive(false);
    }
}
