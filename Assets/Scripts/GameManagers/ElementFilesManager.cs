using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ElementFilesManager : Singleton<ElementFilesManager>
{

    // [SerializeField]
    // public TextAsset defaultAchievementsJsonFile;

    // [SerializeField]
    // public TextAsset defaultElementTypeListJsonFile;

    private string foundElementsFilePath;

    private string achievementsFilePath;

    private string villageObjectsFilePath;

    private string villageSaveDataFilePath;

    private string habitatsFilePath;

    private string buyFloorSaveDataFilePath;

    private string balanceFilePath;

    private string arMarkerAssociationsFilePath;

    private string elementTypeFilePath;

    // public static ElementFilesManager Instance;

    private List<string> initialElements = null;

    private List<string> foundElements = null;

    private Dictionary<ElementPair, string> elementAssociations = null;

    private ElementTypeList elementTypeList = null;



    private void Awake()
    {
        if (FindObjectsOfType(typeof(ElementFilesManager)).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Initialize()
    {
        foundElementsFilePath = Path.Combine(Application.persistentDataPath, "FoundElements.txt");
        achievementsFilePath = Path.Combine(Application.persistentDataPath, "achievements.json");
        villageObjectsFilePath = Path.Combine(Application.persistentDataPath, "villageObjects.json");
        villageSaveDataFilePath = Path.Combine(Application.persistentDataPath, "villageSaveData.json");
        buyFloorSaveDataFilePath = Path.Combine(Application.persistentDataPath, "buyFloor.txt");
        balanceFilePath = Path.Combine(Application.persistentDataPath, "balance.txt");
        arMarkerAssociationsFilePath = Path.Combine(Application.persistentDataPath, "arMarkersAssociations.json");
        habitatsFilePath = Path.Combine(Application.persistentDataPath, "habitats.json");
        // elementTypeFilePath = Path.Combine(Application.persistentDataPath, "elementsType.json");

        elementTypeList = LoadElementTypeData();
        if (elementTypeList != null && elementTypeList.elements != null)
        {
            Debug.Log("--- Element Types ---");
            foreach (var elementType in elementTypeList.elements)
            {
                Debug.Log($"Element: {elementType.element}, Type: {elementType.type}");
            }
            Debug.Log("---------------------");
        }
        else
        {
            Debug.LogWarning("Could not load or print element types.");
        }

        UpdateAll();

        Debug.Log("-----------------");
        Debug.Log("ElementFilesManager initialized");
        Debug.Log("InitialElements: " + initialElements.Count);
        Debug.Log("FoundElements: " + foundElements.Count);
        Debug.Log("ElementAssociations: " + elementAssociations.Count);
        Debug.Log("ArMarkerAssociations: " + GetArMarkerAssociations().associationList.Count);
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
            Debug.LogWarning("Il file JSON non esiste nel percorso: " + filePath);
            File.WriteAllText(filePath, "");
            Debug.LogWarning("Il file JSON è stato creato nel percorso: " + filePath);
        }
        else
        {
            Debug.LogWarning("Il file JSON esiste nel percorso: " + filePath);
            string[] lines = File.ReadAllText(filePath).Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foundItems.AddRange(lines);
        }

        Debug.Log("FOUND ELEMENTS: " + File.ReadAllText(filePath));
        return foundItems;
    }

    private void UpdateFoundElementsFile(List<string> foundElements)
    {
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
        TextAsset defaultAchievementsJsonFile = Resources.Load<TextAsset>("achievements");
        File.WriteAllText(filePath, defaultAchievementsJsonFile.text);
        AchievementsCheck.Instance.ResetAchievements();
    }

    public void ResetVillageObjects()
    {
        string filePath = villageSaveDataFilePath;
        File.WriteAllText(filePath, getDefaultVillageSaveData());
    }

    public void UpdateAll()
    {
        initialElements = null;
        foundElements = null;
        elementAssociations = null;


        initialElements = GetInitialElements();
        foundElements = GetFoundElements();
        elementAssociations = GetElementAssociations();
        // LoadElementTypeData();
    }

    public string getAchievementsJson()
    {
        string filePath = achievementsFilePath;


        if (!File.Exists(filePath))
        {
            Debug.LogError("Il file JSON non esiste nel percorso: " + filePath);
            TextAsset defaultAchievementsJsonFile = Resources.Load<TextAsset>("achievements");
            File.WriteAllText(filePath, defaultAchievementsJsonFile.text);
            //Debug.LogWarning("Il file JSON è stato creato nel percorso: " + filePath);
        }
        else
        {
            Debug.LogWarning("Il file JSON esiste nel percorso: " + filePath);
        }

        // Debug.Log("Reading JSON file achiements.json");
        // Debug.LogWarning("SAVED ACHIEVEMENTS: " + File.ReadAllText(filePath));
        return File.ReadAllText(filePath);

        //return textAsset.text;
        //return achievementsJsonFile.text;
    }

    public void UpdateAchievementsJson(string json)
    {
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

        // Debug.Log("This is the village data:");

        // Debug.LogError("writing village data");
        // foreach (var villageObject in villageData.villageObjects)
        // {
        //     Debug.Log($"Key: {villageObject.Key}, \nValue: {villageObject.Value}, \nRequirements: [{string.Join(", ", villageObject.Requirements)}]");
        //     // Debug.Log("Requirements: " + string.Join(", ", villageObject.Requirements));
        // }
        // Debug.LogError("Finshed writing village data");
        return villageData;
    }

    public void RefreshVillageData()
    {
        UpdateAll();
        VillageData villageData = GetVillageData();
        foreach (var villageObject in villageData.villageObjects)
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
                }
                if (allRequirementsSatisfied)
                {
                    villageObject.Value = 1;
                }
                else
                {
                    villageObject.Value = 0;
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



    public string getDefaultHabitats()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("habitatsDefaults");
        return textAsset.text;
    }

    public VillageHabitats GetVillageHabitats()
    {
        string filePath = habitatsFilePath;

        if (!File.Exists(filePath))
        {
            Debug.LogError("Il file JSON non esiste nel percorso: " + filePath);
            File.WriteAllText(filePath, getDefaultHabitats());
            Debug.LogWarning("Default habitats creato nel percorso: " + filePath);
        }
        else
        {
            Debug.LogWarning("Il file JSON esiste nel percorso: " + filePath);
        }

        string habitatsString = File.ReadAllText(filePath);
        Debug.LogWarning("saved Village habitats: " + habitatsString);

        // Debug.LogWarning("now creating the village data object");

        VillageHabitats villageHabitats = JsonUtility.FromJson<VillageHabitats>(habitatsString);

        if (villageHabitats == null)
        {
            Debug.LogError("Habitats is null after deserialization");
            return null;
        }

        if (villageHabitats.habitats == null)
        {
            Debug.LogError("villageHabitats is null after deserialization");
            return null;
        }

        Debug.Log("This is the village habitats:");

        Debug.LogError("writing village habitats");
        foreach (var villageObject in villageHabitats.habitats)
        {
            Debug.Log($"Key: {villageObject.Key}, \nValue: {villageObject.Value}, \nRequirements: [{string.Join(", ", villageObject.Requirements)}]");
            // Debug.Log("Requirements: " + string.Join(", ", villageObject.Requirements));
        }
        Debug.LogError("Finshed writing village habitats");
        return villageHabitats;
    }

    public void RefreshVillageHabitats()
    {
        UpdateAll();
        VillageHabitats villageHabitats = GetVillageHabitats();
        foreach (var villageHabitat in villageHabitats.habitats)
        {
            if (villageHabitat.Requirements != null)
            {
                bool allRequirementsSatisfied = true;
                foreach (var requirement in villageHabitat.Requirements)
                {
                    if (!foundElements.Contains(requirement.ToLower()) && !initialElements.Contains(requirement.ToLower()))
                    {
                        allRequirementsSatisfied = false;
                        break;
                    }
                }
                if (allRequirementsSatisfied)
                {
                    villageHabitat.Value = 1;
                }
                else
                {
                    villageHabitat.Value = 0;
                }
            }
        }
        UpdateVillageHabitats(villageHabitats);
    }

    public void ResetVillageHabitats()
    {
        string filePath = habitatsFilePath;
        File.WriteAllText(filePath, getDefaultHabitats());
    }

    public void UpdateVillageHabitats(VillageHabitats villageHabitats)
    {
        string filePath = habitatsFilePath;

        if (!File.Exists(filePath))
        {
            Debug.Log("Il file JSON non esiste nel percorso, an empty one will be created: " + filePath);
        }
        else
        {
            Debug.Log("Il file JSON esiste nel percorso: " + filePath);
        }

        string villageHabitatsString = JsonUtility.ToJson(villageHabitats);

        File.WriteAllText(filePath, villageHabitatsString);
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

        if (villageSaveData.floor == null)
        {
            villageSaveData.floor = "black";
        }
        if (villageSaveData.villageObjects == null)
        {
            Debug.LogError("villageObjects is null after deserialization");
            // return null;
        }

        Debug.Log("This is the village save data:");
        Debug.LogWarning("Village data: " + villageSaveData.toString());

        // foreach (var villageObject in villageSaveData.villageObjects)
        // {
        //     Debug.Log($"Key: {villageObject.objectName}, Value: {villageObject.position}");
        // }
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

    public List<string> GetBuyFloorSaveData()
    {
        string filePath = buyFloorSaveDataFilePath;
        List<string> boughtFloors = new List<string>();
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
            boughtFloors.AddRange(lines);
        }

        Debug.LogWarning("BOUGHT FLOORS: " + File.ReadAllText(filePath));
        return boughtFloors;
    }

    public bool SaveBuyFloor(string boughtFloor)
    {
        List<string> boughtFloors = GetBuyFloorSaveData();

        if (boughtFloors.Contains(boughtFloor.ToLower()))
        {
            Debug.LogWarning("Floor già comprato" + boughtFloor);
            return false;
        }

        boughtFloors.Add(boughtFloor.ToLower());
        // UpdateFoundElementsFile(foundElements);

        File.WriteAllLines(buyFloorSaveDataFilePath, boughtFloors);
        Debug.LogWarning("UPDATED BOUGHT FLOORS: " + File.ReadAllText(buyFloorSaveDataFilePath));
        return true;
    }

    public void ResetBuyFloor()
    {
        File.WriteAllLines(buyFloorSaveDataFilePath, new string[0]);
    }

    public void SetBalance(int balance)
    {
        string filePath = balanceFilePath;
        File.WriteAllText(filePath, balance.ToString());
    }

    public void ResetBalance()
    {
        string filePath = balanceFilePath;
        File.WriteAllText(filePath, "0");
    }

    public int GetBalance()
    {
        string filePath = balanceFilePath;
        if (!File.Exists(filePath))
        {
            Debug.LogError("Il file JSON non esiste nel percorso: " + filePath);
            File.WriteAllText(filePath, "0");

            Debug.LogWarning("Il file JSON è stato creato nel percorso: " + filePath);
        }
        // Debug.LogWarning("BALANCE: " + File.ReadAllText(filePath));
        return int.Parse(File.ReadAllText(filePath));
    }

    private ArMarkerAssociations createDefaultMarkerAssociations()
    {
        ArMarkerAssociations defaultAssociations = new ArMarkerAssociations();
        for (int i = 0; i <= 20; i++)
        {
            string defaultValue = "";
            if (i <= 5)
            {
                defaultValue = "water";
            }
            else if (i <= 10)
            {
                defaultValue = "fire";
            }
            else if (i <= 15)
            {
                defaultValue = "wind";
            }
            else if (i <= 20)
            {
                defaultValue = "earth";
            }
            defaultAssociations.AddAssociation(i + "", defaultValue);
        }
        return defaultAssociations;
    }

    public void UpdateMarkerAssociations(ArMarkerAssociations arMarkerAssociations)
    {


        string filePath = arMarkerAssociationsFilePath;


        if (arMarkerAssociations == null)
        {
            Debug.LogError("[EFM] UpdateMarkerAssociations called with null parameter!");
            return;
        }

        if (arMarkerAssociations.associationList == null)
        {
            Debug.LogError("[EFM] UpdateMarkerAssociations called with null associationList!");
            return;
        }

        Debug.Log($"[EFM] UpdateMarkerAssociations called with {arMarkerAssociations.associationList.Count} associations");

        if (arMarkerAssociations.associationList.Count == 0)
        {
            Debug.LogWarning("[EFM] Warning: Updating with empty association list");
        }



        if (!File.Exists(filePath))
        {
            Debug.Log("Il file JSON non esiste nel percorso, a default one will be created: " + filePath);
        }
        else
        {
            Debug.Log("Il file JSON esiste nel percorso: " + filePath);
        }

        string associationsString = JsonUtility.ToJson(arMarkerAssociations);
        Debug.Log($"[EFM] Serialized JSON: {associationsString}");

        File.WriteAllText(filePath, associationsString);

        try
        {
            string jsonContent = File.ReadAllText(filePath);
            var parsedJson = JsonUtility.FromJson<ArMarkerAssociations>(jsonContent);
            string prettyJson = JsonUtility.ToJson(parsedJson, true); // true enables pretty printing
            Debug.LogWarning("[EFM] UPDATED AR MARKER ASSOCIATIONS:\n" + prettyJson);
        }
        catch (Exception e)
        {
            Debug.LogWarning("[EFM] UPDATED AR MARKER ASSOCIATIONS (failed to format):\n" + File.ReadAllText(filePath));
            Debug.LogException(e);
        }
        Debug.Log($"[EFM] Serialized {arMarkerAssociations.associationList.Count} marker associations");
    }


    public ArMarkerAssociations GetArMarkerAssociations()
    {
        string filePath = arMarkerAssociationsFilePath;

        if (!File.Exists(filePath))
        {
            Debug.LogError("Il file JSON non esiste nel percorso: " + filePath);

            ArMarkerAssociations defaultAssociations = createDefaultMarkerAssociations();

            string defaultAssociationsString = JsonUtility.ToJson(defaultAssociations);
            File.WriteAllText(filePath, defaultAssociationsString);

            // Debug output to verify serialization worked
            Debug.LogWarning("Generated association: " + defaultAssociationsString);
            Debug.LogWarning("UPDATED AR MARKER ASSOCIATIONS: " + File.ReadAllText(filePath));
            Debug.Log($"Created default marker associations with {defaultAssociations.associationList.Count} entries");

            return defaultAssociations;
        }
        else
        {
            Debug.LogWarning("Il file JSON esiste nel percorso: " + filePath);
        }

        string associationsString = File.ReadAllText(filePath);

        ArMarkerAssociations arMarkerAssociations = JsonUtility.FromJson<ArMarkerAssociations>(associationsString);

        if (arMarkerAssociations == null)
        {
            Debug.LogError("ArMarkerAssociations is null after deserialization");
            return null;
        }

        Debug.Log($"Loaded {arMarkerAssociations.associationList.Count} marker associations");

        Debug.LogWarning("Generated association: " + associationsString);
        Debug.LogWarning("UPDATED AR MARKER ASSOCIATIONS: " + File.ReadAllText(filePath));

        return arMarkerAssociations;
    }

    public void DeleteMakerAssociations()
    {
        string filePath = arMarkerAssociationsFilePath;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Il file JSON è stato eliminato dal percorso: " + filePath);
        }
        else
        {
            Debug.Log("Il file JSON non esiste nel percorso: " + filePath);
        }
    }

    public string GetMarkerType(string markerId)
    {
        ArMarkerAssociations arMarkerAssociations = createDefaultMarkerAssociations();
        if (arMarkerAssociations == null)
        {
            Debug.LogError("ArMarkerAssociations is null after deserialization");
            return null;
        }

        string elementType = arMarkerAssociations.GetValue(markerId);

        if (elementType == null)
        {
            Debug.LogWarning($"Marker ID '{markerId}' not found in default associations.");
            return null; // Or handle as needed, maybe return a default type?
        }

        return elementType;
    }

    private ElementTypeList LoadElementTypeData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("elementsType");

        // Debug.Log("Loading element type data from: " + textAsset.text);
        if (textAsset == null)
        {
            Debug.LogError("Element type JSON file not found in Resources folder.");
            return null;
        }

        string json = textAsset.text;
        // string json = File.ReadAllText(filePath);

        ElementTypeList elementTypeList = JsonUtility.FromJson<ElementTypeList>(json);

        if (elementTypeList == null || elementTypeList.elements == null)
        {
            Debug.LogError("Failed to parse element type JSON.");
            return null;
        }

        Debug.Log("Loaded " + elementTypeList.elements.Count + " element types.");
        return elementTypeList;
    }

    public string GetElementType(string elementName)
    {
        if (elementTypeList == null)
        {
            elementTypeList = LoadElementTypeData();
        }

        if (elementTypeList == null || elementTypeList.elements == null)
        {
            Debug.LogError("Element type list is null or empty.");
            return null;
        }

        foreach (var element in elementTypeList.elements)
        {
            if (element.element.Equals(elementName, StringComparison.OrdinalIgnoreCase))
            {
                return element.type;
            }
        }

        Debug.LogWarning("Element type not found for: " + elementName);
        return null;
    }

    public bool CreateTutorialFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "tutorial.txt");
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "true");
            Debug.Log("Il file JSON è stato creato nel percorso: " + filePath);
            return true;
        }
        else
        {
            Debug.Log("Il file JSON esiste nel percorso: " + filePath);
            return false;
        }
    }

    public bool GetTutorialFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "tutorial.txt");
        if (!File.Exists(filePath))
        {
            Debug.Log("Il file JSON non esiste nel percorso: " + filePath);
            return false;
        }
        else
        {
            Debug.Log("Il file JSON esiste nel percorso: " + filePath);
            string[] lines = File.ReadAllText(filePath).Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            return bool.Parse(lines[0]);
        }
    }

    public void DeleteTutorialFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "tutorial.txt");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Il file JSON è stato eliminato dal percorso: " + filePath);
        }
        else
        {
            Debug.Log("Il file JSON non esiste nel percorso: " + filePath);
        }
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
            return $"Key: {Key} \nValue: {Value} \nRequirements: {(Requirements != null ? string.Join(", ", Requirements) : "No requirements")}";
        }
    }

    [Serializable]
    public class VillageHabitats
    {
        public List<Habitat> habitats;

        public string toString()
        {
            if (habitats == null)
            {
                return "No village habitats available.";
            }
            return string.Join("\n", habitats.ConvertAll(v => v.toString()));
        }
    }

    [Serializable]
    public class Habitat
    {
        public string Key;
        public int Value;
        public List<string> Requirements;

        public string toString()
        {
            return $"Key: {Key} \nValue: {Value} \nRequirements: {(Requirements != null ? string.Join(", ", Requirements) : "No requirements")}";
        }
    }


    [Serializable]
    public class VillageSaveData
    {
        public List<VillageObjectSaveData> villageObjects;
        public string floor;

        public string toString()
        {
            return "Floor: " + floor + "\n" + string.Join("\n", villageObjects.ConvertAll(v => v.toString()));
        }
    }

    [Serializable]
    public class VillageObjectSaveData
    {
        public string objectName;
        public Vector3 position;

        public string toString()
        {
            return $"Object: {objectName}, Position: {position}";
        }
    }

    [Serializable]
    public class ArMarkerAssociations
    {
        [Serializable]
        public class MarkerAssociation
        {
            public string markerId;
            public string elementType;

            // Parameterless constructor needed for deserialization
            public MarkerAssociation() { }

            public MarkerAssociation(string markerId, string elementType)
            {
                this.markerId = markerId;
                this.elementType = elementType;
            }

        }

        // This is the serializable list that JsonUtility will use
        public List<MarkerAssociation> associationList = new List<MarkerAssociation>();

        // Dictionary for easier runtime access (not serialized)
        [NonSerialized]
        private Dictionary<string, string> _associations = new Dictionary<string, string>();

        // Initialize/sync dictionary from list after deserialization
        public Dictionary<string, string> associations
        {
            get
            {
                // Rebuild dictionary if needed
                if (_associations.Count != associationList.Count)
                {
                    _associations.Clear();
                    foreach (var assoc in associationList)
                    {
                        _associations[assoc.markerId] = assoc.elementType;
                    }
                }
                return _associations;
            }
        }

        public void AddAssociation(string key, string value)
        {
            // Check if this key already exists in the list
            MarkerAssociation existingAssoc = associationList.Find(a => a.markerId == key);

            if (existingAssoc != null)
            {
                // Update existing association
                existingAssoc.elementType = value;
            }
            else
            {
                // Add new association
                associationList.Add(new MarkerAssociation(key, value));
            }

            // Always update the dictionary
            _associations[key] = value;
        }

        public void RemoveAssociation(string key)
        {
            // Remove from list
            associationList.RemoveAll(a => a.markerId == key);

            // Remove from dictionary
            if (_associations.ContainsKey(key))
            {
                _associations.Remove(key);
            }
        }

        public string GetValue(string key)
        {
            if (associations.ContainsKey(key))
            {
                return associations[key];
            }
            return null;
        }
    }


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

}
