using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopItem : MonoBehaviour
{
    private TextMeshProUGUI balanceText;  // Riferimento al saldo, trovato dinamicamente
    private TextMeshProUGUI priceText;   // Riferimento al prezzo nell'oggetto figlio "Price"
    private GameObject lockObject;       // Riferimento all'oggetto figlio "Lock"

    private void Awake()
    {
        // Trova il prezzo e l'oggetto "Lock" tra i figli
        priceText = transform.Find("Price").GetComponent<TextMeshProUGUI>();
        lockObject = transform.Find("Lock")?.gameObject;

        // Trova il riferimento al saldo nell'oggetto Balance-txt
        balanceText = GameObject.Find("Shop/Balance/Balance-txt")?.GetComponent<TextMeshProUGUI>();

        // Aggiunge il listener al pulsante del GameObject stesso
        GetComponent<Button>().onClick.AddListener(OnItemClicked);
    }

    private void OnItemClicked()
    {
        Debug.Log("Hai cliccato su: " + gameObject.GetComponent<Image>().sprite.texture.name.Split('_')[0]);
        if (priceText == null || balanceText == null)
        {
            Debug.Log("Prezzo o saldo mancanti nello ShopItem: " + gameObject.name);
            return;
        }

        // Leggi il prezzo e il saldo attuale
        int itemPrice = int.Parse(priceText.text);
        int currentBalance = ElementFilesManager.Instance.GetBalance();

        if (currentBalance >= itemPrice)
        {
            // Aggiorna il saldo
            currentBalance -= itemPrice;
            ElementFilesManager.Instance.SetBalance(currentBalance);
            balanceText.text = currentBalance.ToString();

            // Disattiva il "Lock" se esiste
            if (lockObject != null)
            {
                lockObject.SetActive(false);
            }

            // ElementFilesManager.Instance.SaveBalance(currentBalance); //TODO

            ElementFilesManager.Instance.SaveBuyFloor(gameObject.GetComponent<Image>().sprite.texture.name.Split('_')[0]);

        }
        else
        {
            Debug.Log("Non hai abbastanza saldo per acquistare questo oggetto.");
            StartCoroutine(ShowLowNotification());  // Attiva la notifica "Low"
        }
    }

    // Coroutine per attivare l'oggetto "Low" per 2 secondi
    private IEnumerator ShowLowNotification()
    {
        // Ottieni il padre dell'oggetto corrente e cerca l'oggetto "Low"
        Transform parentTransform = transform.parent;
        GameObject lowNotification = parentTransform?.Find("Low")?.gameObject;

        if (lowNotification != null)
        {
            // Attiva l'oggetto "Low"
            lowNotification.SetActive(true);

            // Attendi per 2 secondi
            yield return new WaitForSeconds(2f);

            // Disattiva l'oggetto "Low" dopo 2 secondi
            lowNotification.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Oggetto 'Low' non trovato nel padre.");
        }
    }
}
