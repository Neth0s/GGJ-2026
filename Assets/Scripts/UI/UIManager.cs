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
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image blackScreen;
    [Space(10)]
    [SerializeField] private GameObject maskButton;
    [SerializeField] private GameObject indicesButton;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI indiceText;

    [Header("UI Events")]
    [SerializeField] private UnityEngine.Events.UnityEvent sceneEvents;
    #endregion

    public float PlayerResetDuration => fadeOutDuration;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else instance = this;
    }

    private void Start()
    {
        maskPanel.gameObject.SetActive(false);
        merchantPanel.gameObject.SetActive(false);

        sceneEvents.Invoke();
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

    public void DisplayPauseMenu(bool display)
    {
        Timer.Instance.PauseTimer(display);
        Time.timeScale = display ? 0 : 1;

        pauseMenu.SetActive(display);
        DisplayButtonsUI(!display);
    }

    public void QuitGame()
    {
        GameManager.Instance.ReturnToMenu();
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
        DisplayMaskPanel(false);
        DisplayIndices(false, null);

        float alpha = 0f;
        float increment = 3 / fadeOutDuration;
        Color c = blackScreen.color;

        while (alpha < 1)
        {
            alpha = Mathf.Clamp01(alpha + increment * Time.deltaTime);
            blackScreen.color = new Color(c.r, c.g, c.b, alpha);
            yield return new WaitForEndOfFrame();
        }

        player.localPosition = resetPosition;
        yield return new WaitForSeconds(fadeOutDuration / 3);

        while (alpha > 0)
        {
            alpha = Mathf.Clamp01(alpha - increment * Time.deltaTime);
            blackScreen.color = new Color(c.r, c.g, c.b, alpha);
            yield return new WaitForEndOfFrame();
        }
    }
}
