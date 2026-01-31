using UnityEngine;
using UnityEngine.UI;
using Enums;

public class GuardController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Slider suspicionSlider; // Use Slider for bar
    [SerializeField] private float suspicionRate = 20f; 
    [SerializeField] private float recoveryRate = 5f; 

    private float currentSuspicion = 0f;
    private float maxSuspicion = 100f;

    private void Start()
    {
        if (player == null) player = FindFirstObjectByType<Player>();
        if (suspicionSlider != null)
        {
            suspicionSlider.maxValue = maxSuspicion;
            suspicionSlider.value = 0;
        }
    }

    private void Update()
    {
        if (player == null) return;

        bool isSuspicious = false;

        if (player.CurrentGroup != null)
        {
            // Check Mask
            if (player.CurrentGroup.CommonMask != MaskType.None && player.CurrentMask != player.CurrentGroup.CommonMask)
            {
                isSuspicious = true;
            }
        }

        if (isSuspicious)
        {
            currentSuspicion += suspicionRate * Time.deltaTime;
        }
        else
        {
            currentSuspicion -= recoveryRate * Time.deltaTime;
        }

        currentSuspicion = Mathf.Clamp(currentSuspicion, 0, maxSuspicion);

        if (suspicionSlider != null)
        {
            suspicionSlider.value = currentSuspicion;
        }

        if (currentSuspicion >= maxSuspicion)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LooseGame();
                enabled = false; // Disable guard checking
            }
        }
    }
}
