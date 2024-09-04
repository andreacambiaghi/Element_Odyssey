using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ElementFilesManager : MonoBehaviour
{

    [SerializeField]
    public TextAsset defaultAchievementsJsonFile;

    private string foundElementsFilePath;

    private string achievementsFilePath;

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
        foundElementsFilePath = Path.Combine(Application.persistentDataPath, "FoundElements.txt");
        achievementsFilePath = Path.Combine(Application.persistentDataPath, "achievements.json");

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
        // if (foundElements != null)
        // {
        //     return foundElements;
        // }

        string filePath = foundElementsFilePath;
        List<string> foundItems = new List<string>();
        if (!File.Exists(filePath))
        {
            Debug.LogError("Il file JSON non esiste nel percorso: " + filePath);
            File.WriteAllText(filePath, "");
            Debug.LogWarning("Il file JSON è stato creato nel percorso: " + filePath);
        }
        else
        {
            Debug.LogWarning("Il file JSON esiste nel percorso: " + filePath);
            string[] lines = File.ReadAllText(filePath).Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foundItems.AddRange(lines);
        }

        // Debug.Log("Reading JSON file achiements.json");
        Debug.LogWarning("FOUND ELEMENTS: " + File.ReadAllText(filePath));
        return foundItems;

        // List<string> foundItems = new List<string>();
        // TextAsset textAsset = Resources.Load<TextAsset>("FoundElements");
        // if (textAsset != null)
        // {
        //     string[] lines = textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        //     foundItems.AddRange(lines);
        // }
        // else
        // {
        //     Debug.LogError("File non trovato");
        // }
        // return foundItems;
    }

    private void UpdateFoundElementsFile(List<string> foundElements)
    {
        //TODO: mettere anche funzione con append per singolo foundelement
        string filePath = foundElementsFilePath;
        File.WriteAllLines(filePath, foundElements);
        Debug.LogWarning("UPDATED FOUND ELEMENTS: " + File.ReadAllText(filePath));
    }

    public void AddFoundElement(string foundElement)
    {
        //valutare se non aggiornare ogni volta
        List<string> foundElements = GetFoundElements();

        if (foundElements.Contains(foundElement))
        {
            return;
        }

        foundElements.Add(foundElement);
        UpdateFoundElementsFile(foundElements);
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


    private void ListDirectoriesInPersistentDataPath()
    {
        string persistentDataPath = Path.Combine(Application.persistentDataPath, "il2cpp", "Resources");

        if (Directory.Exists(persistentDataPath))
        {
            string[] directories = Directory.GetDirectories(persistentDataPath);

            if (directories.Length == 0)
            {
                Debug.Log("No directories found in the persistentDataPath.");
            }
            else
            {
                Debug.Log("Directories in persistentDataPath:");
                foreach (string directory in directories)
                {
                    Debug.Log(directory);
                }
            }
        }
        else
        {
            Debug.LogError("The persistentDataPath does not exist.");
        }
    }


    public string getAchievementsJson(){
        //Debug.LogWarning("DEFAULT FILE -> " + defaultAchievementsJsonFile.text);
        //ListDirectoriesInPersistentDataPath();
        //string filePath = Path.Combine(Application.dataPath, "Resources", "achievements.json");
        // string filePath = Path.Combine(Application.persistentDataPath, "il2cpp", "Resources", "achievements.json");
        //string filePath = Path.Combine(Application.persistentDataPath, "il2cpp", "Resources", "achievements.json");
        string filePath = achievementsFilePath;

        //TextAsset textAsset = Resources.Load<TextAsset>("achievements");

        // Debug.Log("Reading JSON file achiements");
        // Debug.LogWarning(textAsset.text);

        if (!File.Exists(filePath))
        {
            Debug.LogError("Il file JSON non esiste nel percorso: " + filePath);
            File.WriteAllText(filePath, defaultAchievementsJsonFile.text);
            //Debug.LogWarning("Il file JSON è stato creato nel percorso: " + filePath);
        }
        else
        {
            Debug.LogWarning("Il file JSON esiste nel percorso: " + filePath);
        }

        // Debug.Log("Reading JSON file achiements.json");
        Debug.LogWarning("SAVED ACHIEVEMENTS: " + File.ReadAllText(filePath));
        return File.ReadAllText(filePath);

        //return textAsset.text;
        //return achievementsJsonFile.text;
    }


    public void UpdateAchievementsJson(string json){
        string filePath = Path.Combine(Application.persistentDataPath, "Resources", "achievements.json");

        if (!File.Exists(filePath))
        {
            Debug.Log("Il file JSON non esiste nel percorso, an empty one will be created: " + filePath);
        }
        else
        {
            Debug.Log("Il file JSON esiste nel percorso: " + filePath);
        }
        File.WriteAllText(filePath, json);
    }

    public Sprite[] GetPathSprite()
    {
        Sprite[] pathSprite = Resources.LoadAll<Sprite>("AchievementsImage/Sprites");
        Debug.Log("pathSprite.Length: " + pathSprite.Length);
        return pathSprite;
    }

}
