using UnityEngine;
using UnityEngine.UI;

public class AchievementsMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject openButton;
    [SerializeField] private GameObject slider;
    [SerializeField] private SliderMenuAnim menu;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private GameObject soundButton;
    [SerializeField] private GameObject elementSelected;
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject yourCardsButton;

    public void OpenPanel()
    {

        StepTutorial.Instance.ShowAchievement();

        // Debug.Log("OpenPaneeeeeeeeeeeeeeeeeeeeeeel");
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }

        if (openButton != null)
        {
            openButton.SetActive(false);
        }

        if (homeButton != null)
        {
            homeButton.SetActive(false);
        }

        if (soundButton != null)
        {
            soundButton.SetActive(false);
        }

        if (GameModeManager.Instance.GameMode == "VirtualPlane" && elementSelected != null)
        {
            elementSelected.SetActive(false);
        }

        if (GameModeManager.Instance.GameMode == "CreateMarker" && yourCardsButton != null)
        {
            yourCardsButton.SetActive(false);
        }

        if (slider != null)
        {
            if (menu.GetState())
            {
                Button button = slider.GetComponent<Button>();
                button.onClick.Invoke();
            }
            slider.SetActive(false);
        }
        
        if (coin != null)
        {
            coin.SetActive(false);
        }

    }
    public void ClosePanel()
    {
        StepTutorial.Instance.CloseAchievement();

        // Debug.Log("ClosePaneeeeeeeeeeeeeeeeeeeeeeel");
        if (panel != null)
        {
            panel.SetActive(false);
        }

        if (openButton != null)
        {
            openButton.SetActive(true);
        }

        if (homeButton != null)
        {
            homeButton.SetActive(true);
        }

        if (soundButton != null)
        {
            soundButton.SetActive(true);
        }

        if (slider != null)
        {
            slider.SetActive(true);
        }

        if (GameModeManager.Instance.GameMode == "VirtualPlane" && elementSelected != null)
        {
            elementSelected.SetActive(true);
        }

        if (GameModeManager.Instance.GameMode == "CreateMarker" && yourCardsButton != null)
        {
            yourCardsButton.SetActive(true);
        }

        if (coin != null)
        {
            coin.SetActive(true);
        }
    }

    public void UpdateAchievements()
    {
        AchievementManager.Instance.UpdateAchievements();
    }
}
