using UnityEngine;
using UnityEngine.UI;

public class ImageGallery : MonoBehaviour
{
    [SerializeField] private Image displayImage;
    private Sprite[] images;
    private int currentIndex = 0;

    void Start()
    {
        images = Resources.LoadAll<Sprite>("Tutorial");
        displayImage.sprite = images[currentIndex];
    }

    public void NextImage()
    {
        if (images.Length == 0) return;

        currentIndex = (currentIndex + 1) % images.Length;
        displayImage.sprite = images[currentIndex];
    }

    public void PreviousImage()
    {
        if (images.Length == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = images.Length - 1;
        }
        displayImage.sprite = images[currentIndex];
    }
}
