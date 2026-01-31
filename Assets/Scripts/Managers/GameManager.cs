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

        //Old IA guard
        /*
        // Auto-Setup for Investigation Scene
        if (SceneManager.GetActiveScene().name == "ImplementationSceneVictor")
        {
            if (gameObject.GetComponent<InvestigationAutoSetup>() == null)
            {
                gameObject.AddComponent<InvestigationAutoSetup>();
            }
        }*/
    }

    public void LooseGame()
    {
        SceneManager.LoadScene(2);
    }
    
    public void WinGame()
    {
        SceneManager.LoadScene(3);
    }

    
}
