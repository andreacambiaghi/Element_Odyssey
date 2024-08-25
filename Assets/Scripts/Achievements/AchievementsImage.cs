using UnityEngine;
using UnityEngine.UI;

public class AchievementsImage : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject panel;
    [SerializeField] private int numberOfButtons;
    private AchievementIdentifier identifier;
    private AchievementsInfo _info = new();

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
            if (progressBar != null && i < _info.GetMaxProgressLength())
            {
                progressBar.maxProgress = _info.GetMaxProgress(i);
            }
        }
    }

    public AchievementIdentifier GetIdentifier()
    {
        return identifier;
    }
}
