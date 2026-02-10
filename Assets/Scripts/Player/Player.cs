using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5.0f;
    [SerializeField] private bool canMove;
    [SerializeField] private bool canAccuse;

    [Header("Interaction data")]
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GameObject acusationIcon;

    //Input actions
    private PlayerInputs playerInputs;
    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction accuseAction;
    private InputAction chooseMaskAction;
    private InputAction indiceAction;
    private InputAction escapeAction;
    
    //Components
    private Rigidbody rb;
    private SpriteRenderer sprite;
    private Animator animator;
    private MaskController maskController;

    private GroupController currentGroup;
    private NPCController currentNPC;
    private MerchantController currentMerchant;

    //Variables
    private bool isWalking = false;
    private bool isFacingRight = false;
    private bool isInDialog = false;
    private bool maskPanelOpen = false;
    private bool indicesPanelOpen = false;
    private bool pauseMenuOpen = false;

    private Vector2 moveDirection;
    private Vector3 startPosition;
    private float spriteScale;

    private readonly List<string> clues = new();
    public bool CanAccuse => canAccuse;

    private void OnEnable()
    {
        moveAction = playerInputs.Player.Move;
        interactAction = playerInputs.Player.Interact;
        accuseAction = playerInputs.Player.Accuse;
        chooseMaskAction = playerInputs.Player.Mask;
        indiceAction = playerInputs.Player.Clues;
        escapeAction = playerInputs.Player.Escape;

        playerInputs.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
        startPosition = transform.localPosition;

        interactIcon.SetActive(false);
        acusationIcon.SetActive(false);

        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentsInChildren<SpriteRenderer>()[0];
        spriteScale = sprite.transform.localScale.x;

        maskController = GetComponent<MaskController>();
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        if (escapeAction.triggered)
        {
            if (maskPanelOpen) DisplayChangeMaskUI();
            else if (indicesPanelOpen) DisplayIndicesUI();
            else TogglePauseMenu();
        }

        if (indiceAction.triggered && !isInDialog && !maskPanelOpen)
        {
            DisplayIndicesUI();
        }

        if (chooseMaskAction.triggered && !isInDialog && !indicesPanelOpen)
        {
            DisplayChangeMaskUI();
        }

        //Prevent interactions if panels are open
        if (maskPanelOpen || indicesPanelOpen) return;

        if (interactAction.triggered)
        {
            if (currentGroup) InteractGroup();
            if (currentMerchant && !isInDialog) InteractMerchant();
        }

        if (canAccuse && accuseAction.triggered && currentNPC) Accusation();
    }

    private void FixedUpdate()
    {
        if (canMove && !isInDialog && !maskPanelOpen && !indicesPanelOpen) Move();
    }

    private void DisplayChangeMaskUI()
    {
        maskPanelOpen = !maskPanelOpen;

        if (maskPanelOpen) UIManager.Instance.OpenMaskPanel(maskController);
        else UIManager.Instance.CloseMaskPanel();
    }

    private void DisplayIndicesUI()
    {
        indicesPanelOpen = !indicesPanelOpen;
        UIManager.Instance.DisplayIndices(indicesPanelOpen, clues);
    }

    private void TogglePauseMenu()
    {
        pauseMenuOpen = !pauseMenuOpen;
        UIManager.Instance.DisplayPauseMenu(pauseMenuOpen);
    }

    private void Move()
    {
        MoveAnimation();
        rb.MovePosition(rb.position
            + moveDirection.y * MoveSpeed * Time.deltaTime * transform.forward
            + moveDirection.x * MoveSpeed * Time.deltaTime * transform.right);       
    }

    private void MoveAnimation()
    {
        if (isWalking && moveDirection.magnitude == 0)
        {
            isWalking = false;
            animator.SetBool("WalkBool", false);
        }
        else if (!isWalking && moveDirection.magnitude > 0)
        {
            isWalking = true;
            animator.SetBool("WalkBool", true);           
        }

        if(isFacingRight && moveDirection.x < 0)
        {
            isFacingRight = false;
            sprite.transform.localScale = spriteScale * Vector3.one;

        }
        if (!isFacingRight && moveDirection.x > 0)
        {
            isFacingRight = true;
            sprite.transform.localScale = new Vector3(-spriteScale, spriteScale, spriteScale);
        }
    }

    public void EnableInteraction(GroupController group)
    {
        interactIcon.SetActive(true);
        currentGroup = group;
    }

    public void EnableAccusation(NPCController npc)
    {
        acusationIcon.SetActive(true);
        currentNPC = npc;
    }

    public void EnableInteraction(MerchantController merchant)
    {
        interactIcon.SetActive(true);
        currentMerchant = merchant;
        currentMerchant.SelectMerchant();
    }

    public void DisableInteraction()
    {
        interactIcon.SetActive(false);
        currentGroup = null;
        currentNPC = null;
    }

    public void DisableMerchantInteraction()
    {
        interactIcon.SetActive(false);
        currentMerchant.DeselectMerchant();
        currentMerchant = null;
    }

    public void DisableAccusation()
    {
        acusationIcon.SetActive(false);
    }

    private void InteractGroup()
    {
        isInDialog = currentGroup.DisplayBubble();

        if (!isInDialog)
        {   
            string clue = currentGroup.GetGroupClue();
            if (!clues.Contains(clue) && !string.IsNullOrEmpty(clue))
            {
                clues.Add(clue);
            }

            MaskObject mask = currentGroup.GetGroupMask();
            if (mask != null && !maskController.HasMask(mask))
            {
                maskController.AddMaskToInventory(mask);
            }
        }
    }

    private void Accusation()
    {
        isInDialog = currentGroup.DisplayBubbleAccusation(currentNPC);
    }

    public void ResetPlayer()
    {
        isInDialog = false;
        indicesPanelOpen = false;
        maskPanelOpen = false;

        if (currentGroup) currentGroup.ForceStopDialog();
        UIManager.Instance.FadeOut(transform, startPosition);
        StartCoroutine(LockPlayer());
    }

    private IEnumerator LockPlayer()
    {
        canMove = false;
        yield return new WaitForSeconds(UIManager.Instance.PlayerResetDuration);
        canMove = true;
    }

    #region MERCHANT RELATED
    private void InteractMerchant()
    {
        isInDialog = true;
        UIManager.Instance.OpenMerchantUI(maskController, currentMerchant);
    }

    public void LeaveMerchant()
    {
        isInDialog = false;
    }
    #endregion
}
