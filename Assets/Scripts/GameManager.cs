using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance => instance;
    public event System.Action<string> OnClueAdded;




    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    [SerializeField] private float gameTimer = 300f;
    private List<string> collectedClues = new List<string>();

    public float GameTimer => gameTimer;

    private void Update()
    {
        if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0)
            {
                LooseGame();
            }
        }
    }

   public void LooseGame()
    {
        Debug.Log("Game Over!");
        // SceneManager.LoadScene(2); // Keep existing logic
    }

    public void WinGame()
    {
        Debug.Log("You Found the Culprit!");
        // Load Win Scene
    }

    public void AddClue(string clue)
    {
        if (!collectedClues.Contains(clue))
        {
            collectedClues.Add(clue);
            Debug.Log("New Clue: " + clue);
            OnClueAdded?.Invoke(clue);
        }
    }

}
