using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ElementFilesManager : MonoBehaviour
{
    public static ElementFilesManager Instance;

    private List<string> initialElements = null;

    private List<string> foundElements = null;

    private Dictionary<ElementPair, string> elementAssociations = null;

    private void Awake()
    {
        Debug.Log("ElementFilesManager init");
        if(Instance != null && Instance != this)
        {
           Destroy(this.gameObject);
           return;
        }
        else
        {
            Instance = this;
            Initialize();
        }
    }

    private static ElementFilesManager GetInstance(){
        if(Instance == null){
            Instance = new ElementFilesManager();
        }

        return Instance;
    }


    private void Initialize()
    {

        UpdateAll();
        // initialElements = GetInitialElements();
        // foundElements = GetFoundElements();
        // elementAssociations = GetElementAssociations();

        Debug.Log("-----------------");
        Debug.Log("ElementFilesManager initialized");
        Debug.Log("InitialElements: " + initialElements.Count);
        Debug.Log("FoundElements: " + foundElements.Count);
        Debug.Log("ElementAssociations: " + elementAssociations.Count);
        Debug.Log("-----------------");
        
    }

    public List<string> GetInitialElements()
    {
        if (initialElements != null)
        {
            return initialElements;
        }

        List<string> initialItems = new List<string>();
        TextAsset textAsset = Resources.Load<TextAsset>("InitialElements");
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            initialItems.AddRange(lines);
        }
        else
        {
            Debug.LogError("File non trovato");
        }
        return initialItems;
    }

    public List<string> GetFoundElements()
    {
        if (foundElements != null)
        {
            return foundElements;
        }

        List<string> foundItems = new List<string>();
        TextAsset textAsset = Resources.Load<TextAsset>("FoundElements");
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foundItems.AddRange(lines);
        }
        else
        {
            Debug.LogError("File non trovato");
        }
        return foundItems;
    }

    public Dictionary<ElementPair, string> GetElementAssociations()
    {
        if (elementAssociations != null)
        {
            return elementAssociations;
        }

        Dictionary<ElementPair, string> elementItems = new Dictionary<ElementPair, string>();

        TextAsset csvFile = Resources.Load<TextAsset>("fusions");
        if (csvFile == null)
        {
            Debug.LogError("CSV file not found.");
            return null;
        }

        using (StringReader reader = new StringReader(csvFile.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(';');
                if (parts.Length == 3)
                {
                    string element1 = parts[0].Trim();
                    string element2 = parts[1].Trim();
                    string result = parts[2].Trim();

                    ElementPair pair = new ElementPair(element1, element2);
                    elementItems[pair] = result;
                }
                else
                {
                    Debug.LogWarning("Invalid CSV line format: " + line);
                }
            }
        }
        return elementItems;
    }

    public void UpdateAll(){
        initialElements = null;
        foundElements = null;
        elementAssociations = null;

        initialElements = GetInitialElements();
        foundElements = GetFoundElements();
        elementAssociations = GetElementAssociations();
    }

    public string getAchievementsJson(){
        string filePath = Path.Combine(Application.dataPath, "Resources", "achievements.json");

        if (!File.Exists(filePath))
        {
            Debug.Log("Il file JSON non esiste nel percorso: " + filePath);
            return null;
        }
        else
        {
            Debug.Log("Il file JSON esiste nel percorso: " + filePath);
        }
        return File.ReadAllText(filePath);
    }


    public void UpdateAchievementsJson(string json){
        string filePath = Path.Combine(Application.dataPath, "Resources", "achievements.json");

        if (!File.Exists(filePath))
        {
            Debug.Log("Il file JSON non esiste nel percorso: " + filePath);
            return;
        }
        else
        {
            Debug.Log("Il file JSON esiste nel percorso: " + filePath);
        }
        File.WriteAllText(filePath, json);
    }

}
