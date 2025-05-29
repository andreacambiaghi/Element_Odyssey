using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SliderMenuAnim : MonoBehaviour
{
    public GameObject PanelMenu;
    private bool isMenuOpen = false;
    private HeaderFilter headerFilter;

    void Awake()
    {
        headerFilter = new();
    }

    public void ShowHideMenu()
    {

        headerFilter.ClearAllFilters();

        if (isMenuOpen)
        {
            StepTutorial.Instance.CloseInventory();
        }
        else
        {
            StepTutorial.Instance.OpenInventory();
        }

        isMenuOpen = !isMenuOpen;
        // Debug.Log("Sono denro ShowHideMenu");
        if (PanelMenu != null)
        {
            // Debug.Log("PanelMenu non è null");
            Animator animator = PanelMenu.GetComponent<Animator>();
            if (animator != null)
            {
                //Debug.Log("animator non è null");
                bool isOpen = animator.GetBool("show");
                animator.SetBool("show", !isOpen);
            }
        }
    }

    public bool GetState() {
        return isMenuOpen;
    }
}
