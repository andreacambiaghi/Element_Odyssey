using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCoin : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    public void Click()
    {
        // quando deve attivarsi aspetta 1 secondo, quando deve disattivarsi invece fa subito
        if (!coin.activeSelf)
        {
            StartCoroutine(EnableCoin());
        }
        else
        {
            coin.SetActive(false);
        }

    }

    private IEnumerator EnableCoin()
    {
        yield return new WaitForSeconds(0.5f);
        coin.SetActive(true);
    }
}
