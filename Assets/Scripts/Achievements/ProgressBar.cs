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
    private Sprite[] pathSprite;

    void Start()
    {
        pathSprite = Resources.LoadAll<Sprite>("AchievementsImage/Sprites");
        Debug.Log("pathSprite.Length: " + pathSprite.Length);
    }

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
                        Sprite completedBar = pathSprite[thisIdentifier.uniqueID];
                        child.GetComponent<Image>().sprite = completedBar;
                    }
                }
            }
        }
    }


}
