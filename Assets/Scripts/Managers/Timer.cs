using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timerDuration = 100f;
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
