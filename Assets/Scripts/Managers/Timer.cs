using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region SINGLETON DESIGN PATTERN
    public static Timer Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    [SerializeField] private float timerDuration = 300f;
    [SerializeField] private TextMeshProUGUI textTimer;
    private float currentTime;

    private void Start()
    {
        currentTime = timerDuration;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0.0f)
        {
            GameManager.Instance.LooseGame();
        }
        textTimer.text = ((int)currentTime).ToString();
    }

    public void AddTime(float time)
    {
        currentTime = Mathf.Max(timerDuration, currentTime + time);
    }

    public void RemoveTime(float time)
    {
        currentTime=currentTime - time;
    }
}
