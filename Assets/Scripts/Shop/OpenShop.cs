using System;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    [Header("GameObjects da Disabilitare")]
    [SerializeField] private GameObject slider;
    [SerializeField] private GameObject volume;
    [SerializeField] private GameObject home;

    [Header("GameObject da Attivare")]
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject shopClose;

    public void Click()
    {
        Debug.LogError("Shop aperto");
        if (slider != null)
        {
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

        if (shop != null)
        {
            shop.SetActive(true);
        }

        if (shopClose != null)
        {
            shopClose.SetActive(true);
        }
    }
}
