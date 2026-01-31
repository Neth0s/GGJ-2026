using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    [SerializeField] private List<MaskObject> allMaskObjects = new List<MaskObject>();
    [SerializeField] private List<MaskObject> inventory = new List<MaskObject>();

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
        SceneManager.LoadScene(2);
    }
    
    public void WinGame()
    {
        SceneManager.LoadScene(3);
    }

    
}
