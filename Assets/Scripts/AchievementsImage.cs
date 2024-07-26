using UnityEngine;
using UnityEngine.UI;

public class AchievementsImage : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    public Sprite[] buttonImages;
    public GameObject panel;

    void Start()
    {
        CreateButtonGrid();
    }

    void CreateButtonGrid()
    {
        int totalButtons = buttonImages.Length;

        for (int i = 0; i < totalButtons; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(panel.transform, false);

            newButton.SetActive(true);

            Image buttonImage = newButton.GetComponent<Image>();
            buttonImage.sprite = buttonImages[i];
        }
    }
}
