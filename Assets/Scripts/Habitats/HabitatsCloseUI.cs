using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitatsCloseUI : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    public void CloseUI()
    {
        if (UI != null)
        {
            UI.SetActive(false);
            VillagePlaneManager.Instance.menuOpen = false;
        }

        VillagePlaneManager.Instance.DeselectSelectedGameObject();
    }
}
