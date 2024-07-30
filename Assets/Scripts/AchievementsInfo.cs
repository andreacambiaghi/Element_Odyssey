using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsInfo : MonoBehaviour
{
    private int[] maxProgress = { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

    public int GetMaxProgress(int index)
    {
        return maxProgress[index];
    }

    public int GetMaxProgressLength()
    {
        return maxProgress.Length;
    }
}
