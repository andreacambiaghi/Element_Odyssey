using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMenuAnim : MonoBehaviour
{
    public GameObject PanelMenu;

    public void ShowHideMenu()
    {
        Debug.Log("Sono denro ShowHideMenu");
        if(PanelMenu != null)
        {
            Debug.Log("PanelMenu non è null");
            Animator animator = PanelMenu.GetComponent<Animator>();
            if(animator != null)
            {
                Debug.Log("animator non è null");
                bool isOpen = animator.GetBool("show");
                animator.SetBool("show", !isOpen);
            }
        }
    }
}
