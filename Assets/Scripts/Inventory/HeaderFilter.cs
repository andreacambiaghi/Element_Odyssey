using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; // Aggiungi questo se usi TextMeshPro. Se usi UI Text standard, rimuovilo e sostituisci TMP_Text con Text sotto.
using System.Linq; // Potrebbe servire se ElementDataManager non fosse un Singleton statico, ma utile per altre cose

public class HeaderFilter : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject panel; // Il pannello i cui figli devono essere filtrati

    private Transform circle;

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

        if (panel == null)
        {
             Debug.LogError("Panel non assegnato nell'Inspector di " + gameObject.name);
        }
        if (ElementDataManager.Instance == null)
        {
            Debug.LogWarning("ElementDataManager.Instance non è ancora disponibile in Awake.");
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
             return; 
        }


        bool isCurrentlyActive = circle != null && circle.gameObject.activeSelf;
        string clickedType = gameObject.name.ToLower();
        string typeToFilter = null;


        foreach (Transform child in container)
        {
            Transform c = child.Find("Circle");
            if (c != null)
            {
                c.gameObject.SetActive(false);
            }
        }

        if (circle != null && !isCurrentlyActive)
        {
            circle.gameObject.SetActive(true);
            typeToFilter = clickedType;
             Debug.Log($"Filtro attivato: {typeToFilter}");
        }
        else
        {
            typeToFilter = null;
            Debug.Log("Filtro disattivato.");
        }

        ApplyFilter(typeToFilter);
    }

     private void ApplyFilter(string filterType)
    {
        bool noFilter = string.IsNullOrEmpty(filterType);

        foreach (Transform item in panel.transform)
        {
        
            TextMeshProUGUI textComponent = item.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent == null)
            {
                Debug.LogWarning($"Nessun componente TextMeshProUGUI (o Text) trovato nei figli di {item.name} dentro il Panel. L'oggetto verrà { (noFilter ? "mostrato" : "nascosto") }.", item);
                item.gameObject.SetActive(noFilter);
                continue;
            }

            if (noFilter)
            {
                item.gameObject.SetActive(true);
                continue;
            }

            string itemText = textComponent.text;
            string elementType = null;

            try{
                 elementType = ElementDataManager.Instance.GetElementsType(itemText);
            }
            catch(System.Exception ex)
            {
                Debug.LogError($"Errore chiamando ElementDataManager.Instance.GetElementsType con testo '{itemText}' da {item.name}: {ex.Message}", item);
                item.gameObject.SetActive(false);
                continue;
            }


            if (elementType == null)
            {
                 Debug.LogWarning($"ElementDataManager.Instance.GetElementsType ha restituito null per il testo '{itemText}' su {item.name}. L'oggetto verrà nascosto.", item);
                 item.gameObject.SetActive(false);
                 continue;
            }

            bool shouldBeActive = elementType.ToLower() == filterType;
            item.gameObject.SetActive(shouldBeActive);
        }
         
        Debug.Log($"Filtraggio completato per type: {(noFilter ? "NESSUNO" : filterType)}. Elementi visibili nel panel: {panel.transform.Cast<Transform>().Count(t => t.gameObject.activeSelf)}");
    }
}