using UnityEngine;

/// <summary>
/// Script of the Group
/// </summary>
public class GroupController : MonoBehaviour
{
    #region VARIABLES
    [Header("Group Data")]
    [SerializeField] private GroupData _data;
    #endregion

    public Enums.MaskType CommonMask => _data != null ? _data.CommonMask : Enums.MaskType.None;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.SetCurrentGroup(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.SetCurrentGroup(null);
            }
        }
    }

    public string GetRandomClue()
    {
        if (_data != null && _data.Clues != null && _data.Clues.Count > 0)
        {
            return _data.Clues[Random.Range(0, _data.Clues.Count)];
        }
        return "...";
    }

    public void TriggerGroupSelection()
    {
        print("Group "+gameObject.name+" is selected !");
    }
}
