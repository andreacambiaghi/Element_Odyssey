using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class AchievementManager : MonoBehaviour
{
    private TextAsset jsonFile;

    private string filePath;    
    ElementFilesManager elementFilesManager;


    void Awake()
    {
        // filePath = Path.Combine(Application.dataPath, "Resources", "achievements.json");

        // if (!File.Exists(filePath))
        // {
        //     Debug.Log("Il file JSON non esiste nel percorso: " + filePath);
        // }
        // else
        // {
        //     Debug.Log("Il file JSON esiste nel percorso: " + filePath);
        // }

        elementFilesManager = ElementFilesManager.Instance;

        UpdateAchievements();
    }

    public int GetAchievementValue(string achievementKey)
    {
        //string json = File.ReadAllText(filePath);
        string json = elementFilesManager.getAchievementsJson();
        Debug.LogWarning(json);

        AchievementWrapper achievementWrapper = JsonUtility.FromJson<AchievementWrapper>(json);

        if (achievementWrapper == null)
        {
            Debug.LogError("AchievementWrapper is null");
        }

        foreach (var achievement in achievementWrapper.Achievements)
        {
            if (achievement.Key == achievementKey)
            {
                return achievement.Value;
            }
        }

        Debug.Log("Chiave dell'achievement non trovata: " + achievementKey);
        return -1;
    }


    public void SetAchievementValue(string achievementKey, int value)
    {
        Debug.Log("SetAchievementValue: " + achievementKey + " - " + value);
        // string json = File.ReadAllText(filePath);
        Debug.Log("ZERO");
        string json = elementFilesManager.getAchievementsJson();
        Debug.Log("UNO");
        AchievementWrapper achievementWrapper = JsonUtility.FromJson<AchievementWrapper>(json);
        Debug.Log("DUE");
        if (achievementWrapper == null)
        {
            achievementWrapper = new AchievementWrapper { Achievements = new Achievement[0] };
        }
        Debug.Log("TRE");

        var achievementsList = new List<Achievement>(achievementWrapper.Achievements);
        Debug.Log("QUATTRO");
        var existingAchievement = achievementsList.Find(a => a.Key == achievementKey);
        Debug.Log("CINQUE");
        if (existingAchievement != null)
        {
            existingAchievement.Value = value;
            Debug.Log("SEI");
        }

        else
        {
            achievementsList.Add(new Achievement { Key = achievementKey, Value = value });
            Debug.Log("SETTE");
        }

        achievementWrapper.Achievements = achievementsList.ToArray();
        Debug.Log("OTTO");
        string updatedJson = JsonUtility.ToJson(achievementWrapper, true);
        Debug.Log("NOVE");
        
        elementFilesManager.UpdateAchievementsJson(updatedJson);
        Debug.Log("DIECI");
        //File.WriteAllText(filePath, updatedJson);
    }
    
    public void UpdateAchievements()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject achievementObject = FindGameObjectWithUniqueID(i);
            if (achievementObject != null)
            {
                ProgressBar progressBar = achievementObject.GetComponent<ProgressBar>();
                if (progressBar != null)
                {
                    int progress = GetAchievementValue("Achievement " + i);
                    progressBar.SetProgress(progress);
                    // Debug.Log("progress: " + progress);
                }
            }
        }
    }

    GameObject FindGameObjectWithUniqueID(int uniqueID)
    {
        GameObject[] achievementObjects = GameObject.FindGameObjectsWithTag("Achievement");
        // Debug.Log("achievementObjects.Length: " + achievementObjects.Length);
        foreach (GameObject obj in achievementObjects)
        {
            AchievementIdentifier identifier = obj.GetComponent<AchievementIdentifier>();
            if (identifier != null && identifier.uniqueID == uniqueID)
            {
                return obj;
            }
        }
        return null;
    }
}

[System.Serializable]
public class AchievementWrapper
{
    public Achievement[] Achievements;
}

[System.Serializable]
public class Achievement
{
    public string Key;
    public int Value;
}
