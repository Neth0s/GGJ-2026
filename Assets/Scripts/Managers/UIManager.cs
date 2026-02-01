using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance => instance;

    #region VARIABLES
    [Header("UI Variables")]
    [SerializeField] private GameObject chooseMaskPanel;
    [SerializeField] private GameObject chooseMaskButtonUI;
    [SerializeField] private GameObject indicePanel;
    [SerializeField] private GameObject indicePanelButtonUI;

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
        _merchantUIController.gameObject.SetActive(false);
    }

    public void DisplayChooseMask(bool display)
    {
        chooseMaskPanel.SetActive(display);
        indicePanelButtonUI.SetActive(!display);
        chooseMaskButtonUI.SetActive(!display);
        if (display) chooseMaskPanel.GetComponent<MaskChoiceController>().InitializeUI();
              
    }

    public void DisplayIndice(bool display, List<string> indices)
    {
        indicePanel.SetActive(display);
        indicePanelButtonUI.SetActive(!display);
        chooseMaskButtonUI.SetActive(!display);
        if (display) {
            string textToDisplay = "Indices :" + '\n';
            foreach (string indice in indices)
            {
                textToDisplay += '-' + indice + '\n';
            }
            indicePanel.GetComponentInChildren<TextMeshProUGUI>().text = textToDisplay;
        }
        
    }

    public void DisplayMerchantUI(bool display)
    {
        _merchantUIController.gameObject.SetActive(display);
        if (display) _merchantUIController.InitializeUI();
    }
}
