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
    private int minutes;
    private int seconds;

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

        minutes = Mathf.FloorToInt(currentTime / 60F);
        seconds = Mathf.FloorToInt(currentTime - minutes * 60);

        textTimer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
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
