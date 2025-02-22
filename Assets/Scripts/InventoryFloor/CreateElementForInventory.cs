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

    void OnEnable()
    {
        // Distrugge tutti i figli di target
        foreach (Transform child in target.transform)
        {
            Destroy(child.gameObject);
        }
        
        // Carica la lista dei nomi accettati da "initialFloor.txt" e "buyFloor.txt"
        string[] initialNames = LoadFloorNames("initialFloor");
        string[] buyNames = ElementFilesManager.Instance.GetBuyFloorSaveData().ToArray();

        // Carica tutti gli sprite dalla cartella "Resources/Floor/Icon"
        Sprite[] sprites = Resources.LoadAll<Sprite>("Floor/Icon");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("Nessuno sprite trovato in Resources/Floor/Icon");
            return;
        }

        // Ordina gli sprite in modo che quelli di initialNames vengano prima di quelli di buyNames
        var orderedSprites = sprites
            .Where(s => initialNames.Contains(s.name))
            .Concat(sprites.Where(s => buyNames.Contains(s.name)))
            .ToArray();

        if (orderedSprites.Length == 0)
        {
            Debug.LogWarning("Nessuna immagine corrisponde ai nomi in initialFloor.txt e buyFloor.txt");
            return;
        }

        // Genera gli oggetti nella griglia
        for (int i = 0; i < orderedSprites.Length; i++)
        {
            GameObject item = Instantiate(prefab, target.transform);
            item.name = orderedSprites[i].name.Split('_')[0]; // Nome dell'oggetto senza il numero

            Image imgComponent = item.GetComponent<Image>();
            if (imgComponent != null)
            {
                imgComponent.sprite = orderedSprites[i];
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

    private string[] LoadFloorNames(string filePath)
    {
        TextAsset file = Resources.Load<TextAsset>(filePath);
        if (file != null)
        {
            Debug.Log("Floor caricati: " + file.text);
            return file.text.Split('\n').Select(name => name.Trim()).Where(name => !string.IsNullOrEmpty(name)).ToArray();
        }

        return new string[0];
    }
}
