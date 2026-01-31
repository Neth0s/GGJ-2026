using UnityEngine;

/// <summary>
/// Script of the NPC
/// </summary>
public class NPCController : MonoBehaviour
{
    #region VARIABLES
    [Header("NPC Data")]
    [SerializeField] private NPCData _data;

    private SpriteRenderer _spriteRenderer;
    private GameObject _highlight;
    private SpriteRenderer _highlightRenderer;

    /* TODO : DELETE
    [SerializeField] private Material _highlightMaterial; //TODO : might be removed (material highlight)
    
    private Material _defaultMaterial; //TODO : might be removed (material highlight)
    private MeshRenderer _meshRenderer;
    */
    #endregion

    private void Awake()
    {
        //_meshRenderer = GetComponentInChildren<MeshRenderer>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer == null) return;
        _highlight = new GameObject("Highlight Object");
        _highlight.transform.parent = this.transform;
        _highlightRenderer = _highlight.AddComponent<SpriteRenderer>();
        _highlightRenderer.sprite = _spriteRenderer.sprite;
        _highlight.transform.localPosition = new Vector3(0.0f, 0.11f, 0.16f);
        _highlight.transform.localScale = new Vector3(0.39f, 0.39f, 0.39f);
        _highlight.transform.rotation = transform.rotation;
        _highlightRenderer.color = Color.cyan;
        _highlight.SetActive(false);
        //_defaultMaterial = _meshRenderer.material;
    }

    /// <summary>
    /// Function that will trigger the highlight
    /// </summary>
    public void TriggerHightlight()
    {
        //TODO : Highlight to change (visual)
        //_meshRenderer.material = _highlightMaterial;
        _highlight?.SetActive(true);
    }

    /// <summary>
    /// Function that will hide the highlight
    /// </summary>
    public void AbandonHighlight()
    {
        //TODO : Highlight to change (visual)
        //_meshRenderer.material = _defaultMaterial;
        _highlight?.SetActive(false);
    }

    /// <summary>
    /// Will tell if the NPC is a bad guy or not
    /// </summary>
    /// <returns></returns>
    public bool IsNPCBadGuy()
    {
        if (_data.IsBadGuy)
        {
            return true;
        }
        return false;
    }
}
