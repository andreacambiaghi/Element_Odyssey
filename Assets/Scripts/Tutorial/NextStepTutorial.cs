using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStepTutorial : MonoBehaviour
{
    public void NextStep()
    {
        StepTutorial.Instance.NextStep();
    }
}
