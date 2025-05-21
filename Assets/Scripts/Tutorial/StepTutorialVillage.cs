using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StepTutorialVillage : MonoBehaviour
{
    public static StepTutorialVillage Instance { get; private set; }

    [SerializeField] private GameObject welcomePrefab;
    [SerializeField] private GameObject nextStepButton;
    [SerializeField] private GameObject unlockFramePrefab;
    [SerializeField] private GameObject habitatFramePrefab;
    [SerializeField] private GameObject ChangeHabitatFramePrefab;
    [SerializeField] private GameObject ChangeFloorFramePrefab;
    [SerializeField] private GameObject EndFramePrefab;

    private GameObject currentTutorialFrameInstance;

    private readonly bool[] stepCompleted = new bool[6];
    private int nextStep = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // if (ElementFilesManager.Instance != null && ElementFilesManager.Instance.GetTutorialFile())
        // {
        //     Destroy(gameObject);
        //     CleanupTutorialFrames();
        //     nextStep = 99;
        //     return;
        // }

        if (nextStep == 0 && !stepCompleted[0])
        {
            Step0();
        }
    }

    private void DestroyCurrentFrameInstance()
    {
        if (currentTutorialFrameInstance != null)
        {
            Destroy(currentTutorialFrameInstance);
            currentTutorialFrameInstance = null;
        }
    }

    private void SetCurrentFrameInstance(GameObject prefabToInstantiate)
    {
        DestroyCurrentFrameInstance();
        if (prefabToInstantiate != null)
        {
            currentTutorialFrameInstance = Instantiate(prefabToInstantiate);
            // if (UIManager.Instance != null && UIManager.Instance.TutorialCanvas != null)
            // {
            //     currentTutorialFrameInstance.transform.SetParent(UIManager.Instance.TutorialCanvas.transform, false);
            // }
        }
    }

    private void Step0()
    {
        if (stepCompleted[0]) return;
        SetCurrentFrameInstance(welcomePrefab);
        nextStepButton.SetActive(true);
        stepCompleted[0] = true;
        nextStep = 1;
    }

    private void Step1()
    {
        if (stepCompleted[1]) return;
        SetCurrentFrameInstance(unlockFramePrefab);
        stepCompleted[1] = true;
        nextStep = 2;
    }

    private void Step2()
    {
        if (stepCompleted[2]) return;
        SetCurrentFrameInstance(habitatFramePrefab);
        stepCompleted[2] = true;
        nextStep = 3;
    }

    private void Step3()
    {
        if (stepCompleted[3]) return;
        SetCurrentFrameInstance(ChangeHabitatFramePrefab);
        stepCompleted[3] = true;
        nextStep = 4;
    }

    private void Step4()
    {
        if (stepCompleted[4]) return;
        SetCurrentFrameInstance(ChangeFloorFramePrefab);
        stepCompleted[4] = true;
        nextStep = 5;
    }

    private void Step5()
    {
        if (stepCompleted[5]) return;
        SetCurrentFrameInstance(EndFramePrefab);

        if (currentTutorialFrameInstance != null)
        {
            Destroy(currentTutorialFrameInstance, 1f);
            currentTutorialFrameInstance = null;
        }

        stepCompleted[5] = true;
        nextStep = 6;

        nextStepButton.SetActive(false);

        // if (ElementFilesManager.Instance != null)
        // {
        //     ElementFilesManager.Instance.SetTutorialFile(true);
        // }
        // Destroy(gameObject, 3.5f);
    }

    public void NextStep()
    {
        if (nextStep >= stepCompleted.Length)
        {
            Debug.LogWarning("Tutorial already finished or not started.");
            return;
        }

        switch (nextStep)
        {
            case 0: Step0(); break;
            case 1: Step1(); break;
            case 2: Step2(); break;
            case 3: Step3(); break;
            case 4: Step4(); break;
            case 5: Step5(); break;
        }
    }

    private void CleanupTutorialFrames()
    {
        DestroyCurrentFrameInstance();
    }

    void OnDestroy()
    {
        CleanupTutorialFrames();
    }
}
