using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; // Aggiungi questo se usi TextMeshPro. Se usi UI Text standard, rimuovilo e sostituisci TMP_Text con Text sotto.
using System.Linq; // Potrebbe servire se ElementDataManager non fosse un Singleton statico, ma utile per altre cose

public class HeaderFilter : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject panel; // Il pannello i cui figli devono essere filtrati

    private Transform circle;

    // --- Aggiunta per il filtro ---
    // Potremmo memorizzare il tipo attivo per evitare ricalcoli, ma per ora lo facciamo direttamente nel click
    // private string activeFilterType = null;
    // ---------------------------

    private void Awake()
    {
        circle = transform.Find("Circle");

        if (circle != null)
        {
            circle.gameObject.SetActive(false); // Disattiva all'avvio
        }
        else
        {
            Debug.LogWarning("Nessun figlio chiamato 'Circle' trovato su " + gameObject.name);
        }

        // Assicurati che il panel sia assegnato nell'Inspector
        if (panel == null)
        {
             Debug.LogError("Panel non assegnato nell'Inspector di " + gameObject.name);
        }
         // Assicurati che ElementDataManager esista all'avvio se possibile
        if (ElementDataManager.Instance == null)
        {
            Debug.LogWarning("ElementDataManager.Instance non è ancora disponibile in Awake.");
            // Potrebbe essere normale se viene inizializzato dopo, ma attenzione ai race condition.
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (container == null)
        {
            Debug.LogError("Container non assegnato su " + gameObject.name);
            return;
        }
         if (panel == null)
        {
            Debug.LogError("Panel non assegnato su " + gameObject.name);
            return;
        }
        if (ElementDataManager.Instance == null)
        {
             Debug.LogError("ElementDataManager.Instance non è disponibile!");
             return; // Non possiamo filtrare senza il data manager
        }


        bool isCurrentlyActive = circle != null && circle.gameObject.activeSelf;
        string clickedType = gameObject.name.ToLower(); // Ottieni il 'type' dal nome dell'oggetto cliccato
        string typeToFilter = null; // Tipo da usare per il filtro, null se nessun filtro

        // --- Logica Attivazione/Disattivazione Cerchi ---
        // Prima disattiva tutti i cerchi nel container
        foreach (Transform child in container)
        {
            // Ignora se stesso se necessario, anche se Find non dovrebbe dare problemi
            Transform c = child.Find("Circle");
            if (c != null)
            {
                c.gameObject.SetActive(false);
            }
        }

        // Ora, decidi se attivare il cerchio cliccato
        if (circle != null && !isCurrentlyActive)
        {
            // Se non era attivo, attivalo ora
            circle.gameObject.SetActive(true);
            // E imposta il tipo per il filtro
            typeToFilter = clickedType;
             Debug.Log($"Filtro attivato: {typeToFilter}"); // Log per debug
        }
        else
        {
            // Se era già attivo, cliccandolo di nuovo lo si disattiva (già fatto dal loop sopra)
            // Quindi nessun filtro attivo
            typeToFilter = null;
            Debug.Log("Filtro disattivato."); // Log per debug
        }

        // --- Applica il Filtro al Panel ---
        ApplyFilter(typeToFilter);
    }

    /// <summary>
    /// Applica il filtro agli elementi figli del Panel.
    /// </summary>
    /// <param name="filterType">Il tipo (in minuscolo) da usare per filtrare. Se null o vuoto, mostra tutti gli elementi.</param>
    private void ApplyFilter(string filterType)
    {
        bool noFilter = string.IsNullOrEmpty(filterType);

        // Itera su tutti i figli diretti del panel
        foreach (Transform item in panel.transform)
        {
            // Trova il componente Text (o TMP_Text) nel figlio dell'item
            // Assumiamo che sia un figlio diretto o comunque facile da trovare con GetComponentInChildren
            // Cambia TextMeshProUGUI a Text se usi UI Text standard
            TextMeshProUGUI textComponent = item.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent == null)
            {
                Debug.LogWarning($"Nessun componente TextMeshProUGUI (o Text) trovato nei figli di {item.name} dentro il Panel. L'oggetto verrà { (noFilter ? "mostrato" : "nascosto") }.", item);
                // Se non c'è testo, non può corrispondere a un filtro specifico.
                // Mostralo solo se non c'è nessun filtro attivo.
                item.gameObject.SetActive(noFilter);
                continue; // Passa al prossimo item
            }

            // Se non c'è filtro, attiva l'oggetto e passa al prossimo
            if (noFilter)
            {
                item.gameObject.SetActive(true);
                continue;
            }

            // --- C'è un filtro attivo ---
            string itemText = textComponent.text;
            string elementType = null;

            try{
                 elementType = ElementDataManager.Instance.GetElementsType(itemText);
            }
            catch(System.Exception ex)
            {
                Debug.LogError($"Errore chiamando ElementDataManager.Instance.GetElementsType con testo '{itemText}' da {item.name}: {ex.Message}", item);
                item.gameObject.SetActive(false); // Nascondi in caso di errore nel recupero del tipo
                continue;
            }


            if (elementType == null)
            {
                 Debug.LogWarning($"ElementDataManager.Instance.GetElementsType ha restituito null per il testo '{itemText}' su {item.name}. L'oggetto verrà nascosto.", item);
                 item.gameObject.SetActive(false); // Se non otteniamo un tipo, non può corrispondere
                 continue;
            }

            // Confronta il tipo dell'elemento (convertito in minuscolo) con il tipo del filtro
            bool shouldBeActive = elementType.ToLower() == filterType;
            item.gameObject.SetActive(shouldBeActive);
        }
         // Log finale opzionale
         // Debug.Log($"Filtraggio completato per type: {(noFilter ? "NESSUNO" : filterType)}. Elementi visibili nel panel: {panel.transform.Cast<Transform>().Count(t => t.gameObject.activeSelf)}");
    }
}