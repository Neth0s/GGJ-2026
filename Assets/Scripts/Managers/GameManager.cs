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
        UIManager.Instance.DisplayButtonsUI(false);
    }
    
    public void WinGame()
    {
        SceneManager.LoadScene("Victory");
        UIManager.Instance.DisplayButtonsUI(false);
    }
}
