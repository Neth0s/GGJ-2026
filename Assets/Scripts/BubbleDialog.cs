using TMPro;
using UnityEngine;

public class BubbleDialog : MonoBehaviour
{
    public bool canDisplayNewBubble = false;

   public void FinishBubbleAnim()
    {
        canDisplayNewBubble = true;
    }

}
