using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int maxProgress = 100;
    public int currentProgress = 0;
    [SerializeField] private Image mask;
    [SerializeField] private Sprite completedBar;

    void Update()
    {
        GetCurrentFill();
        if (currentProgress >= maxProgress)
        {
            SetImageCompletedBar();
        }
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)currentProgress / (float)maxProgress;
        mask.fillAmount = fillAmount;
    }

    public void SetProgress(int progress)
    {
        currentProgress = progress;
    }

    public void IncreaseProgress(int progress)
    {
        currentProgress += progress;
    }

    public void DecreaseProgress(int progress)
    {
        currentProgress -= progress;
    }

    public void ResetProgress()
    {
        currentProgress = 0;
    }

    public void SetMaxProgress(int progress)
    {
        maxProgress = progress;
    }

    public void SetImageCompletedBar()
    {
        GameObject[] achievementGOs = GameObject.FindGameObjectsWithTag("Achievement");
        Debug.Log("achievementGOs.Length: " + achievementGOs.Length);
        AchievementIdentifier thisIdentifier = gameObject.GetComponent<AchievementIdentifier>();
        foreach (GameObject go in achievementGOs)
        {
            AchievementIdentifier identifier = go.GetComponent<AchievementIdentifier>();
            if (identifier != null && identifier.uniqueID == thisIdentifier.uniqueID)
            {
                Transform[] children = go.GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child.CompareTag("Logo"))
                    {
                        child.GetComponent<Image>().sprite = completedBar;
                    }
                }
            }
        }
    }


}
