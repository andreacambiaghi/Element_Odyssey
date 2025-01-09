using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsCheck : MonoBehaviour
{
    private readonly List<string> waterElements = new()
    { 
        "fjord", "lake", "ocean", "seaweed", "steam", "surf" , "swamp", "tea", "tsunami",
        "wave", "rain", "snow", "surfing", "submarine", "sea", "lagoon"
    };

    private readonly List<string> fireElements = new()
    { 
        "ash", "eruption", "lava", "volcano"
    };

    private readonly List<string> earthElements = new()
    { 
      "dandelion", "dust", "island", "lily", "mountain", "mountainRange", "mud", "plant", "sand", "stone", "tree"
    };

    private readonly List<string> airElements = new()
    { 
        "cloud", "dustStorm", "fog", "hurricane", "pollen", "sandStorm", "smoke", "storm",
        "tornado", "jet", "windmill", "helicopter"
    };

    private readonly List<string> otherElements = new()
    { 
        "avalnche", "engine", "incense", "planet", "lightning", "rainbow", "sushi", "angel", "sky",
        "tractor", "train", "car", "boat", "vacuum", "rocket", "teapot", "sandbox",
        "steamroller", "wood", "steamboat", "bee", "saturn", "snowmobile",
        "brick", "clay", "beach", "mudslide", "mudweed", "quagmire", "puddle", "lotus", "prefume",
        "chai", "kite", "teaBag", "mountainDew", "tempest", "sandwich", "rock", "wine", "honey", 
        "matcha", "plantea", "teaStorm", "teaParty", "surfer", "surfboard", "palm", "allergy",
        "titanic", "earthquake", "wish", "forest", "poseidon", "mist", "atlantis", "disaster"
    };

    private int countWaterElements;
    private int countFireElements;
    private int countEarthElements;
    private int countAirElements;
    private int countOtherElements;
    private int countAllElements;

    private int minutesPlayed = 0;
    private float startTime = 0.0f;
    private List<float> timeList;
    public static AchievementsCheck Instance;

    private readonly AchievementManager achievementManager = AchievementManager.Instance;

    [SerializeField] private readonly GameObject achievementPanel;

    public void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        startTime = Time.time;
        timeList = new();

        countWaterElements = AchievementManager.Instance.GetAchievementValue("Achievement 4");
        countFireElements = AchievementManager.Instance.GetAchievementValue("Achievement 5");
        countEarthElements = AchievementManager.Instance.GetAchievementValue("Achievement 6");
        countAirElements = AchievementManager.Instance.GetAchievementValue("Achievement 7");
        countAllElements = AchievementManager.Instance.GetAchievementValue("Achievement 9");
        countOtherElements = countAllElements - countWaterElements - countFireElements - countEarthElements - countAirElements;

    }

    public void FoundedElement(string element)
    {
        Debug.Log("FoundedElement: " + element);

        if (waterElements.Contains(element))
        {
            countWaterElements++;
        }
        else if (fireElements.Contains(element))
        {
            countFireElements++;
        }
        else if (earthElements.Contains(element))
        {
            countEarthElements++;
        }
        else if (airElements.Contains(element))
        {
            countAirElements++;
        }
        else if (otherElements.Contains(element))
        {
            countOtherElements++;
        }

        countAllElements++;

        timeList.Add(Time.time);

        UpdateAchievementsJson();
    }

    public void ResetAchievements()
    {
        countWaterElements = 0;
        countFireElements = 0;
        countEarthElements = 0;
        countAirElements = 0;
        countAllElements = 0;
        countOtherElements = 0;
    }

    public int GetCountAllElements()
    {
        return countWaterElements + countFireElements + countEarthElements + countAirElements + countOtherElements;
    }

    private void UpdateAchievementsJson() {

        achievementManager.SetAchievementValue("Achievement 0", GetCountAllElements());
        achievementManager.SetAchievementValue("Achievement 1", GetCountAllElements());
        achievementManager.SetAchievementValue("Achievement 2", GetCountAllElements() - 1);
        achievementManager.SetAchievementValue("Achievement 4", countWaterElements);
        achievementManager.SetAchievementValue("Achievement 5", countFireElements);
        achievementManager.SetAchievementValue("Achievement 6", countEarthElements);
        achievementManager.SetAchievementValue("Achievement 7", countAirElements);
        achievementManager.SetAchievementValue("Achievement 9", GetCountAllElements());

        minutesPlayed = (int)(Time.time - startTime) / 60;
        achievementManager.SetAchievementValue("Achievement 3", minutesPlayed);

        if (countAllElements >= 5)
        {
            bool foundInTime = false;

            for (int i = 0; i <= timeList.Count - 5; i++) 
            {
                if (timeList[i + 4] - timeList[i] <= 300)
                {
                    achievementManager.SetAchievementValue("Achievement 8", 5);
                    foundInTime = true;
                    break;
                }
            }

            if (!foundInTime)
            {
                for (int i = 0; i < countAllElements; i++)
                {
                    int maxCountInTime = 1;

                    for (int j = i + 1; j < timeList.Count; j++)
                    {
                        if (timeList[j] - timeList[i] <= 300)
                        {
                            maxCountInTime++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    achievementManager.SetAchievementValue("Achievement 8", Mathf.Max(
                        achievementManager.GetAchievementValue("Achievement 8"),
                        maxCountInTime
                    ));
                }
            }
        }
        else
        {
            achievementManager.SetAchievementValue("Achievement 8", countAllElements);
        }


        for (int i = 0; i < 10; i++)
        {
            if (achievementManager.GetAchievementValue("Achievement " + i) == AchievementManager.Instance.GetMaxProgress(i))
            {
                GameObject achievement = Instantiate(achievementPanel, achievementPanel.transform.parent);

                AudioClip clip = Resources.Load<AudioClip>("Sounds/achievement");
                GameObject tempAudioObject = new GameObject("TempAudioObject");
                AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.Play();

                Destroy(achievement, 3f);
            }
        }

        
    }
}
