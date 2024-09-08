using System.Collections;
using System.Collections.Generic;
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

    public void OpenPanel()
    {
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

        if (slider != null)
        {
            if (menu.GetState())
            {
                Button button = slider.GetComponent<Button>();
                button.onClick.Invoke();
            }
            slider.SetActive(false);
        }

    }
    public void ClosePanel()
    {
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
    }

    public void UpdateAchievements()
    {
        AchievementManager.Instance.UpdateAchievements();
    }
}
