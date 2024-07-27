using UnityEngine;
using UnityEngine.UI;

public class AchievementsImage : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject panel;
    [SerializeField] private int numberOfButtons;
    private string baseID = "Achievement_";

    void Start()
    {
        CreateButtonGrid();
    }

    void CreateButtonGrid()
    {
        int totalButtons = numberOfButtons;

        for (int i = 0; i < totalButtons; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(panel.transform, false);

            newButton.SetActive(true);

            // Add and set unique identifier
            AchievementsIdentifier achievementsIdentifier = newButton.AddComponent<AchievementsIdentifier>();
            achievementsIdentifier.uniqueID = baseID + i.ToString();
        }
    }
}
