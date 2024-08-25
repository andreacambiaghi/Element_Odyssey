using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsInfo
{
    private int[] maxProgress = { 1, 5, 10, 10, 5, 5, 5, 5, 50, 1 };
    private string[] achievemetsInfo = {"Find 1 element", "Find 5 elements", "Find 10 elements", "Play 10 minutes", "Find 5 water elements", "Find 5 fire elements", "Find 5 superheroes elements", "Find 5 elements in five minutes", "Find all elements", "It's a secret" };


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
