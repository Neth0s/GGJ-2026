using System.Collections;
using UnityEngine;

public class HoverSprite : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float alphaSpeed = 0.02f;
    [SerializeField] private float alphaHover = 0.5f;
    [SerializeField] private float alphaSelected = 1.0f;

    private Color _initialColor;
    private Coroutine _currentCoroutine = null;
    #endregion

    private void Awake()
    {
        _initialColor = _spriteRenderer.color;
        _initialColor.a = _spriteRenderer.color.a;
    }

    public void Appear(bool selected)
    {
        _spriteRenderer.color = _initialColor;
        _currentCoroutine ??= StartCoroutine(AppearCoroutine(selected));
    }

    public void Hide()
    {
        StopAllCoroutines();
        _currentCoroutine = null;
        Color newColor = _initialColor;
        newColor.a = 0.0f;
        _spriteRenderer.color = newColor;
    }

    private IEnumerator AppearCoroutine(bool selected)
    {
        float targetAlpha = selected ? alphaSelected : alphaHover;

        while (_spriteRenderer.color.a <= targetAlpha)
        {
            float alpha = _spriteRenderer.color.a + alphaSpeed;
            Color newColor = _spriteRenderer.color;
            newColor.a = alpha;
            _spriteRenderer.color = newColor;
            yield return new WaitForFixedUpdate();
        }
    }


}
