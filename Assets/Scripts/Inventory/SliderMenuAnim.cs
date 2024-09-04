using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SliderMenuAnim : MonoBehaviour
{
    public GameObject PanelMenu;
    private bool isMenuOpen = false;

    public void ShowHideMenu()
    {
        isMenuOpen = !isMenuOpen;
        // Debug.Log("Sono denro ShowHideMenu");
        if(PanelMenu != null)
        {
            // Debug.Log("PanelMenu non è null");
            Animator animator = PanelMenu.GetComponent<Animator>();
            if(animator != null)
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
