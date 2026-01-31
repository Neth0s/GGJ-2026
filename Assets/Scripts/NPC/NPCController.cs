using UnityEngine;
using Enums;

/// <summary>
/// Script of the NPC
/// </summary>
public class NPCController : MonoBehaviour
{
    #region VARIABLES
    [Header("NPC Data")]
    [SerializeField] private NPCData _data;
    [SerializeField] private SpriteRenderer maskRenderer; 

    [SerializeField] private Material _highlightMaterial; 
    
    private Material _defaultMaterial; 
    private MeshRenderer _meshRenderer;

    public NPCData Data => _data;

    #endregion

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (_meshRenderer != null)
        {
            _defaultMaterial = _meshRenderer.material;
        }
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (_data != null && maskRenderer != null)
        {
            switch (_data.Mask)
            {
                case MaskType.Triangle: maskRenderer.color = Color.red; break;
                case MaskType.Circle: maskRenderer.color = Color.blue; break;
                case MaskType.Square: maskRenderer.color = Color.green; break;
                default: maskRenderer.color = Color.white; break;
            }
        }
    }

    /// <summary>
    /// Function that will trigger the highlight
    /// </summary>
    public void TriggerHightlight()
    {
        if (_meshRenderer != null && _highlightMaterial != null)
            _meshRenderer.material = _highlightMaterial;
    }

    /// <summary>
    /// Function that will hide the highlight
    /// </summary>
    public void AbandonHighlight()
    {
        if (_meshRenderer != null && _defaultMaterial != null)
            _meshRenderer.material = _defaultMaterial;
    }

    public string GiveClue()
    {
        if (_data != null && !string.IsNullOrEmpty(_data.SpecificClue))
        {
            return _data.SpecificClue;
        }
        
        var group = GetComponentInParent<GroupController>();
        if (group != null)
        {
            return group.GetRandomClue();
        }

        return "I have nothing to say.";
    }

    public bool CheckAccusation()
    {
        return _data != null && _data.IsCulprit;
    }
}
