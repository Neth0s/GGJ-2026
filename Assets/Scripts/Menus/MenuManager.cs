using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    public void LaunchLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        UIManager.Instance.DisplayButtonsUI(true);
    }
    public void LaunchLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        UIManager.Instance.DisplayButtonsUI(true);
    }
}
