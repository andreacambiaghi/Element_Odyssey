using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsInfo : MonoBehaviour
{
    private int[] maxProgress = { 1, 5, 10, 10, 5, 5, 5, 5, 50, 1 };
    private string[] achievemetsInfo = {"Find the first element", "Find five elements", "Find ten elements", "Play ten minutes", "Find five water elements", "Find five fire elements", "Find five superheroes elements", "Find five elements in five minutes", "Find all elements", "It's a secret" };

    public int GetMaxProgress(int index)
    {
        return maxProgress[index];
    }

    public int GetMaxProgressLength()
    {
        return maxProgress.Length;
    }

    public string GetAchievementInfo(int index)
    {
        return achievemetsInfo[index];
    }
}
