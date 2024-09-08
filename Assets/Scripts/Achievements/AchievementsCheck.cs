using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsCheck : MonoBehaviour
{
    private readonly List<string> waterElements = new()
    { 
        "fjord", "lake", "ocean", "seaweed", "steam", "surf" , "swamp", "tea", "tsunami", "wave"
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
        "cloud", "dustStorm", "fog", "hurricane", "pollen", "sandStorm", "smoke", "storm", "tornado"
    };

    private readonly List<string> otherElements = new()
    { 
        "avalnche", "engine", "incense", "planet",
    };

    private int countWaterElements = 0;
    private int countFireElements = 0;
    private int countEarthElements = 0;
    private int countAirElements = 0;
    private int countOtherElements = 0;

    private int minutesPlayed = 0;
    private float time = 0.0f;

    private int foundedIn5Minutes = 0;
    private float time5Minutes = 0.0f;
    private bool is5Minutes = false;

    public static AchievementsCheck Instance;

    private readonly AchievementManager achievementManager = AchievementManager.Instance;

    public void Awake()
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
        time = Time.time;
        time5Minutes = Time.time;
        foundedIn5Minutes = 0;
    }

    public void Update()
    {
        achievementManager.SetAchievementValue("Achievement 3", minutesPlayed);
        minutesPlayed = (int)(Time.time - time) / 60;

        if (Time.time - time5Minutes >= 300 && !is5Minutes)
        {
            time5Minutes = Time.time;
            foundedIn5Minutes = 0;
        }

        if (foundedIn5Minutes == 5)
        {
            achievementManager.SetAchievementValue("Achievement 8", 1);
            is5Minutes = true;
        }
        
    }

    public void FoundedElement(string element)
    {
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

        UpdateAchievementsJson();
    }

    public void ResetAchievements()
    {
        countWaterElements = 0;
        countFireElements = 0;
        countEarthElements = 0;
        countAirElements = 0;
        countOtherElements = 0;
    }

    public int GetCountWaterElements()
    {
        return countWaterElements;
    }

    public int GetCountFireElements()
    {
        return countFireElements;
    }

    public int GetCountEarthElements()
    {
        return countEarthElements;
    }

    public int GetCountAirElements()
    {
        return countAirElements;
    }

    public int GetCountOtherElements()
    {
        return countOtherElements;
    }

    public int GetCountAllElements()
    {
        return countWaterElements + countFireElements + countEarthElements + countAirElements + countOtherElements;
    }

    private void UpdateAchievementsJson() {

        achievementManager.SetAchievementValue("Achievement 0", GetCountAllElements());
        achievementManager.SetAchievementValue("Achievement 1", GetCountAllElements());
        achievementManager.SetAchievementValue("Achievement 2", GetCountAllElements());
        achievementManager.SetAchievementValue("Achievement 4", countWaterElements);
        achievementManager.SetAchievementValue("Achievement 5", countFireElements);
        achievementManager.SetAchievementValue("Achievement 6", countEarthElements);
        achievementManager.SetAchievementValue("Achievement 7", countAirElements);
        achievementManager.SetAchievementValue("Achievement 9", GetCountAllElements());

        
    }
}
