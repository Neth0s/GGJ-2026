using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public float MoveSpeed = 5.0f;

    private InputAction moveAction; 
    private Rigidbody rb;
    private PlayerInputs playerInputs;

    private Vector2 moveDirection;

    private void OnEnable()
    {
        moveAction = playerInputs.Player.Move;
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
    }

    private void Update()
    {
       moveDirection = moveAction.ReadValue<Vector2>();

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
