using System.Collections;
using UnityEngine;

public class HoverSprite : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float changeSpeed = 1f;
    [SerializeField] private Color colorHover = Color.black;
    [SerializeField] private Color colorSelected = Color.black;

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
        Color targetColor = selected ? colorSelected : colorHover;
        float t = 0.0f;

        while (t < 1)
        {
            t += changeSpeed * Time.deltaTime;
            _spriteRenderer.color = Color.Lerp(_initialColor, targetColor, t);
            yield return new WaitForFixedUpdate();
        }

        _spriteRenderer.color = targetColor;
    }


}
