using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance => instance;

    #region VARIABLES
    [Header("Variables")]
    [SerializeField] private float fadeOutDuration = 1f;

    [Header("References")]
    [SerializeField] private GameObject indicePanel;
    [SerializeField] private MaskChoiceController maskPanel;
    [SerializeField] private MerchantUIController merchantPanel;
    [SerializeField] private Image blackScreen;
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
            Destroy(gameObject);
            return;
        }
        else instance = this;

        DontDestroyOnLoad(gameObject);
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

    public void FadeOut(Transform player, Vector3 resetPosition)
    {
        StartCoroutine(FadeOutCoroutine(player, resetPosition));
    }

    private IEnumerator FadeOutCoroutine(Transform player, Vector3 resetPosition)
    {
        float alpha = 0f;
        float increment = 2 / fadeOutDuration;
        Color c = blackScreen.color;

        while (alpha < 1)
        {
            alpha = Mathf.Clamp01(alpha + increment * Time.deltaTime);
            blackScreen.color = new Color(c.r, c.g, c.b, alpha);
            yield return new WaitForEndOfFrame();
        }

        player.localPosition = resetPosition;

        while (alpha > 0)
        {
            alpha = Mathf.Clamp01(alpha - increment * Time.deltaTime);
            blackScreen.color = new Color(c.r, c.g, c.b, alpha);
            yield return new WaitForEndOfFrame();
        }

        DisplayMaskPanel(false);
        DisplayIndices(false, null);
    }
}
