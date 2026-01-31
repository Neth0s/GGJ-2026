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

    #endregion

    private void Awake()
    {
        if( _playerSprite == null)
        {
            Debug.LogError("ERROR : Player must have a sprite renderer in one of its children");
        }
        _initialColor = _playerSprite.color;
    }

    public void DarkenPlayer()
    {
        _playerSprite.color = _initialColor;
    }

    private IEnumerator DarkenPlayerCoroutine()
    {
        Color newColor = new Color(_playerSprite.color.r - _colorFactor, _playerSprite.color.g - _colorFactor, _playerSprite.color.b - _colorFactor);
        yield return new WaitForEndOfFrame();
    }
}
