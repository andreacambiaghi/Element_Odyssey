using UnityEngine;
using UnityEngine.UI;

public class ImageGallery : MonoBehaviour
{
    [SerializeField] private Image displayImage;
    [SerializeField] private GameObject TutorialPanel;
    [SerializeField] private GameObject slider;
    [SerializeField] private GameObject soundButton;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private GameObject achievementButton;
    [SerializeField] private GameObject coin;
    private Sprite[] images;
    private int currentIndex = 0;

    void Start()
    {
        images = Resources.LoadAll<Sprite>("Tutorial");
        displayImage.sprite = images[currentIndex];
        slider.SetActive(false);
        soundButton.SetActive(false);
        homeButton.SetActive(false);
        achievementButton.SetActive(false);
        coin.SetActive(false);
    }

    public void NextImage()
    {
        if (images.Length == 0) return;

        if(currentIndex == images.Length - 1)
        {
            TutorialPanel.SetActive(false);
            slider.SetActive(true);
            soundButton.SetActive(true);
            homeButton.SetActive(true);
            achievementButton.SetActive(true);
            coin.SetActive(true);
        }

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
