using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        if (balanceText == null)
        {
            Debug.LogError("Impossibile trovare il riferimento al saldo (Balance-txt).");
        }

        // Aggiunge il listener al pulsante del GameObject stesso
        GetComponent<Button>().onClick.AddListener(OnItemClicked);
    }

    private void OnItemClicked()
    {
        if (priceText == null || balanceText == null)
        {
            Debug.LogError("Prezzo o saldo mancanti nello ShopItem: " + gameObject.name);
            return;
        }

        // Leggi il prezzo e il saldo attuale
        int itemPrice = int.Parse(priceText.text);
        int currentBalance = int.Parse(balanceText.text);

        if (currentBalance >= itemPrice)
        {
            // Aggiorna il saldo
            currentBalance -= itemPrice;
            balanceText.text = currentBalance.ToString();

            // Disattiva il "Lock" se esiste
            if (lockObject != null)
            {
                lockObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("Non hai abbastanza saldo per acquistare questo oggetto.");
        }
    }
}
