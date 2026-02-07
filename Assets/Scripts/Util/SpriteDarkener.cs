using System.Collections;
using UnityEngine;

public class SpriteDarkener : MonoBehaviour
{
    #region VARIABLES
    [SerializeField, Tooltip("Sprite to darken")] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _darkenSpeed = 0.02f;
    [SerializeField] private float _colorMin = 0.7f;

    private Color _initialColor;
    private Material _material;
    private Coroutine _currentCoroutine = null;
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
            _initialColor = _spriteRenderer.material.color;
        }
    }

    public void Darken()
    {
        _spriteRenderer.material.color = _initialColor;
        _currentCoroutine ??= StartCoroutine(DarkenCoroutine());
    }

    public void Brighten()
    {
        StopAllCoroutines();
        _currentCoroutine = null;
        _spriteRenderer.material.color = _initialColor;
    }

    private IEnumerator DarkenCoroutine()
    {
        while (_spriteRenderer.material.color.r > _colorMin)
        {
            float luminosity = _spriteRenderer.material.color.r - _darkenSpeed;
            Color newColor = new(luminosity, luminosity, luminosity);
            _spriteRenderer.material.color = newColor;
            yield return new WaitForFixedUpdate();
        }
    }
}
