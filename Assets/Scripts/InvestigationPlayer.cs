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

    public string CurrentMaskFeature => availableMasks[_currentMaskIndex];

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction accuseAction;
    private Rigidbody rb;
    private PlayerInputs playerInputs;
    private DetectController detectController;

    private Vector2 moveDirection;

    [SerializeField] private GameObject interactIcon;
    private bool isInDialog = false;
    GroupController currentGroup; 

    private void OnEnable()
    {
        moveAction = playerInputs.Player.Move;
        interactAction = playerInputs.Player.Interact;
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
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            CycleMask();
        }
        if (interactAction.triggered && currentGroup) { Interact(); }
    }

    private void CycleMask()
    {
        _currentMaskIndex = (_currentMaskIndex + 1) % availableMasks.Length;
        Debug.Log($"Switched to mask: {CurrentMaskFeature}");
    }

    /*

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (detectController == null) return;

        NPCController npc = detectController.GetCurrentlySelectedNPC();
        if (npc != null)
        {
            Debug.Log($"Asking clue to {npc.name}: {npc.Data.ClueText}");
            DetectiveManager.Instance.AddClue(npc.Data.ClueText);
        }
    }

    private void OnAccusePerformed(InputAction.CallbackContext context)
    {
        if (detectController == null) return;

        NPCController npc = detectController.GetCurrentlySelectedNPC();
        if (npc != null)
        {
            Debug.Log($"Accusing {npc.name}!");
            DetectiveManager.Instance.AttemptAccusation(npc.Data.IsCulprit);
        }

        if (interactAction.triggered && currentGroup) { Interact(); }
    }
    */

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

    public void DisableInterraction()
    {
        interactIcon.SetActive(false);
    }

    private void Interact()
    {
        isInDialog = currentGroup.DisplayBubble();
    }

}
