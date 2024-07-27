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
        Transform achievementSprite = transform.Find("AchievementSprite");
        if (achievementSprite != null)
        {
            Transform achievement = achievementSprite.Find("Achievement");
            if (achievement != null)
            {
                Transform logo = achievement.Find("Logo");
                if (logo != null)
                {
                    Image logoImage = logo.GetComponent<Image>();
                    if (logoImage != null)
                    {
                        logoImage.sprite = completedBar;
                    }
                    else
                    {
                        Debug.Log("Logo does not have an Image component.");
                    }
                }
                else
                {
                    Debug.Log("Logo child not found.");
                }
            }
            else
            {
                Debug.Log("Achievement child not found.");
            }
        }
        else
        {
            Debug.Log("AchievementSprite child not found.");
        }
    }


}
