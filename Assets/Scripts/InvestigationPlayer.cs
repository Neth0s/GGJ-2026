using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InvestigationPlayer : MonoBehaviour
{

    public float MoveSpeed = 5.0f;

    [Header("Mask Settings")]
    [SerializeField] private string[] availableMasks = { "No Mask", "Red Mask", "Blue Mask", "Green Mask" };
    private int _currentMaskIndex = 0;

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction accuseAction;
    private Rigidbody rb;
    private PlayerInputs playerInputs;
    private DetectController detectController;

    private Vector2 moveDirection;

    [Header("Interaction data")]
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GameObject _accusationInteractIcon;
    private bool isInDialog = false;
    GroupController currentGroup;

    private NPCController _currentNPC;

    private void OnEnable()
    {
        moveAction = playerInputs.Player.Move;
        interactAction = playerInputs.Player.Interact;
        accuseAction = playerInputs.Player.Accuse;
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
        if (interactAction.triggered && currentGroup) { Interact(); }

        if (accuseAction.triggered) 
        { 
            print("accuse");
            InteractAccusation(); 
        }
    }

    private void FixedUpdate()
    {
        if (!isInDialog) Move();
    }

    private void Move()
    {
        rb.MovePosition(rb.position
            + transform.forward * moveDirection.y * MoveSpeed * Time.deltaTime
            + transform.right * moveDirection.x * MoveSpeed * Time.deltaTime);
    }

    public void EnableInterraction(GroupController group)
    {
        interactIcon.SetActive(true);
        currentGroup = group;
    }

    public void EnableAccusationInteraction(NPCController npc)
    {
        _accusationInteractIcon.SetActive(true);
        _currentNPC = npc;
    }

    public void DisableInterraction()
    {
        interactIcon.SetActive(false);
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
        isInDialog = currentGroup.DisplayBubbleAccusation();
    }

}
