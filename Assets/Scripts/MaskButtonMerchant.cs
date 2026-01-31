using UnityEngine;
using UnityEngine.UI;
public class MaskButtonMerchant : MonoBehaviour
{
    public MaskObject maskObject;
    [SerializeField] private Image upperImage;
    [SerializeField] private Image lowerImage;

    private void Start()
    {
        upperImage.sprite = maskObject.GetUpperPart().MaskSprite;
        lowerImage.sprite = maskObject.GetLowerPart().MaskSprite;
    }


    public void MaskClick()
    {
        print("click");
    }
}
