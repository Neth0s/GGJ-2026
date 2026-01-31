using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance => instance;

    #region VARIABLES
    [Header("UI Variables")]
    [SerializeField] private GameObject chooseMaskPanel;
    [SerializeField] private MerchantUIController _merchantUIController;
    #endregion

    #region GETTERS AND SETTERS
    public MerchantUIController GetMerchantUIController() { return _merchantUIController; }
    #endregion

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

    public void DisplayMerchantUI(bool display)
    {
        _merchantUIController.gameObject.SetActive(display);
        if (display) _merchantUIController.InitializeUI();
    }
}
