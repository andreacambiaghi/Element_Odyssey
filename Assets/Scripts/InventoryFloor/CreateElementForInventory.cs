using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class CreateElementForInventory : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] private GameObject target;  // Il contenitore in cui inserire gli oggetti
    [SerializeField] private GameObject prefab;  // Il prefab da istanziare, deve avere un componente Image

    [Header("Impostazioni Griglia")]
    [SerializeField] private int itemsPerRow = 3;                   // Numero di oggetti per riga
    [SerializeField] private Vector2 spacing = new Vector2(10f, 10f); // Spazio tra gli oggetti
    [SerializeField] private Vector2 startPosition = new Vector2(0f, 0f); // Posizione iniziale

    void Start()
    {
        // Carica la lista dei nomi accettati da "possessed.txt"
        string[] validNames = LoadPossessedNames();
        if (validNames == null || validNames.Length == 0)
        {
            Debug.LogWarning("Il file possessed.txt è vuoto o non esiste.");
            return;
        }

        // Carica tutti gli sprite dalla cartella "Resources/Floor/Icon"
        Sprite[] sprites = Resources.LoadAll<Sprite>("Floor/Icon");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("Nessuno sprite trovato in Resources/Floor/Icon");
            return;
        }

        // Filtra gli sprite per includere solo quelli presenti in "possessed.txt"
        sprites = sprites.Where(s => validNames.Contains(s.name)).ToArray();
        if (sprites.Length == 0)
        {
            Debug.LogWarning("Nessuna immagine corrisponde ai nomi in possessed.txt");
            return;
        }

        // Genera gli oggetti nella griglia
        for (int i = 0; i < sprites.Length; i++)
        {
            GameObject item = Instantiate(prefab, target.transform);
            item.name = "InventoryItem_" + i;

            Image imgComponent = item.GetComponent<Image>();
            if (imgComponent != null)
            {
                imgComponent.sprite = sprites[i];
            }
            else
            {
                Debug.LogWarning("Il prefab non possiede un componente Image.");
            }

            // Posizionamento nella griglia
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
        TextAsset file = Resources.Load<TextAsset>("Floor/possessed");
        if (file != null)
        {
            return file.text.Split('\n').Select(name => name.Trim()).Where(name => !string.IsNullOrEmpty(name)).ToArray();
        }
        return new string[0];
    }
}
