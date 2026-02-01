using UnityEngine;

/// <summary>
/// Script with only ONE purpose : call the function DisplayButtonsUI of UIManager
/// Why ? Cause fuk u that's why
/// </summary>
public class ButtonsUICaller : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.DisplayButtonsUI(true);
    }
}
