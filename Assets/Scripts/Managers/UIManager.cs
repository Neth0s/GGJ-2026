using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance => instance;

    [SerializeField] private GameObject chooseMaskPanel;
    [SerializeField] private GameObject indicePanel;

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


    public void DisplayIndice(bool display, List<string> indices)
    {
        indicePanel.SetActive(display);
        if (display) {
            string textToDisplay = "";
            foreach(string indice in indices)
            {
                textToDisplay += indice + '\n';
            }
            indicePanel.GetComponentInChildren<TextMeshProUGUI>().text = textToDisplay;
        }

    }
}
