using UnityEngine;

/// <summary>
/// Script that will govern the NPC-side merchant
/// </summary>
public class MerchantController : MonoBehaviour
{
    #region VARIABLES
    private InvestigationPlayer _player;
    #endregion
    
    /// <summary>
    /// Function that will start the Merchant interaction. In effect, will communicate with the UI to trigger it.
    /// </summary>
    /// <param name="player">Player</param>
    /// <returns></returns>
    public bool StartMerchantInteraction(InvestigationPlayer player)
    {
        print("Starting Merchant interaction");
        _player = player;
        MerchantUIController merchantUI = UIManager.Instance.GetMerchantUIController();
        UIManager.Instance.DisplayMerchantUI(true);
        return true;
    }

    /// <summary>
    /// Function that will end the merchant's interaction.
    /// </summary>
    public void EndMerchantInteraction()
    {
        print("Ending merchant interaction");
        _player.DisableMerchantInteraction();
        UIManager.Instance.DisplayMerchantUI(false);
    }
}
