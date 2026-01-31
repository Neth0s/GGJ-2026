using UnityEngine;

/// <summary>
/// Script of the NPC
/// </summary>
public class NPCController : MonoBehaviour
{
    #region VARIABLES
    [Header("NPC Data")]
    [SerializeField] private NPCData _data;

    [SerializeField] private Material _highlightMaterial; //TODO : might be removed (material highlight)
    
    private Material _defaultMaterial; //TODO : might be removed (material highlight)
    private MeshRenderer _meshRenderer;

    #endregion

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _defaultMaterial = _meshRenderer.material;
    }

    /// <summary>
    /// Function that will trigger the highlight
    /// </summary>
    public void TriggerHightlight()
    {
        //TODO : Highlight to change (visual)
        _meshRenderer.material = _highlightMaterial;
    }

    /// <summary>
    /// Function that will hide the highlight
    /// </summary>
    public void AbandonHighlight()
    {
        //TODO : Highlight to change (visual)
        _meshRenderer.material = _defaultMaterial;
    }
}
