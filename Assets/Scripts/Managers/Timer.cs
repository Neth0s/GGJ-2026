using System.Collections;
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
    private Coroutine _timerLossCoroutine;
    
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
        textTimer.text = TransformTimeToClockTime(currentTime);
    }

    public void AddTime(float time)
    {
        currentTime = Mathf.Max(timerDuration, currentTime + time);
    }

    /// <summary>
    /// Remove time from the timer
    /// </summary>
    /// <param name="time"></param>
    public void RemoveTime(float time)
    {
        currentTime=currentTime - time;
        _timerLoss.GetComponent<Animator>().Play("TimerLoss");
        _textTimerLoss.text = "- "+TransformTimeToClockTime(time);
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
