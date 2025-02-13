using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class CreateElementForInventory : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] private GameObject target; // Contenitore UI
    [SerializeField] private GameObject prefab; // Prefab con Image e TextMeshProUGUI

    [Header("Impostazioni Griglia")]
    [SerializeField] private int itemsPerRow = 3;
    [SerializeField] private Vector2 spacing = new Vector2(10f, 10f);
    [SerializeField] private Vector2 startPosition = new Vector2(0f, 0f);

    void Start()
    {
        // Carica i nomi dei file da possessed.txt
        string[] allowedNames = LoadPossessedNames();
        if (allowedNames == null || allowedNames.Length == 0)
        {
            Debug.LogWarning("possessed.txt è vuoto o non trovato.");
            return;
        }

        // Carica tutti gli sprite dalla cartella Resources/Floor/Icon
        Sprite[] allSprites = Resources.LoadAll<Sprite>("Floor/Icon");
        if (allSprites == null || allSprites.Length == 0)
        {
            Debug.LogWarning("Nessuno sprite trovato in Resources/Floor/Icon");
            return;
        }

        // Filtra gli sprite in base ai nomi trovati in possessed.txt
        Sprite[] filteredSprites = allSprites.Where(sprite => allowedNames.Contains(sprite.name)).ToArray();
        if (filteredSprites.Length == 0)
        {
            Debug.LogWarning("Nessuno sprite corrisponde ai nomi in possessed.txt");
            return;
        }

        // Creazione degli elementi in base agli sprite filtrati
        for (int i = 0; i < filteredSprites.Length; i++)
        {
            GameObject item = Instantiate(prefab, target.transform);
            item.name = "InventoryItem_" + i;

            // Assegna lo sprite
            Image imgComponent = item.GetComponent<Image>();
            if (imgComponent != null)
            {
                imgComponent.sprite = filteredSprites[i];
            }
            else
            {
                Debug.LogWarning("Il prefab non possiede un componente Image.");
            }

            // Estrai il prezzo dal nome dell'immagine
            int price = ExtractNumberFromName(filteredSprites[i].name);

            // Imposta il prezzo nel componente UI
            Transform priceTransform = item.transform.Find("Price");
            if (priceTransform != null)
            {
                TextMeshProUGUI priceText = priceTransform.GetComponent<TextMeshProUGUI>();
                if (priceText != null)
                {
                    priceText.text = price.ToString();
                }
                else
                {
                    Debug.LogWarning("Il figlio 'Price' non ha un componente TextMeshProUGUI.");
                }
            }
            else
            {
                Debug.LogWarning("Non è stato trovato il figlio 'Price' nel prefab.");
            }

            // Calcola la posizione nella griglia
            int row = i / itemsPerRow;
            int col = i % itemsPerRow;
            RectTransform rt = item.GetComponent<RectTransform>();
            if (rt != null)
            {
                float posX = startPosition.x + col * (rt.sizeDelta.x + spacing.x);
                float posY = startPosition.y - row * (rt.sizeDelta.y + spacing.y);
                rt.anchoredPosition = new Vector2(posX, posY);
            }
        }
    }

    private string[] LoadPossessedNames()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Floor/possessed");
        return textAsset != null ? textAsset.text.Split('\n').Select(line => line.Trim()).ToArray() : null;
    }

    private int ExtractNumberFromName(string name)
    {
        int number = 0;
        string[] parts = name.Split('_');
        if (parts.Length > 1)
        {
            string numberPart = System.IO.Path.GetFileNameWithoutExtension(parts[parts.Length - 1]);
            int.TryParse(numberPart, out number);
        }
        return number;
    }
}
