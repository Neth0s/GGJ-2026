using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5.0f;

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

    private GroupController currentGroup;
    private NPCController currentNPC;
    private MerchantController currentMerchant;

    //Variables
    private bool isWalking = false;
    private bool isFacingRight = false;
    private bool isInDialog = false;
    private bool maskPanelOpen = false;
    private bool indicesPanelOpen = false;

    private Vector2 moveDirection;
    private Vector3 startPosition;
    private float spriteScale;

    private readonly List<string> indices = new();

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
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        if (escapeAction.triggered)
        {
            if(maskPanelOpen) DisplayChangeMaskUI();
            if(indicesPanelOpen) DisplayIndicesUI();
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

        if (accuseAction.triggered && currentNPC) Accusation();
    }

    private void FixedUpdate()
    {
        if (!isInDialog && !maskPanelOpen && !indicesPanelOpen) Move();
    }

    private void DisplayChangeMaskUI()
    {
        maskPanelOpen = !maskPanelOpen;
        UIManager.Instance.DisplayMaskPanel(maskPanelOpen);
    }

    private void DisplayIndicesUI()
    {
        indicesPanelOpen = !indicesPanelOpen;
        UIManager.Instance.DisplayIndices(indicesPanelOpen, indices);
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
        string indice = currentGroup.GetGroupIndice();

        if (!indices.Contains(indice) && !string.IsNullOrEmpty(indice))
        {
            indices.Add(indice);
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
    }

    #region MERCHANT RELATED
    private void InteractMerchant()
    {
        UIManager.Instance.DisplayMerchantUI(true);
        isInDialog = true;
    }

    public void LeaveMerchant()
    {
        UIManager.Instance.DisplayMerchantUI(false);
        isInDialog = false;
    }
    #endregion
}
