using UnityEngine;

/// <summary>
/// Script of the NPC
/// </summary>
public class NPCController : MonoBehaviour
{
    #region VARIABLES
    [Header("NPC Data")]
    [SerializeField] private Material _highlightMaterial;
    
    private Material _defaultMaterial;
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
        print("I'm highlighted hooray woop woop !");
        _meshRenderer.material = _highlightMaterial;
    }

    /// <summary>
    /// Function that 
    /// </summary>
    public void AbandonHighlight()
    {
        _meshRenderer.material = _defaultMaterial;
    }
}
