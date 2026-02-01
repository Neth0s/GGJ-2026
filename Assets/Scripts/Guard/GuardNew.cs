using System.Collections;
using UnityEngine;

public class GuardNew : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private Vector3[] waypoints;
    [SerializeField] private float minPauseTime, maxPauseTime;
    
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 5f;
    
    [Header("Gauge")]
    [SerializeField] private float gaugeIncreaseSpeed = 100f;
    [SerializeField] private float maxGauge = 100f;

    [Header("Penalty")]
    [SerializeField] private float penaltySeconds = 20f;     // seconds subtracted from Timer when player is caught
    [SerializeField] private float respawnCooldown = 0.5f;   // prevents immediate re-trigger

    
    [Header("Gauge Visual (crossfade)")]
    [Tooltip("SpriteRenderer showing the LIGHT (empty) image")]
    [SerializeField] private SpriteRenderer gaugeLightSprite;
    [Tooltip("SpriteRenderer showing the DARK (filled) image — will fade in as gauge fills")]
    [SerializeField] private SpriteRenderer gaugeDarkSprite;
    [SerializeField] private Vector3 gaugeOffset = new Vector3(0, 2, 0); // Position au-dessus du garde
    [SerializeField] private float fadeSmoothing = 0.15f; // how quickly alpha lerps to the target (lower = snappier)

    private int currentWaypoint = 0;
    private bool isWalking = true;
    private float pauseTimer = 0f;
    private float pauseDuration = 0f;

    private float currentGauge = 0f;
    private bool isDetectingPlayer = false;
    private bool gaugeTriggered = false;
    
    private Transform playerTransform;
    private DetectController detectController;
    private Vector3 playerStartPosition;
    
    private void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("GuardNew: waypoints must be set and contain at least one position.");
            enabled = false;
            return;
        }   
        transform.position = waypoints[0];
        currentWaypoint = 1;
        
        // Trouve automatiquement le joueur par son tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            playerTransform = player.transform;
            detectController = player.GetComponent<DetectController>();
            // Capture la position initiale du joueur (fallback si GameManager n'a pas d'info)
            playerStartPosition = playerTransform.position;
        }
        else
        {
            Debug.LogError("Aucun objet avec le tag 'Player' trouvé!");
        }

        // Initialize sprite alpha/visibility if assigned
        if (gaugeLightSprite != null)
        {
            SetSpriteAlpha(gaugeLightSprite, 0f);
            gaugeLightSprite.enabled = false;
        }
        if (gaugeDarkSprite != null)
        {
            SetSpriteAlpha(gaugeDarkSprite, 0f);
            gaugeDarkSprite.enabled = false;
        }
    }

    void Update()
    {
        CheckDetection();

        if (isWalking && !isDetectingPlayer)
        {
            Vector3 target = waypoints[currentWaypoint];
            Vector3 direction = (target - transform.position).normalized;

            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

            if (Mathf.Abs(direction.x) > 0.5f)
            {
                float currentFacing = transform.localScale.x;
                if ((direction.x > 0 && currentFacing < 0) || (direction.x < 0 && currentFacing > 0))
                {
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                }
            }

            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                transform.position = target;
                isWalking = false;
                pauseDuration = Random.Range(minPauseTime, maxPauseTime);
                pauseTimer = 0f;
            }
        }
        else if (!isWalking && !isDetectingPlayer)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                isWalking = true;
            }
        }

        if (isDetectingPlayer)
        {
            print("player suspicious");
            // accumulate gauge while detecting
            currentGauge += gaugeIncreaseSpeed * Time.deltaTime;
            currentGauge = Mathf.Min(currentGauge, maxGauge);

            // trigger once when reaching max
            if (currentGauge >= maxGauge && !gaugeTriggered)
            {
                StartCoroutine(HandleCaught());
            }
        }
        else
        {
            // decay gauge when not detecting
            currentGauge = Mathf.Max(0, currentGauge - gaugeIncreaseSpeed * 0.3f * Time.deltaTime);
        }
        UpdateGaugeVisual();
    }

    void CheckDetection()
    {
        if (playerTransform == null || detectController == null)
        {
            isDetectingPlayer = false;
            return;
        }

        // Calcule la distance entre le garde et le joueur
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Si les masques sont OK, le garde ne détecte pas le joueur
            if (detectController.masksAreOK)
            {
                isDetectingPlayer = false;
            }
            else
            {
                // Les masques ne sont pas OK
                // Le joueur n'est détecté QUE s'il est dans un groupe (GroupController actif)
                GroupController currentGroup = detectController.GetCurrentlySelectedGroup();
                
                isDetectingPlayer = (currentGroup != null);
                
            }
        }
        else
        {
            isDetectingPlayer = false;
        }
    }

    void GetsCaught()
    {
        if (!gaugeTriggered)
        {
            StartCoroutine(HandleCaught());
        }
    }
    
    IEnumerator HandleCaught()
    {
        gaugeTriggered = true;
        Debug.Log("Joueur détecté par le garde!");
        
        // Teleport player back to their initial position (playerStartPosition captured at Start)
        // Prefer centralized GameManager method if available (keeps logic single place).
        if (GameManager.Instance != null)
        {
            TeleportPlayerToStart();
            if (Timer.Instance != null)
                Timer.Instance.RemoveTime(penaltySeconds);
        }
        else
        {
            // no GameManager -> handle locally and try Timer singleton
            TeleportPlayerToStart();
            if (Timer.Instance != null)
                Timer.Instance.RemoveTime(penaltySeconds);
            else
                Debug.LogWarning("GuardNew: No GameManager and no Timer.Instance found. Penalty not applied.");
        }
        // Reset guard's gauge (customize if you want a partial drain instead)
        currentGauge = 0f;

        // short cooldown to avoid immediate retriggering
        yield return new WaitForSeconds(respawnCooldown);

        gaugeTriggered = false;
    }

    private void TeleportPlayerToStart()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("GuardNew: No playerTransform to teleport.");
            return;
        }

        // Teleport handling for CharacterController and Rigidbody
        var cc = playerTransform.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            playerTransform.position = playerStartPosition;
            cc.enabled = true;
        }
        else
        {
            playerTransform.position = playerStartPosition;
        }

        var playerRb = playerTransform.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isDetectingPlayer ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public float GetGaugePercentage()
    {
        return Mathf.Clamp01(currentGauge / maxGauge);
    }

    void UpdateGaugeVisual()
    {
        // If the new crossfade sprites are assigned, use them
        if (gaugeLightSprite == null || gaugeDarkSprite == null)
            return;

        // Visible only when currentGauge > 0
        bool visible = currentGauge > 0f;
        if (!visible)
        {
            // ensure sprites are disabled and fully transparent
            SetSpriteAlpha(gaugeLightSprite, 0f);
            SetSpriteAlpha(gaugeDarkSprite, 0f);
            gaugeLightSprite.enabled = false;
            gaugeDarkSprite.enabled = false;
            return;
        }

        // Position sprites above the guard
        gaugeLightSprite.transform.position = transform.position + gaugeOffset;
        gaugeDarkSprite.transform.position = transform.position + gaugeOffset;

        // Make sure dark overlays light (sortingOrder or z)
        if (gaugeDarkSprite.sortingOrder <= gaugeLightSprite.sortingOrder)
            gaugeDarkSprite.sortingOrder = gaugeLightSprite.sortingOrder + 1;

        // Enable renderers when visible
        gaugeLightSprite.enabled = true;
        gaugeDarkSprite.enabled = true;

        float target = Mathf.Clamp01(currentGauge / maxGauge);

        // Smooth the alpha change for nicer visuals
        float currentDarkAlpha = gaugeDarkSprite.color.a;
        float newDarkAlpha = Mathf.Lerp(currentDarkAlpha, target, 1f - Mathf.Exp(-fadeSmoothing * Time.deltaTime * 60f));
        float newLightAlpha = 1f - newDarkAlpha;

        SetSpriteAlpha(gaugeDarkSprite, newDarkAlpha);
        SetSpriteAlpha(gaugeLightSprite, newLightAlpha);
    }

    // Helper to set alpha while preserving RGB
    private void SetSpriteAlpha(SpriteRenderer sr, float alpha)
    {
        if (sr == null) return;
        Color c = sr.color;
        c.a = Mathf.Clamp01(alpha);
        sr.color = c;
    }
}
