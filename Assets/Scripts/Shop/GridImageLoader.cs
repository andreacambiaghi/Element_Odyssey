using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Runtime.InteropServices;

public class GridImageLoader : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] private GameObject target;  // Il contenitore in cui inserire gli oggetti (ad es. un pannello della UI)
    [SerializeField] private GameObject prefab;  // Il prefab da istanziare, deve avere un componente Image

    [Header("Impostazioni Griglia")]
    [SerializeField] private int itemsPerRow = 3;                   // Numero di oggetti per riga
    [SerializeField] private Vector2 spacing = new Vector2(10f, 10f); // Spazio orizzontale e verticale tra gli oggetti
    [SerializeField] private Vector2 startPosition = new Vector2(0f, 0f); // Posizione iniziale (in coordinate locali del target)

    public void Start()
    {
       SpawnItem();
    }

    public void SpawnItem() {

        // Prima di tutto distrugge tutti gli elementi figli del target, tranne quello che si chiama "Low"
        foreach (Transform child in target.transform)
        {
            if (child.name != "Low")
            {
                Destroy(child.gameObject);
            }
        }

        string[] buyNames = ElementFilesManager.Instance.GetBuyFloorSaveData().ToArray();

        // Carica tutti gli sprite dalla cartella Resources/ImageShop
        Sprite[] sprites = Resources.LoadAll<Sprite>("ImageShop");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("Nessuno sprite trovato in Resources/ImageShop");
            return;
        }

        // Cicla attraverso gli sprite e crea gli oggetti in base al numero di immagini trovate
        int k = 0;
        Debug.Log(buyNames + "Griglia");
        for (int i = 0; i < sprites.Length; i++)
        {
            Debug.Log("Sprite: " + sprites[i].name);
            // Se l'oggetto è già stato acquistato, salta il ciclo
            if (buyNames.Contains(sprites[i].name.Split('_')[0]))
            {
                Debug.Log("Elemento già acquistato: " + sprites[i].name);
                continue;
            }

            // Istanzia il prefab come figlio del target
            GameObject item = Instantiate(prefab, target.transform);

            // Trova il componente Image del prefab
            Image imgComponent = item.GetComponent<Image>();
            if (imgComponent != null)
            {
                imgComponent.sprite = sprites[i]; // Assegna lo sprite trovato
            }
            else
            {
                Debug.LogWarning("Il prefab non possiede un componente Image.");
            }

            // Estrarre il numero dal nome dell'immagine
            string spriteName = sprites[i].name; // ad esempio "nome_10"
            int price = ExtractNumberFromName(spriteName);
            item.name = spriteName; // Imposta il nome dell'oggetto

            // Impostare il valore nel componente "Price"
            Transform priceTransform = item.transform.Find("Price");
            if (priceTransform != null)
            {
                TextMeshProUGUI priceText = priceTransform.GetComponent<TextMeshProUGUI>();
                if (priceText != null)
                {
                    priceText.text = price.ToString(); // Imposta il prezzo nel campo di testo
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

            // Calcola la posizione dell'oggetto nella griglia
            int row = k / itemsPerRow;  // Indice della riga
            int col = k % itemsPerRow;  // Indice della colonna
            k++;

            // Se l'oggetto ha un RectTransform (tipico degli elementi UI), usiamo l'anchoredPosition
            RectTransform rt = item.GetComponent<RectTransform>();
            if (rt != null)
            {
                // Presupponiamo che l'ancoraggio sia in alto a sinistra
                float posX = startPosition.x + col * (rt.sizeDelta.x + spacing.x);
                float posY = startPosition.y - row * (rt.sizeDelta.y + spacing.y);
                rt.anchoredPosition = new Vector2(posX, posY);
            }
            else
            {
                // Se non si tratta di un oggetto UI, posiziona relativamente al target (modifica questo blocco se necessario)
                Vector3 newPos = target.transform.position + new Vector3(col * (spacing.x + 1f), -row * (spacing.y + 1f), 0f);
                item.transform.position = newPos;
            }

        }
    }

    private int ExtractNumberFromName(string name)
    {
        int number = 0;
        string[] parts = name.Split('_');
        if (parts.Length > 1)
        {
            string numberPart = parts[parts.Length - 1]; // Ultima parte "10.png"
            numberPart = System.IO.Path.GetFileNameWithoutExtension(numberPart); // Rimuove ".png"
            
            int.TryParse(numberPart, out number); // Conversione sicura
        }
        return number;
    }
}
