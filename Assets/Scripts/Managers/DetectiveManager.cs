using System.Collections.Generic;
using UnityEngine;
using TMPro; // Assuming TextMeshPro is used for UI
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the investigation gameplay loop: Timer, Clues, and Win/Loss conditions.
/// </summary>
public class DetectiveManager : MonoBehaviour
{
    public static DetectiveManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float timeLimit = 300f; // 5 minutes
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI clueTextDisplay;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private float _timer;
    private bool _isGameActive;
    private List<string> _collectedClues = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _timer = timeLimit;
        _isGameActive = true;
        UpdateClueUI();
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
    }

    private void Update()
    {
        if (!_isGameActive) return;

        _timer -= Time.deltaTime;
        UpdateTimerUI();

        if (_timer <= 0)
        {
            EndGame(false);
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(_timer / 60);
            int seconds = Mathf.FloorToInt(_timer % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void AddClue(string clue)
    {
        if (!_collectedClues.Contains(clue))
        {
            _collectedClues.Add(clue);
            UpdateClueUI();
        }
    }

    private void UpdateClueUI()
    {
        if (clueTextDisplay != null)
        {
            clueTextDisplay.text = "Clues:\n";
            foreach (var clue in _collectedClues)
            {
                clueTextDisplay.text += $"- {clue}\n";
            }
        }
    }

    public void AttemptAccusation(bool isCulprit)
    {
        if (isCulprit)
        {
            EndGame(true);
        }
        else
        {
            // Wrong accusation logic - maybe time penalty or instant loss?
            // For MVP, let's say instant loss or just a message. 
            // Let's go with instant loss for high stakes.
            EndGame(false);
        }
    }

    public void CaughtByGuard()
    {
        EndGame(false);
    }

    private void EndGame(bool win)
    {
        _isGameActive = false;
        if (win)
        {
            if (winPanel) winPanel.SetActive(true);
            Debug.Log("YOU WON! Culprit found.");
        }
        else
        {
            if (losePanel) losePanel.SetActive(true);
            Debug.Log("GAME OVER.");
        }
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
