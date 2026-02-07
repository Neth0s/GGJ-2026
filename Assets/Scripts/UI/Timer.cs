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

    [Header("Timer Loss Elements")]
    [SerializeField] private GameObject _timerLoss;
    [SerializeField] private TextMeshProUGUI _textTimerLoss;
    
    private float currentTime;
    private bool isTimerRunning = false;

    private void Start()
    {
        currentTime = timerDuration;
        isTimerRunning = true;
    }

    private void Update()
    {
        if (isTimerRunning) currentTime -= Time.deltaTime;
        textTimer.text = TransformTimeToClockTime(currentTime);

        if (currentTime <= 0.0f) GameManager.Instance.LoseGame();
    }

    public void PauseTimer(bool value) => isTimerRunning = !value;

    public void AddTime(float time)
    {
        currentTime = Mathf.Max(timerDuration, currentTime + time);
    }

    public void RemoveTime(float time)
    {
        currentTime = currentTime - time;
        _timerLoss.GetComponent<Animator>().Play("TimerLoss");
        _textTimerLoss.text = "- " + TransformTimeToClockTime(time);
    }

    /// <summary>
    /// Will return a float time to the format minutes:seconds
    /// </summary>
    /// <param name="time">Inputted time (float)</param>
    /// <returns>String in format minutes:seconds</returns>
    private string TransformTimeToClockTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
