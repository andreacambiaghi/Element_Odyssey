using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StepTutorialVillage : MonoBehaviour
{

    public static StepTutorialVillage Instance { get; private set; }

    [SerializeField] private GameObject welcome;
    [SerializeField] private GameObject nextStepButton;
    [SerializeField] private GameObject unlockFrame;
    [SerializeField] private GameObject habitatFrame;
    [SerializeField] private GameObject ChangeHabitatFrame;
    [SerializeField] private GameObject ChangeFloorFrame;
    [SerializeField] private GameObject EndFrame;

    // array di bool per gli step.. possono essere fatti solo una volta.. metti che il marker viene inquadrato due volte? o apro troppo spesso l'inventario?
    // non voglio che il tutorial si ripeta.. quindi mettiamo un array di bool per ogni step.. se è true non lo faccio più
    private readonly bool[] stepCompleted = new bool[7];
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
        }

        // if (ElementFilesManager.Instance.GetTutorialFile())
        // {
        //     Destroy(gameObject);
        //     nextStep = 99;
        //     return;
        // }

        if (nextStep == 0) Step0();
    }
    private void Step0()
    {
        if (stepCompleted[0]) return; // Se lo step è già completato, non fare nulla
        Instantiate(welcome);
        nextStepButton.SetActive(true);
        stepCompleted[0] = true; // Segna lo step come completato
        nextStep = 1;
    }

    private void Step1()
    {
        if (stepCompleted[1]) return; // Se lo step è già completato, non fare nulla
        Destroy(welcome);
        Instantiate(unlockFrame);
        stepCompleted[1] = true; // Segna lo step come completato
        nextStep = 2;
    }

    private void Step2()
    {
        if (stepCompleted[2]) return; // Se lo step è già completato, non fare nulla
        Destroy(unlockFrame);
        Instantiate(habitatFrame);
        stepCompleted[2] = true; // Segna lo step come completato
        nextStep = 3;
    }

    private void Step3()
    {
        if (stepCompleted[3]) return; // Se lo step è già completato, non fare nulla
        Destroy(habitatFrame);
        Instantiate(ChangeHabitatFrame);
        stepCompleted[3] = true; // Segna lo step come completato
        nextStep = 4;
    }

    private void Step4()
    {
        if (stepCompleted[4]) return; // Se lo step è già completato, non fare nulla
        Destroy(ChangeHabitatFrame);
        Instantiate(ChangeFloorFrame);
        stepCompleted[4] = true; // Segna lo step come completato
        nextStep = 5;
    }

    private void Step5()
    {
        if (stepCompleted[5]) return; // Se lo step è già completato, non fare nulla
        Destroy(ChangeFloorFrame);
        Instantiate(EndFrame);
        stepCompleted[5] = true; // Segna lo step come completato
        nextStep = 6;
    }
    private void Step6()
    {
        if (stepCompleted[6]) return; // Se lo step è già completato, non fare nulla
        Destroy(EndFrame);
        nextStepButton.SetActive(false);
        stepCompleted[6] = true; // Segna lo step come completato
        nextStep = 7;
        nextStepButton.SetActive(false);
    }
    public void NextStep()
    {
        switch (nextStep)
        {
            case 0:
                Step0();
                break;
            case 1:
                Step1();
                break;
            case 2:
                Step2();
                break;
            case 3:
                Step3();
                break;
            case 4:
                Step4();
                break;
            case 5:
                Step5();
                break;
            case 6:
                Step6();
                break;
        }
    }

}
