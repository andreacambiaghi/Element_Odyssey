using System;
using UnityEngine;
using UnityEngine.UI;

public class OpenFloor : MonoBehaviour
{
    [Header("GameObjects da Disabilitare")]
    [SerializeField] private GameObject slider;
    [SerializeField] private SliderMenuAnim menu;
    [SerializeField] private GameObject volume;
    [SerializeField] private GameObject home;
    [SerializeField] private GameObject elementSelected;
    [SerializeField] private GameObject openShop;
    [SerializeField] private GameObject inventoryButton;

    [Header("GameObject da Attivare")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject invClose;

    public void Open()
    {
        Debug.Log("Inventory aperto");
        
        if (slider != null)
        {
            if (menu.GetState())
            {
                Button button = slider.GetComponent<Button>();
                button.onClick.Invoke();
            }
            slider.SetActive(false);
        }

        if (volume != null)
        {
            volume.SetActive(false);
        }

        if (home != null)
        {
            home.SetActive(false);
        }

        if (elementSelected != null)
        {
            elementSelected.SetActive(false);
        }

        if (openShop != null)
        {
            openShop.SetActive(false);
        }

        if (inventoryButton != null)
        {
            inventoryButton.SetActive(false);
        }

        if (inventory != null)
        {
            inventory.SetActive(true);
        }

        if (invClose != null)
        {
            invClose.SetActive(true);
        }

    }
    public void Close()
    {
        Debug.Log("Inventory chiuso");
        if (slider != null)
        {
            slider.SetActive(true);
        }

        if (volume != null)
        {
            volume.SetActive(true);
        }

        if (home != null)
        {
            home.SetActive(true);
        }

        if (elementSelected != null)
        {
            elementSelected.SetActive(true);
        }

        if (openShop != null)
        {
            openShop.SetActive(true);
        }

        if (inventoryButton != null)
        {
            inventoryButton.SetActive(true);
        }

        if (inventory != null)
        {
            inventory.SetActive(false);
        }

        if (invClose != null)
        {
            invClose.SetActive(false);
        }
    }
}
