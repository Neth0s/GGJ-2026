using System.Collections.Generic;
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
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void LooseGame()
    {
        SceneManager.LoadScene("EndScene");
    }
    
    public void WinGame()
    {
        SceneManager.LoadScene("EndVictoryScene");
    }

    
}
