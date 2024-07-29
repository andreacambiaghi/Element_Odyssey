using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject openButton;
    [SerializeField] private GameObject slider;

    void Start()
    {
        slider = GameObject.Find("Slider");
    }
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

        if (slider != null)
        {
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

        if (slider != null)
        {
            slider.SetActive(true);
        }
    }
}
