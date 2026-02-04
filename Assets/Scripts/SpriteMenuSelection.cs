using UnityEngine;
using UnityEngine.UI;

public class SpriteMenuSelection : MonoBehaviour
{
    [SerializeField] private Image mainImage;
    [SerializeField] private Image overlayImage;

    public Image MainImage => mainImage;
    public Image OverlayImage => overlayImage;

    public void SetMainSprite(Sprite sprite)
    {
        if (mainImage != null)
            mainImage.sprite = sprite;
    }

    public void SetOverlaySprite(Sprite sprite)
    {
        if (overlayImage != null)
            overlayImage.sprite = sprite;
    }

    public void SetOverlayVisible(bool visible)
    {
        if (overlayImage != null)
            overlayImage.enabled = visible;
    }
}
