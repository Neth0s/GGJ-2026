using System.Collections;
using UnityEngine;

/// <summary>
/// This script will handle the darkening of the player
/// </summary>
public class PlayerDarkController : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private SpriteRenderer _playerSprite;

    private Color _initialColor;

    [SerializeField] private float _colorFactor = 0.02f;
    [SerializeField] private float _colorMin = 0.3f;
    #endregion

    private void Awake()
    {
        if( _playerSprite == null)
        {
            Debug.LogError("ERROR : Player must have a sprite renderer in Dark Controller");
        }
        _initialColor = _playerSprite.color;
    }

    public void DarkenPlayer()
    {
        _playerSprite.color = _initialColor;
        StartCoroutine(DarkenPlayerCoroutine());
    }

    public void BrightenPlayer()
    {
        _playerSprite.color = _initialColor;
    }

    private IEnumerator DarkenPlayerCoroutine()
    {
        print(_playerSprite.color.r);
        while (_playerSprite.color.r > _colorMin)
        {
            float colorElt = _playerSprite.color.r - _colorFactor;
            print("Woop " + colorElt);
            Color newColor = new Color(colorElt, colorElt, colorElt);
            _playerSprite.color = newColor;
            yield return new WaitForFixedUpdate();
        }
    }
}
