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
    [SerializeField] private GameObject sweep;
    [SerializeField] private GameObject coin;

    [Header("GameObject da Attivare")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject invClose;

    [Header("Floors")]
    [SerializeField] private GameObject floors;

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

        if (sweep != null)
        {
            sweep.SetActive(false);
        }

        if (coin != null)
        {
            coin.SetActive(false);
        }

        if (inventory != null)
        {
            inventory.SetActive(true);
        }

        if (invClose != null)
        {
            invClose.SetActive(true);
        }

        // // prendo i figli del floor in una lista e stampo quanti sono 
        // Transform[] children = floors.GetComponentsInChildren<Transform>();
        // Debug.Log("Numero di figli: " + children.Length);
        // // stampo i nomi dei figli
        // foreach (Transform child in children)
        // {
        //     Debug.Log("Nome figlio: " + child.name);
        // }
        // Debug.Log("MUCCAMUCCA: " + ElementFilesManager.Instance.getVillageSaveData().floor);
        // // scorro i figli e se un figlio si chiama mario lo attivo
        // foreach (Transform child in children)
        // {
        //     if (child.name == ElementFilesManager.Instance.getVillageSaveData().floor)
        //     {
        //         child.gameObject.SetActive(true);
        //     }
        // }

        Debug.Log("Piano selezionato: " + ElementFilesManager.Instance.getVillageSaveData().floor);
        // stampo i figli del floor
        foreach (Transform child in floors.transform)
        {
            Debug.Log("Nome figlio: " + child.name);
            if (child.name == ElementFilesManager.Instance.getVillageSaveData().floor)
            {
                // prendo il suo unico figlio del figlio e lo attivo
                foreach (Transform child2 in child)
                {
                    child2.gameObject.SetActive(true);
                }
                
            }
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

        if (sweep != null)
        {
            sweep.SetActive(true);
        }

        if (coin != null)
        {
            coin.SetActive(true);
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
