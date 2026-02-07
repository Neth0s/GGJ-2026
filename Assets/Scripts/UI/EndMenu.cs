using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
}
