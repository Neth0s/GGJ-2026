using UnityEngine;

/// <summary>
/// Script of the NPC
/// </summary>
public class NPCController : MonoBehaviour
{
    #region VARIABLES
    [Header("NPC Data")]
    [SerializeField] private NPCData _data;

    [Header("Highlight")]
    [SerializeField] private GameObject baseSprite;
    [SerializeField] private Sprite outline;
    [SerializeField] private Vector3 _highlightScale;
    [SerializeField] private Color _highlightColor = Color.cyan;

    private SpriteRenderer _spriteRenderer;
    private GameObject _highlight;
    private SpriteRenderer _highlightRd;
    #endregion

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer == null) return;
        
        CreateHighlight();
    }

    private void CreateHighlight()
    {
        _highlight = new GameObject("Highlight Object");
        _highlight.transform.parent = this.transform;

        _highlight.transform.localPosition = baseSprite.transform.localPosition;
        _highlight.transform.localScale = baseSprite.transform.localScale.x * _highlightScale;
        _highlight.transform.rotation = transform.rotation;

        _highlightRd = _highlight.AddComponent<SpriteRenderer>();
        _highlightRd.sprite = outline;
        _highlightRd.color = _highlightColor;

        _highlight.SetActive(false);
    }

    public void Select()
    {
        _highlight.SetActive(true);
    }

    public void Deselect()
    {
        _highlight.SetActive(false);
    }

    public bool IsNPCBadGuy()
    {
        return _data.IsBadGuy;
    }
}
