using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public GameObject panel;
    public GameObject openButton;
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
    }
}
