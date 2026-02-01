using UnityEngine;

/// <summary>
/// Script that will govern the NPC-side merchant. Mostly used for detection purposes
/// </summary>
public class MerchantController : MonoBehaviour
{

    //IF LEFT UNUSED : DELETE everything here
    
    /// <summary>
    /// Function that will start the Merchant interaction. In effect, will communicate with the UI to trigger it.
    /// </summary>
    /// <param name="player">Player</param>
    /// <returns></returns>
    /*
    public bool StartMerchantInteraction(InvestigationPlayer player)
    {
        print("Starting Merchant interaction");
        _player = player;
        UIManager.Instance.DisplayMerchantUI(true);
        return true;
    }
    */

    /// <summary>
    /// Function that will end the merchant's interaction. In effect, will communicate with the UI to trigger it.
    /// </summary>
    /*
    public void EndMerchantInteraction()
    {
        print("Ending merchant interaction");
        _player.DisableMerchantInteraction();
        UIManager.Instance.DisplayMerchantUI(false);
    }
    */
}
