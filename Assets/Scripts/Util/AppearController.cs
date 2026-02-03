using System.Collections;
using UnityEngine;

public class AppearController : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Coroutine _currentCoroutine = null;
    private Color _initialColor;

    [SerializeField] private float _alphaFactor = 0.02f;
    [SerializeField] private float _maxAlpha = 1.0f;
    #endregion

    private void Awake()
    {
        if (_spriteRenderer == null)
        {
            Debug.LogError("ERROR : " + gameObject.name + " must have a sprite renderer in Appear Controller");
            _initialColor.a = 0f;
        }
        else
        {
            _initialColor = _spriteRenderer.color;
            _initialColor.a = _spriteRenderer.color.a;
        }
    }

    public void Appear()
    {
        _spriteRenderer.color = _initialColor;
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(AppearCoroutine());
        }
    }

    public void Hide()
    {
        StopAllCoroutines();
        _currentCoroutine = null;
        Color newColor = _initialColor;
        newColor.a = 0.0f;
        _spriteRenderer.color = newColor;
    }

    private IEnumerator AppearCoroutine()
    {
        while (_spriteRenderer.color.a <= _maxAlpha)
        {
            float alphaElt = _spriteRenderer.color.a + _alphaFactor;
            Color newColor = _spriteRenderer.color;
            newColor.a = alphaElt;
            _spriteRenderer.color = newColor;
            yield return new WaitForFixedUpdate();
        }
    }


}
