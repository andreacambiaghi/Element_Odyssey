using UnityEngine;
using UnityEngine.UI;

public class CardsButton : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject openButton;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject slider;
    [SerializeField] private SliderMenuAnim menu;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private GameObject soundButton;
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject achivementsButton;

    public void OpenPanel()
    {
        StepTutorial.Instance.ShowCardBoard();
        
        // Debug.Log("OpenPaneeeeeeeeeeeeeeeeeeeeeeel");
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }

        if (openButton != null)
        {
            openButton.SetActive(false);
        }

        if (closeButton != null)
        {
            closeButton.SetActive(true);
        }

        if (homeButton != null)
        {
            homeButton.SetActive(false);
        }

        if (soundButton != null)
        {
            soundButton.SetActive(false);
        }

        if (achivementsButton != null)
        {
            achivementsButton.SetActive(false);
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
        StepTutorial.Instance.CloseCardBoard();

        // Debug.Log("ClosePaneeeeeeeeeeeeeeeeeeeeeeel");
        if (panel != null)
        {
            panel.SetActive(false);
        }

        if (openButton != null)
        {
            openButton.SetActive(true);
        }

        if (closeButton != null)
        {
            closeButton.SetActive(false);
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

        if (achivementsButton != null)
        {
            achivementsButton.SetActive(true);
        }

        if (coin != null)
        {
            coin.SetActive(true);
        }
    }
}
