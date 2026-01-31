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

    private void OnEnable()
    {
        moveAction = playerInputs.Player.Move;
        interactAction = playerInputs.Player.Interact;
        accuseAction = playerInputs.Player.Accuse;

        interactAction.performed += OnInteractPerformed;
        accuseAction.performed += OnAccusePerformed;

        playerInputs.Player.Enable();
    }

    private void OnDisable()
    {
        interactAction.performed -= OnInteractPerformed;
        accuseAction.performed -= OnAccusePerformed;
        playerInputs.Player.Disable();
    }

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
        detectController = GetComponent<DetectController>();
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        // Temporary mask cycle for testing/MVP
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            CycleMask();
        }
    }

    private void CycleMask()
    {
        _currentMaskIndex = (_currentMaskIndex + 1) % availableMasks.Length;
        Debug.Log($"Switched to mask: {CurrentMaskFeature}");
    }

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
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.MovePosition(rb.position
            + transform.forward * moveDirection.y * MoveSpeed * Time.deltaTime
            + transform.right * moveDirection.x * MoveSpeed * Time.deltaTime);
    }
}
