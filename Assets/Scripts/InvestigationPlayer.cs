using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvestigationPlayer : MonoBehaviour
{

    public float MoveSpeed = 5.0f;

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction accuseAction;
    private InputAction chooseMaskAction;
    private InputAction seeIndiceAction;
    private InputAction escapeAction;
    private Rigidbody rb;
    private PlayerInputs playerInputs;
    private DetectController detectController;

    private Animator animator;
    private bool isWalking = false;
    private SpriteRenderer[] sprites;

    public List<string> indices = new List<string>();

    private Vector2 moveDirection;

    [Header("Interaction data")]
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GameObject _accusationInteractIcon;
    private bool isInDialog = false;
    private bool isChoosingMask = false;
    private bool isLookingForIndice = false;
    GroupController currentGroup; 

    private NPCController _currentNPC;
    private MerchantController _currentMerchant;

    private void OnEnable()
    {
        moveAction = playerInputs.Player.Move;
        interactAction = playerInputs.Player.Interact;
        accuseAction = playerInputs.Player.Accuse;
        chooseMaskAction = playerInputs.Player.ChooseMask;
        seeIndiceAction = playerInputs.Player.SeeIndice;
        escapeAction = playerInputs.Player.EscapeUI;

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

        animator = GetComponentInChildren<Animator>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        if (escapeAction.triggered)
        {
            if(isChoosingMask) DisplayChangeMaskUI();
            if(isLookingForIndice) DisplayIndicesUI();
        }

        if (seeIndiceAction.triggered && !isInDialog && !isChoosingMask)
        {
            DisplayIndicesUI();
        }

        if (chooseMaskAction.triggered && !isInDialog && !isLookingForIndice)
        {
            DisplayChangeMaskUI();
        }
        if (isChoosingMask || isLookingForIndice) return; //skip interaction

        if (interactAction.triggered && currentGroup) { InteractGroup(); }

        if (accuseAction.triggered && _currentNPC)  {  InteractAccusation(); }


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

    private void DisplayIndicesUI()
    {
        isLookingForIndice = !isLookingForIndice;
        UIManager.Instance.DisplayIndice(isLookingForIndice, indices);
    }

    private void FixedUpdate()
    {
        if (!isInDialog && !isChoosingMask && !isLookingForIndice) Move();
    }

    private void Move()
    {
        MoveAnimation();
        rb.MovePosition(rb.position
            + transform.forward * moveDirection.y * MoveSpeed * Time.deltaTime
            + transform.right * moveDirection.x * MoveSpeed * Time.deltaTime);       
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
        foreach(var sprite in sprites)
        {
            int rotation = moveDirection.x < 0 ? 0 : 180;
            sprite.transform.localRotation = Quaternion.Euler(0, rotation, 0);
        }
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
        _currentNPC = null;
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

    private void InteractGroup()
    {
        isInDialog = currentGroup.DisplayBubble();
        if (!indices.Contains(currentGroup.GetGroupIndice()))
        {
            indices.Add(currentGroup.GetGroupIndice());
        }
    }

    private void InteractAccusation()
    {
        isInDialog = currentGroup.DisplayBubbleAccusation(_currentNPC);
    }

    #region MERCHANT RELATED
    /// <summary>
    /// Will trigger the merchant's interaction
    /// </summary>
    private void InteractMerchant()
    {
        UIManager.Instance.DisplayMerchantUI(true);
        isInDialog = true;
        //isInDialog = _currentMerchant.StartMerchantInteraction(this);
    }

    /// <summary>
    /// Will deactivate the merchant's interaction
    /// </summary>
    public void DeactivateMerchantInteraction()
    {
        UIManager.Instance.DisplayMerchantUI(false);
        _currentMerchant = null;
        isInDialog = false;
    }
    #endregion
}
