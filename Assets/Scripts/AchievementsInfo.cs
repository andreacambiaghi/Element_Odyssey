using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsInfo : MonoBehaviour
{
    public static AchievementsInfo Instance;

    private int[] maxProgress = { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetMaxProgress(int index)
    {
        return maxProgress[index];
    }

    public int GetMaxProgressLength()
    {
        return maxProgress.Length;
    }
}
