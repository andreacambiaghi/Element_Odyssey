using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int maxProgress = 100;
    public int currentProgress = 0;
    public Image mask;

    void Update()
    {
        GetCurrentFill();
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
}
