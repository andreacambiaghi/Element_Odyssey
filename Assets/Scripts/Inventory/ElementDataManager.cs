using UnityEngine;
using System.Collections.Generic;
using System.IO; // Anche se usiamo Resources, è buona norma averlo per System.Serializable

// Metti questo script su un GameObject vuoto nella tua scena iniziale,
// oppure lascia che il Singleton lo crei automaticamente la prima volta che viene richiesto.
public class ElementDataManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    private static ElementDataManager _instance;
    public static ElementDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ElementDataManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("ElementDataManager");
                    _instance = singletonObject.AddComponent<ElementDataManager>();
                    Debug.Log("ElementDataManager instance created.");
                }
            }
            return _instance;
        }
    }

    // --- Data Structures ---
    [System.Serializable]
    private class ElementType 
    {
        public string element;
        public string type;
    }

    [System.Serializable]
    private class ElementTypeList 
    {
        public List<ElementType> elements;
    }

    private Dictionary<string, string> elementTypesMap = null;
    private bool isDataLoaded = false;
    private const string JsonResourcePath = "elementsType"; // Nome del file in Resources (SENZA estensione .json)

    private void Awake()
    {
        // Gestione classica del Singleton per evitare duplicati
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Rende il Singleton persistente tra le scene
            LoadElementData(); // Carica i dati all'avvio
        }
        else if (_instance != this)
        {
            // Se esiste già un'istanza, distruggi questo duplicato
            Debug.LogWarning("Duplicate ElementDataManager instance detected. Destroying the new one.");
            Destroy(gameObject);
        }
    }

    private void LoadElementData()
    {
        if (isDataLoaded) return; // Non ricaricare se già fatto

        elementTypesMap = new Dictionary<string, string>(); // Inizializza il dizionario

        // Carica il file come TextAsset dalla cartella Resources
        TextAsset jsonTextAsset = Resources.Load<TextAsset>(JsonResourcePath);

        if (jsonTextAsset == null)
        {
            Debug.LogError($"ElementDataManager Error: Failed to load JSON file from Resources. " +
                           $"Make sure '{JsonResourcePath}.json' exists directly inside a 'Resources' folder.");
            isDataLoaded = true; // Segna come 'caricato' (anche se fallito) per evitare tentativi continui
            return;
        }

        string jsonString = jsonTextAsset.text;

        string wrappedJson = jsonString;
        if (!string.IsNullOrWhiteSpace(jsonString) && jsonString.TrimStart().StartsWith("["))
        {
            wrappedJson = $"{{\"elements\":{jsonString}}}";
        }
        else if (string.IsNullOrWhiteSpace(jsonString) || !jsonString.TrimStart().StartsWith("{"))
        {
             Debug.LogError($"ElementDataManager Error: The content of '{JsonResourcePath}.json' doesn't seem to be a valid JSON array or the expected wrapper object.");
             isDataLoaded = true;
             return;
        }

        // Esegui il parsing del JSON
        try
        {
            ElementTypeList loadedData = JsonUtility.FromJson<ElementTypeList>(wrappedJson);

            if (loadedData != null && loadedData.elements != null)
            {
                foreach (ElementType item in loadedData.elements)
                {
                    // Aggiungi al dizionario, gestendo eventuali duplicati o elementi vuoti
                    if (!string.IsNullOrEmpty(item.element))
                    {
                        if (!elementTypesMap.ContainsKey(item.element))
                        {
                            elementTypesMap.Add(item.element, item.type);
                        }
                        else
                        {
                            Debug.LogWarning($"ElementDataManager Warning: Duplicate element name '{item.element}' found in '{JsonResourcePath}.json'. Using the first occurrence.");
                        }
                    }
                    else
                    {
                         Debug.LogWarning($"ElementDataManager Warning: Found an entry with an empty element name in '{JsonResourcePath}.json'. Skipping.");
                    }
                }
                Debug.Log($"ElementDataManager: Successfully loaded {elementTypesMap.Count} element types from '{JsonResourcePath}.json'.");
            }
            else
            {
                Debug.LogError($"ElementDataManager Error: Failed to parse JSON structure correctly from '{JsonResourcePath}.json' even after wrapping. Check the JSON content. JSON processed: {wrappedJson}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"ElementDataManager Error: Exception while parsing JSON from '{JsonResourcePath}.json'. Error: {ex.Message}\nJSON processed: {wrappedJson}");
        }

        isDataLoaded = true;
    }

    public string GetElementsType(string elementName)
    {
        if (!isDataLoaded || elementTypesMap == null)
        {
            Debug.LogError("ElementDataManager: Attempted to get element type, but data is not loaded or the map is null. Check for earlier loading errors.");
            return null;
        }

        if (elementTypesMap.TryGetValue(elementName, out string type))
        {
            return type;
        }
        else
        {
            Debug.LogWarning($"ElementDataManager Warning: Element '{elementName}' not found in the loaded data. Returning null.");
            return null;
        }
    }
}