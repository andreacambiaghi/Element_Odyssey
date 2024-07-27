using UnityEngine;
using UnityEngine.UI;

public class AchievementsImage : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject panel;
    [SerializeField] private int numberOfButtons;
    public AchievementIdentifier identifier;
    private int[] maxProgressValues = { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

    void Awake()
    {
        CreateButtonGrid();
    }

    void CreateButtonGrid()
    {
        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(panel.transform, false);

            newButton.SetActive(true);

            identifier = newButton.AddComponent<AchievementIdentifier>();
            identifier.uniqueID = i;

            ProgressBar progressBar = newButton.GetComponent<ProgressBar>();
            if (progressBar != null && i < maxProgressValues.Length)
            {
                progressBar.maxProgress = maxProgressValues[i];
            }
        }
    }
}
