using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsCheck : MonoBehaviour
{
    private List<string> waterElements = new List<string> 
    { 
        "fjord", "lake", "ocean", "seaweed", "steam", "surf" , "swamp", "tea", "tsunami", "wave"
    };

    private List<string> fireElements = new List<string> 
    { 
        "ash", "eruption", "lava", "volcano"
    };

    private List<string> earthElements = new List<string> 
    { 
      "dandelion", "dust", "island", "lily", "mountain", "mountainRange", "mud", "plant", "sand", "stone", "tree"
    };

    private List<string> airElements = new List<string> 
    { 
        "cloud", "dustStorm", "fog", "hurricane", "pollen", "sandStorm", "smoke", "storm", "tornado"
    };

    private List<string> otherElements = new List<string> 
    { 
        "avalnche", "engine", "incense", "planet",
    };

    private int countWaterElements = 0;
    private int countFireElements = 0;
    private int countEarthElements = 0;
    private int countAirElements = 0;
    private int countOtherElements = 0;

    public static AchievementsCheck Instance;

    private AchievementManager achievementManager = AchievementManager.Instance;

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
        if (GetCountAllElements() >= 1)
        {
            Debug.LogWarning("Achievement 0 unlocked");
            achievementManager.SetAchievementValue("Achievement 0", 1);
        }
    }
}
