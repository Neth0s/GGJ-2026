using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Enums;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    [SerializeField] private MaskType currentMask = MaskType.None;

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction accuseAction;

    private Rigidbody rb;
    private PlayerInputs playerInputs;
    private Vector2 moveDirection;

    private GroupController currentGroup;

    public MaskType CurrentMask => currentMask;
    public GroupController CurrentGroup => currentGroup;

    private void OnEnable()
    {
        if (playerInputs == null) playerInputs = new PlayerInputs();

        moveAction = playerInputs.Player.Move;
        interactAction = playerInputs.Player.Interact;
        accuseAction = playerInputs.Player.Accuse;

        playerInputs.Player.Enable();

        interactAction.performed += OnInteract;
        accuseAction.performed += OnAccuse;
    }

    private void OnDisable()
    {
        if (playerInputs != null)
        {
            playerInputs.Player.Disable();
            interactAction.performed -= OnInteract;
            accuseAction.performed -= OnAccuse;
        }
    }

    private void Awake()
    {
        if (playerInputs == null) playerInputs = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        // Mask Switching (MVP Keyboard)
        if (Keyboard.current.digit1Key.wasPressedThisFrame) ChangeMask(MaskType.Triangle);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) ChangeMask(MaskType.Circle);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) ChangeMask(MaskType.Square);
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

    public void SetCurrentGroup(GroupController group)
    {
        currentGroup = group;
        Debug.Log(group != null ? $"Entered Group: {group.name}" : "Left Group");
        // Also notify Guard if needed? Or Guard checks Player.CurrentGroup
    }

    private void ChangeMask(MaskType type)
    {
        currentMask = type;
        Debug.Log($"Changed Mask to: {type}");
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hit in hits)
        {
            NPCController npc = hit.GetComponent<NPCController>();
            if (npc != null)
            {
                string clue = npc.GiveClue();
                if (GameManager.Instance != null)
                    GameManager.Instance.AddClue(clue);
                return;
            }
        }
    }

    private void OnAccuse(InputAction.CallbackContext context)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hit in hits)
        {
            NPCController npc = hit.GetComponent<NPCController>();
            if (npc != null)
            {
                if (npc.CheckAccusation())
                {
                    if (GameManager.Instance != null) GameManager.Instance.WinGame();
                }
                else
                {
                    if (GameManager.Instance != null) GameManager.Instance.LooseGame();
                }
                return;
            }
        }
    }
}
