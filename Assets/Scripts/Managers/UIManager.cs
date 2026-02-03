using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance => instance;

    #region VARIABLES
    [Header("UI Variables")]
    [SerializeField] private GameObject indicePanel;
    [SerializeField] private MaskChoiceController maskPanel;
    [SerializeField] private MerchantUIController merchantPanel;
    [Space(10)]
    [SerializeField] private GameObject maskButton;
    [SerializeField] private GameObject indicesButton;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI indiceText;
    #endregion

    #region GETTERS AND SETTERS
    public MerchantUIController GetMerchantUIController() { return merchantPanel; }
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
        maskPanel.gameObject.SetActive(false);
        merchantPanel.gameObject.SetActive(false);

        maskButton.SetActive(false);
        indicesButton.SetActive(false);
    }

    /// <summary>
    /// Changes the display status of the two buttons of the UI
    /// </summary>
    /// <param name="status"></param>
    public void DisplayButtonsUI(bool status)
    {
        maskButton.SetActive(status);
        indicesButton.SetActive(status);
    }

    public void DisplayMaskPanel(bool display)
    {
        maskPanel.gameObject.SetActive(display);
        indicesButton.SetActive(!display);
        maskButton.SetActive(!display);

        if (display) maskPanel.InitializeUI();
              
    }

    public void DisplayIndices(bool display, List<string> indices)
    {
        indicePanel.SetActive(display);
        indicesButton.SetActive(!display);
        maskButton.SetActive(!display);

        if (display)
        {
            string textToDisplay = string.Empty;

            foreach (string indice in indices)
            {
                textToDisplay += "- " + indice + '\n';
            }
            indiceText.text = textToDisplay;
        }
    }

    public void DisplayMerchantUI(bool display)
    {
        merchantPanel.gameObject.SetActive(display);
        if (display) merchantPanel.InitializeUI();
    }
}
