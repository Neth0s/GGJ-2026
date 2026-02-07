using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void LoseGame()
    {
        SceneManager.LoadScene("Defeat");
    }
    
    public void WinGame()
    {
        SceneManager.LoadScene("Victory");
    }
}
