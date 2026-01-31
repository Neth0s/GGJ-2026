using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI urgeText; // Suspicion?
    [SerializeField] private GameObject clueWindow;
    [SerializeField] private TextMeshProUGUI clueText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnClueAdded += UpdateClueWindow;
        }
        if (clueText != null) clueText.text = "";
    }

    private void UpdateClueWindow(string newClue)
    {
        if (clueText != null)
        {
            clueText.text += "- " + newClue + "\n";
        }
        if (clueWindow != null) clueWindow.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.Instance != null && timerText != null)
        {
            float t = GameManager.Instance.GameTimer;
            timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(t / 60), Mathf.FloorToInt(t % 60));
        }
    }
}
