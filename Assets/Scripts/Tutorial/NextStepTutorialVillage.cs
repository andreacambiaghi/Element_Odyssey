using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStepTutorialVillage : MonoBehaviour
{
    public void NextStep()
    {
        StepTutorialVillage.Instance.NextStep();
    }
}
