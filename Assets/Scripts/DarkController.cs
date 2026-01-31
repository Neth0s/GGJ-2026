using System.Collections;
using UnityEngine;

/// <summary>
/// This script will handle the darkening of a sprite element
/// </summary>
public class DarkController : MonoBehaviour
{
    #region VARIABLES
    [SerializeField, Tooltip("Sprite to darken")] private SpriteRenderer _spriteRenderer;

    private Color _initialColor;

    [SerializeField] private float _colorFactor = 0.02f;
    [SerializeField] private float _colorMin = 0.7f;

    private Coroutine _currentCoroutine=null;
    #endregion

    private void Awake()
    {
        if( _spriteRenderer == null)
        {
            Debug.LogError("ERROR : "+ gameObject.name +" must have a sprite renderer in Dark Controller");
            _initialColor = Color.white;
        }
        else
        {
            _initialColor = _spriteRenderer.color;
        }
    }

    public void Darken()
    {
        _spriteRenderer.color = _initialColor;
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(DarkenCoroutine());
        }
    }

    public void Brighten()
    {
        StopAllCoroutines();
        _currentCoroutine = null;
        _spriteRenderer.color = _initialColor;
    }

    private IEnumerator DarkenCoroutine()
    {
        while (_spriteRenderer.color.r > _colorMin)
        {
            float colorElt = _spriteRenderer.color.r - _colorFactor;
            Color newColor = new Color(colorElt, colorElt, colorElt);
            _spriteRenderer.color = newColor;
            yield return new WaitForFixedUpdate();
        }
    }
}
