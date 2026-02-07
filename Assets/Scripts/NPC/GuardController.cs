using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class GuardController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float pauseTime;
    [SerializeField] private Vector3[] waypoints;

    [Header("Detection")]
    [Tooltip("Time (in seconds) the player must remain in detection radius to be caught")]
    [SerializeField] private float detectionTime = 3f;
    [Tooltip("Penalty time (in seconds) subtracted from Timer when player is caught")]
    [SerializeField] private float penaltySeconds = 20f;
    [Tooltip("Cooldown time (in seconds) after catching player")]
    [SerializeField] private float respawnCooldown = 0.5f;   // prevents immediate re-trigger

    [Header("Display")]
    [SerializeField] private GameObject guardSprite;
    [SerializeField] private Image iconLight;
    [SerializeField] private Image iconDark;

    //Components
    private Rigidbody rb;
    private Animator animator;
    private GameObject player;
    private DetectController detectController;

    //Variables
    private int currentWaypoint = 0;
    private float pauseTimer = 0f;
    private float detectionTimer = 0f;
    private float spriteScale;

    private bool isWalking = true;
    private bool immobileGuard = true;

    private bool isDetectingPlayer = false;
    private bool soundEffectTriggered = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        spriteScale = guardSprite.transform.localScale.x;
    }

    private void Start()
    {
        if (waypoints != null && waypoints.Length > 1)
        {
            transform.position = waypoints[0];
            currentWaypoint = 1;
            immobileGuard = false;
        }
        
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            detectController = player.GetComponent<DetectController>();
        }
        else Debug.LogError("Aucun objet avec le tag 'Player' trouvé!");

        iconLight.gameObject.SetActive(false);
        iconDark.fillAmount = 0f;

        animator.SetBool("WalkBool", !immobileGuard);
    }

    void Update()
    {
        if (!immobileGuard)
        {
            if (isWalking && !isDetectingPlayer)
            {
                Vector3 target = waypoints[currentWaypoint];
                Vector3 direction = (target - transform.position).normalized;

                rb.MovePosition(rb.position + speed * Time.deltaTime * direction);

                // Flip guard to face movement direction
                if (direction.x > 0 || direction.z > 0)
                {
                    guardSprite.transform.localScale = new Vector3(-spriteScale, spriteScale, spriteScale);
                }
                else
                {
                    guardSprite.transform.localScale = spriteScale * Vector3.one;

                }

                //Snap to waypoint and pause
                if (Vector3.Distance(transform.position, target) < 0.05f)
                {
                    transform.position = target;
                    isWalking = false;
                    pauseTimer = 0f;
                    animator.SetBool("WalkBool", false);
                }
            }
            else if (!isWalking && !isDetectingPlayer)
            {
                pauseTimer += Time.deltaTime;
                if (pauseTimer >= pauseTime)
                {
                    currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                    isWalking = true;
                    animator.SetBool("WalkBool", true);
                }
            }
        }

        if (isDetectingPlayer)
        {
            detectionTimer += Time.deltaTime;

            if (!soundEffectTriggered)
            {
                animator.SetBool("WalkBool", false);
                MusicManager.Instance.PlayGuardSuspiciousSound();
                soundEffectTriggered = true;
            }

            if (detectionTimer >= detectionTime)
            {
                StartCoroutine(HandleCaught());
            }   
        }
        else if (detectionTimer > 0)
        {
            detectionTimer -= Time.deltaTime;
            soundEffectTriggered = false;
        }

        UpdateDetectionIcons();
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CheckDetection();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isDetectingPlayer = false;
            animator.SetBool("WalkBool", !immobileGuard);
        }
    }

    void CheckDetection()
    {
        bool previousDetectingState = isDetectingPlayer;

        if (detectController.MasksAreOK)
        {
            isDetectingPlayer = false;
        }
        else
        {
            GroupController currentGroup = detectController.GetCurrentGroup();
            isDetectingPlayer = (currentGroup != null);
        }

        if (isDetectingPlayer != previousDetectingState)
        {
            animator.SetBool("WalkBool", !isDetectingPlayer && !immobileGuard);
        }
    }
    
    IEnumerator HandleCaught()
    {
        detectionTimer = 0f;
        MusicManager.Instance.PlayGuardCatchSound();

        if (Timer.Instance != null)
        {
            player.GetComponent<Player>().ResetPlayer();
            Timer.Instance.RemoveTime(penaltySeconds);
        }
        else
        {
            Debug.LogError("Timer not found");
        }

        yield return new WaitForSeconds(respawnCooldown);
    }

    void UpdateDetectionIcons()
    {
        bool visible = detectionTimer > 0f;
        iconLight.gameObject.SetActive(visible);
        iconDark.fillAmount = Mathf.Clamp01(detectionTimer / detectionTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (waypoints != null && waypoints.Length > 1)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                Vector3 current = waypoints[i];
                Vector3 next = waypoints[(i + 1) % waypoints.Length];
                Gizmos.DrawLine(current, next);
            }
        }
    }
}
