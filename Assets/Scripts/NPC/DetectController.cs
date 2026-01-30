using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This script will govern the detection by the player towards NPCs and other shits like that
/// </summary>
public class DetectController : MonoBehaviour
{
    #region VARIABLES
    [Header("Detect Parameters")]
    [SerializeField] private float _detectRadius = 20f;

    private NPCController _currentlySelectedNPC = null;
    
    #endregion


    private void Update()
    {
        StartDetectScan();
    }

    /// <summary>
    /// Function that will trigger the scan of the surroundings of the player
    /// </summary>
    private void StartDetectScan()
    {
        NPCController closestNPC = GetClosestNPC();
        if (closestNPC != _currentlySelectedNPC) 
        {
            _currentlySelectedNPC?.AbandonHighlight();
            _currentlySelectedNPC = closestNPC;
            closestNPC?.TriggerHightlight();
        }
    }

    /// <summary>
    /// Obtain the list of NPC Controllers in range of the detect radius
    /// </summary>
    /// <returns>List of NPC Controllers</returns>
    private List<NPCController> ObtainListNPCControllersInRange()
    {
        Collider[] collidersDetected = Physics.OverlapSphere(gameObject.transform.position, _detectRadius);
        List<NPCController> npcControllerList = new List<NPCController>();
        foreach (Collider collider in collidersDetected)
        {
            if (collider.gameObject.GetComponent<NPCController>() != null)
            {
                npcControllerList.Add(collider.gameObject.GetComponent<NPCController>());
            }
        }
        return npcControllerList;
    }

    /// <summary>
    /// Function that returns the closest NPC Controller detected by the player
    /// </summary>
    /// <returns></returns>
    private NPCController GetClosestNPC()
    {
        List<NPCController> npcDetected = ObtainListNPCControllersInRange();
        if (npcDetected.Count < 1) 
        {
            return null;
        }
        NPCController closestNPC = npcDetected[0];
        float minDistance = Vector3.Distance(closestNPC.gameObject.transform.position, gameObject.transform.position);
        for (int i = 0; i < npcDetected.Count; i++)
        {
            float calcDistance = Vector3.Distance(npcDetected[i].gameObject.transform.position, gameObject.transform.position);
            if (calcDistance < minDistance)
            {
                closestNPC = npcDetected[i];
                minDistance = calcDistance;
            }
        }
        return closestNPC;
    }
}
