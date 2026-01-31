using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance => instance;

    [SerializeField] private GameObject chooseMaskPanel;

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

    private void Start()
    {
        chooseMaskPanel.SetActive(false);
    }

    public void DisplayChooseMask(bool display)
    {
        chooseMaskPanel.SetActive(display);
        if (display) chooseMaskPanel.GetComponent<MaskChoiceController>().InitializeUI();
              
    }

}
