using System;
using UnityEngine;
using UnityEngine.UI;

public class OpenShop : MonoBehaviour
{
    [Header("GameObjects da Disabilitare")]
    [SerializeField] private GameObject slider;
    [SerializeField] private SliderMenuAnim menu;
    [SerializeField] private GameObject volume;
    [SerializeField] private GameObject home;
    [SerializeField] private GameObject elementSelected;
    [SerializeField] private GameObject openShop;
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private GameObject sweepButton;

    [Header("GameObject da Attivare")]
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject shopClose;

    [Header("Buttons")]
    [SerializeField] private GameObject itemList;

    public void Open()
    {
        CheckBuy();

        Debug.Log("Shop aperto");
        
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

        if (sweepButton != null)
        {
            sweepButton.SetActive(false);
        }   

        if (shop != null)
        {
            shop.SetActive(true);
        }

        if (shopClose != null)
        {
            shopClose.SetActive(true);
        }

    }
    public void Close()
    {
        Debug.Log("Shop chiuso");
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

        if (sweepButton != null)
        {
            sweepButton.SetActive(true);
        }

        if (shop != null)
        {
            shop.SetActive(false);
        }

        if (shopClose != null)
        {
            shopClose.SetActive(false);
        }
    }

    private void CheckBuy()
    {
        string[] buyNames = ElementFilesManager.Instance.GetBuyFloorSaveData().ToArray();
        Debug.Log("Elementi acquistati: " + string.Join(", ", buyNames));

        //gridImageLoader.CreateGrid();

        // // Ciclo sui figli di itemList e se il nome dell'elemento è contenuto in buyNames, disabilito il Lock
        // foreach (Transform child in itemList.transform)
        // {
        //     Debug.Log("Nome figlio: " + child.name.Split('_')[0]);
        //     if (Array.Exists(buyNames, element => element == child.name.Split('_')[0]))
        //     {
        //         child.Find("Lock")?.gameObject.SetActive(false);
        //     }
        // }
    }
}
