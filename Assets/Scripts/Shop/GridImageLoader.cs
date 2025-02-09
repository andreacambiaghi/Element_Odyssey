using UnityEngine;
using UnityEngine.UI;

public class GridImageLoader : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] private GameObject target;  // Il contenitore in cui inserire gli oggetti (ad es. un pannello della UI)
    [SerializeField] private GameObject prefab;  // Il prefab da istanziare, deve avere un componente Image

    [Header("Impostazioni Griglia")]
    [SerializeField] private int itemsPerRow = 3;                   // Numero di oggetti per riga
    [SerializeField] private Vector2 spacing = new Vector2(10f, 10f); // Spazio orizzontale e verticale tra gli oggetti
    [SerializeField] private Vector2 startPosition = new Vector2(0f, 0f); // Posizione iniziale (in coordinate locali del target)

    void Start()
    {
        // Carica tutti gli sprite dalla cartella Resources/ImageShop
        Sprite[] sprites = Resources.LoadAll<Sprite>("ImageShop");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("Nessuno sprite trovato in Resources/ImageShop");
            return;
        }

        // Cicla attraverso gli sprite e crea gli oggetti in base al numero di immagini trovate
        for (int i = 0; i < sprites.Length; i++)
        {
            // Istanzia il prefab come figlio del target
            GameObject item = Instantiate(prefab, target.transform);
            item.name = "ImageItem_" + i;

            // Trova il componente Image del prefab (non il figlio Image)
            Image imgComponent = item.GetComponent<Image>();
            if (imgComponent != null)
            {
                imgComponent.sprite = sprites[i]; // Assegna lo sprite trovato
            }
            else
            {
                Debug.LogWarning("Il prefab non possiede un componente Image.");
            }

            // Calcola la posizione dell'oggetto nella griglia
            int row = i / itemsPerRow;  // Indice della riga
            int col = i % itemsPerRow;  // Indice della colonna

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
}
