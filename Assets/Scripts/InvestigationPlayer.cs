using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InvestigationPlayer : MonoBehaviour
{

    public float MoveSpeed = 5.0f;

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction accuseAction;
    private InputAction chooseMaskAction;
    private Rigidbody rb;
    private PlayerInputs playerInputs;
    private DetectController detectController;

    private Vector2 moveDirection;

    [Header("Interaction data")]
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GameObject _accusationInteractIcon;
    private bool isInDialog = false;
    private bool isChoosingMask = false;
    GroupController currentGroup; 

    private NPCController _currentNPC;
    private MerchantController _currentMerchant;

    private void OnEnable()
    {
        moveAction = playerInputs.Player.Move;
        interactAction = playerInputs.Player.Interact;
        accuseAction = playerInputs.Player.Accuse;
        chooseMaskAction = playerInputs.Player.ChooseMask;
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
        detectController = GetComponent<DetectController>();

        interactIcon.SetActive(false);
        _accusationInteractIcon.SetActive(false);
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        if (chooseMaskAction.triggered && !isInDialog)
        {
            DisplayChangeMaskUI();
        }
        if (isChoosingMask) return; //skip interaction

        if (interactAction.triggered && currentGroup) { Interact(); }

        if (accuseAction.triggered) 
        { 
            InteractAccusation(); 
        }

        if(interactAction.triggered && _currentMerchant)
        {
            if (!isInDialog)
            {
                InteractMerchant();
            }
        }

        
    }

    private void DisplayChangeMaskUI()
    {
        isChoosingMask = !isChoosingMask;
        UIManager.Instance.DisplayChooseMask(isChoosingMask);

    }

    private void FixedUpdate()
    {
        if (!isInDialog && !isChoosingMask) Move();
    }

    private void Move()
    {
        rb.MovePosition(rb.position
            + transform.forward * moveDirection.y * MoveSpeed * Time.deltaTime
            + transform.right * moveDirection.x * MoveSpeed * Time.deltaTime);
    }

    public void EnableInteraction(GroupController group)
    {
        interactIcon.SetActive(true);
        currentGroup = group;
    }

    public void EnableInteraction(NPCController npc)
    {
        _accusationInteractIcon.SetActive(true);
        _currentNPC = npc;
    }
    public void EnableInteraction(MerchantController merchant)
    {
        interactIcon.SetActive(true);
        _currentMerchant = merchant;
    }

    public void DisableInteraction()
    {
        interactIcon.SetActive(false);
        currentGroup = null;
    }
    public void DisableMerchantInteraction()
    {
        interactIcon.SetActive(false);
        _currentMerchant= null;
    }

    public void DisableAccusationInteraction()
    {
        _accusationInteractIcon.SetActive(false);
    }

    private void Interact()
    {
        isInDialog = currentGroup.DisplayBubble();
    }

    private void InteractAccusation()
    {
        isInDialog = currentGroup.DisplayBubbleAccusation(_currentNPC);
    }
    private void InteractMerchant()
    {
        UIManager.Instance.DisplayMerchantUI(true);
        isInDialog = true;
        //isInDialog = _currentMerchant.StartMerchantInteraction(this);
    }

}
