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
    [SerializeField] private float detectionRadius = 20f;
    
    [Header("Gauge")]
    [SerializeField] private float gaugeIncreaseSpeed = 20f;
    [SerializeField] private float maxGauge = 100f;

    [Header("Gauge Visual")]
[SerializeField] private SpriteRenderer gaugeSprite;
[SerializeField] private Vector3 gaugeOffset = new Vector3(0, 2, 0); // Position au-dessus du garde

    private int currentWaypoint = 0;
    private bool isWalking = true;
    private float pauseTimer = 0f;
    private float pauseDuration = 0f;

    private float currentGauge = 0f;
    private bool isDetectingPlayer = false;
    
    private Transform playerTransform;
    private DetectController detectController;

    private void Start()
    {
        transform.position = waypoints[0];
        currentWaypoint = 1;
        
        // Trouve automatiquement le joueur par son tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        print(player.name);
        if (player != null)
        {
            playerTransform = player.transform;
            detectController = player.GetComponent<DetectController>();
        }
        else
        {
            Debug.LogError("Aucun objet avec le tag 'Player' trouvé!");
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
            currentGauge += gaugeIncreaseSpeed * Time.deltaTime;
            if (currentGauge >= maxGauge)
            {
                GameOver();
            }
        }
        else
        {
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

    void GameOver()
    {
        Debug.Log("Game Over - Joueur détecté par le garde!");
        currentGauge = maxGauge;
        // Ajoute ici ton code de game over
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isDetectingPlayer ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public float GetGaugePercentage()
    {
        return currentGauge / maxGauge;
    }

    void UpdateGaugeVisual()
    {
        if (gaugeSprite != null)
        {
            // Affiche la jauge seulement si elle est > 0
            gaugeSprite.enabled = (currentGauge > 0);
            
            if (currentGauge > 0)
            {
                // Position au-dessus du garde
                gaugeSprite.transform.position = transform.position + gaugeOffset;
                
                // Change la largeur selon le pourcentage
                float percentage = currentGauge / maxGauge;
                Vector3 scale = gaugeSprite.transform.localScale;
                scale.x = percentage;
                gaugeSprite.transform.localScale = scale;
                
                // Change la couleur (vert → rouge)
                gaugeSprite.color = Color.Lerp(Color.green, Color.red, percentage);
            }
        }
}
}