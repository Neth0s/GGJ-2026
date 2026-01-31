using System.Collections;
using UnityEngine;

public class GuardNew : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private Vector3[] waypoints;

    [SerializeField] private float minPauseTime, maxPauseTime;

    private int currentWaypoint = 0;
    private bool isWalking = true;
    private float pauseTimer = 0f;
    private float pauseDuration = 0f;

    private void Start()
    {
        transform.position = waypoints[0];
        currentWaypoint = 1;
    }

    void Update()
    {
        if (isWalking)
        {
            Vector3 target = waypoints[currentWaypoint];
            Vector3 direction = (target - transform.position).normalized;

            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

            // Flip sprite when moving along X
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

            // Arrived at waypoint
            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                transform.position = target;
                isWalking = false;
                pauseDuration = Random.Range(minPauseTime, maxPauseTime);
                pauseTimer = 0f;
            }
        }
        else
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                isWalking = true;
            }
        }
    }
}