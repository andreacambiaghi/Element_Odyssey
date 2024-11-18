using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ElementFilesManager : MonoBehaviour
{

    [SerializeField]
    public TextAsset defaultAchievementsJsonFile;

    private string foundElementsFilePath;

    private string achievementsFilePath;

    private string villageObjectsFilePath;

    private string villageSaveDataFilePath;

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
        villageObjectsFilePath = Path.Combine(Application.persistentDataPath, "villageObjects.json");
        villageSaveDataFilePath = Path.Combine(Application.persistentDataPath, "villageSaveData.json");

        UpdateAll();

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

        Debug.Log("INITIAL ELEMENTS: " + string.Join(", ", initialItems));

        return initialItems;
    }

    public List<string> GetFoundElements()
    {
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

    public bool AddFoundElement(string foundElement)
    {
        //valutare se non aggiornare ogni volta
        List<string> foundElements = GetFoundElements();

        if (foundElements.Contains(foundElement.ToLower()))
        {
            Debug.LogWarning("Elemento già trovato (EFM): " + foundElement);
            return false;
        }

        foundElements.Add(foundElement.ToLower());
        UpdateFoundElementsFile(foundElements);
        return true;
    }

    public void ResetFoundElements()
    {
        List<string> foundElements = GetFoundElements();
        foundElements.Clear();
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

    public void ResetAchievements()
    {
        string filePath = achievementsFilePath;
        File.WriteAllText(filePath, defaultAchievementsJsonFile.text);
        AchievementsCheck.Instance.ResetAchievements();
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
        string filePath = achievementsFilePath;

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

    public List<string> GetOthersElements(string fileName = "others")
    {
        TextAsset file = Resources.Load<TextAsset>(fileName);
        List<string> elements = new();

        if (file != null)
        {
            string[] lines = file.text.Split('\n');
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    elements.Add(line.Trim());
                }
            }
        }
        else
        {
            Debug.LogError("File not found in Resources folder!");
        }

        return elements;
    }

    public string getDefaultVillageObjects()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("villageObjectsDefaults");
        return textAsset.text;
    }

    public VillageData GetVillageData()
    {
        string filePath = villageObjectsFilePath;

        if (!File.Exists(filePath))
        {
            Debug.LogError("Il file JSON non esiste nel percorso: " + filePath);
            File.WriteAllText(filePath, getDefaultVillageObjects());
        }
        else
        {
            Debug.LogWarning("Il file JSON esiste nel percorso: " + filePath);
        }

        string villageDataString = File.ReadAllText(filePath);
        //Debug.LogWarning("saved Village data: " + villageDataString);

        //Debug.LogWarning("now creating the village data object");

        VillageData villageData = JsonUtility.FromJson<VillageData>(villageDataString);

        if (villageData == null)
        {
            Debug.LogError("Village data is null after deserialization");
            return null;
        }

        if (villageData.villageObjects == null)
        {
            Debug.LogError("villageObjects is null after deserialization");
            return null;
        }

        Debug.Log("This is the village data:");
        Debug.LogWarning("Village data: " + villageData.toString());

        //Debug.LogError("writing village data");
        // foreach (var villageObject in villageData.villageObjects)
        // {
        //     Debug.Log($"Key: {villageObject.Key}, Value: {villageObject.Value}");
        //     Debug.Log("Requirements: " + string.Join(", ", villageObject.Requirements));
        // }
        //Debug.LogError("Finshed writing village data");
        return villageData;
    }

    public void RefreshVillageData()
    {
        UpdateAll();
        VillageData villageData = GetVillageData();
        foreach(var villageObject in villageData.villageObjects)
        {
            if (villageObject.Requirements != null)
            {
                bool allRequirementsSatisfied = true;
                foreach (var requirement in villageObject.Requirements)
                {
                    if (!foundElements.Contains(requirement.ToLower()) && !initialElements.Contains(requirement.ToLower()))
                    {
                        allRequirementsSatisfied = false;
                        break;
                    }
                    if(allRequirementsSatisfied)
                    {
                        villageObject.Value = 1;
                    } else
                    {
                        villageObject.Value = 0;
                    }
                }
            }
        }
        UpdateVillageData(villageData);
    }

    public void ResetVillageData()
    {
        string filePath = villageObjectsFilePath;
        File.WriteAllText(filePath, getDefaultVillageObjects());
    }

    public void UpdateVillageData(VillageData villageData)
    {
        string filePath = villageObjectsFilePath;

        if (!File.Exists(filePath))
        {
            Debug.Log("Il file JSON non esiste nel percorso, an empty one will be created: " + filePath);
        }
        else
        {
            Debug.Log("Il file JSON esiste nel percorso: " + filePath);
        }

        string villageDataString = JsonUtility.ToJson(villageData);
        File.WriteAllText(filePath, villageDataString);
    }



    public string getDefaultVillageSaveData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("villageSaveDataDefaults");
        return textAsset.text;
    }

    public VillageSaveData getVillageSaveData()
    {
        string filePath = villageSaveDataFilePath;

        if (!File.Exists(filePath))
        {
            Debug.LogError("Il file JSON non esiste nel percorso: " + filePath);
            File.WriteAllText(filePath, getDefaultVillageSaveData());        
        }
        else
        {
            Debug.LogWarning("Il file JSON esiste nel percorso: " + filePath);
        }

        string villageSaveDataString = File.ReadAllText(filePath);
        Debug.LogWarning("saved Village save data: " + villageSaveDataString);

        //Debug.LogWarning("now creating the village data object");

        VillageSaveData villageSaveData = JsonUtility.FromJson<VillageSaveData>(villageSaveDataString);

        if (villageSaveData == null)
        {
            Debug.LogError("Village save data is null after deserialization");
            return null;
        }
        if (villageSaveData.villageObjects == null)
        {
            Debug.LogError("villageObjects is null after deserialization");
            return null;
        }

        Debug.Log("This is the village save data:");
        Debug.LogWarning("Village data: " + villageSaveData.toString());

        //Debug.LogError("writing village data");
        foreach (var villageObject in villageSaveData.villageObjects)
        {
            Debug.Log($"Key: {villageObject.objectName}, Value: {villageObject.position}");
        }
        //Debug.LogError("Finshed writing village data");
        return villageSaveData;
    }

    public void UpdateVillageSaveData(VillageSaveData villageSaveData)
    {
        string filePath = villageSaveDataFilePath;

        if (!File.Exists(filePath))
        {
            Debug.Log("Il file JSON non esiste nel percorso, an empty one will be created: " + filePath);
        }
        else
        {
            Debug.Log("Il file JSON esiste nel percorso: " + filePath);
        }

        string villageSaveDataString = JsonUtility.ToJson(villageSaveData);
        File.WriteAllText(filePath, villageSaveDataString);

        Debug.LogWarning("UPDATED VILLAGE SAVE DATA: " + File.ReadAllText(filePath));

    }








    [Serializable]
    public class VillageData
    {
        public List<VillageObject> villageObjects;

        public string toString()
        {
            if (villageObjects == null)
            {
                return "No village objects available.";
            }
            return string.Join("\n", villageObjects.ConvertAll(v => v.toString()));
        }
    }

    [Serializable]
    public class VillageObject
    {
        public string Key; 
        public int Value;
        public List<string> Requirements;

        public string toString()
        {
            return $"{Key} {Value} {(Requirements != null ? string.Join(", ", Requirements) : "No requirements")}";
        }
    }

    [Serializable]
    public class VillageSaveData
    {
        public List<VillageObjectSaveData> villageObjects;

        public string toString()
        {
            return string.Join("\n", villageObjects.ConvertAll(v => v.toString()));
        }
    }

    [Serializable]
    public class VillageObjectSaveData
    {
        public string objectName;
        public Vector3 position;

        public string toString()
        {
            return $"{objectName} {position}";
        }
    }

}
