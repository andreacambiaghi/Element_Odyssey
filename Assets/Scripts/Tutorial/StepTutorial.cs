using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StepTutorial : MonoBehaviour
{

    public static StepTutorial Instance { get; private set; }

    [SerializeField] private GameObject welcome;
    [SerializeField] private GameObject clickOpenMenu;
    [SerializeField] private GameObject clickOpenMenu2;
    [SerializeField] private GameObject waterFrame;
    [SerializeField] private GameObject fireFrame;
    [SerializeField] private GameObject achievementButton;
    [SerializeField] private GameObject cardBoardButton;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private GameObject slider;
    [SerializeField] private GameObject unionElement;
    [SerializeField] private GameObject showAchievement;
    [SerializeField] private GameObject openInventory;
    [SerializeField] private GameObject clickAchievementButton;
    [SerializeField] private GameObject clickCardBoardButton;
    [SerializeField] private GameObject combineElement;
    [SerializeField] private GameObject cardBoardExplanation;
    [SerializeField] private GameObject nextStepButton;
    [SerializeField] private GameObject clickCloseAchievementButton;
    [SerializeField] private GameObject clickCloseCardBoardButton;
    [SerializeField] private GameObject forChange;
    [SerializeField] private GameObject finalTutorial;

    // array di bool per gli step.. possono essere fatti solo una volta.. metti che il marker viene inquadrato due volte? o apro troppo spesso l'inventario?
    // non voglio che il tutorial si ripeta.. quindi mettiamo un array di bool per ogni step.. se è true non lo faccio più
    private readonly bool[] stepCompleted = new bool[14];
    private int nextStep = 1;

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

        clickOpenMenu.SetActive(false);
        achievementButton.SetActive(false);
        cardBoardButton.SetActive(false);
        homeButton.SetActive(false);
        slider.SetActive(false);
        nextStepButton.SetActive(false);
        clickOpenMenu2.SetActive(false);

        if (nextStep == 1) Step1();
    }

    public void FrameMarker(string type)
    {
        int val = int.Parse(type);
        if (val >= 1 && val <= 5)
        {
            if(nextStep == 2) Step2();
        }
        else if (val >= 6 && val <= 10)
        {
            if(nextStep == 3) Step3();
        }

    }

    public void UnionElement()
    {
        if (nextStep == 4) Step4();
    }

    public void OpenInventory()
    {
        if (nextStep == 6) Step6();
    }

    public void CloseInventory()
    {
        if (nextStep == 7) Step7();
    }

    public void ShowAchievement()
    {
        if (nextStep == 8) Step8();
    }

    public void CloseAchievement()
    {
        if (nextStep == 9) Step9();
    }
    public void ShowCardBoard()
    {
        if (nextStep == 10) Step10();
    }

    public void CloseCardBoard()
    {
        if (nextStep == 11) Step11();
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
        Destroy(GameObject.Find(welcome.name + "(Clone)"));
        nextStepButton.SetActive(false);
        Instantiate(waterFrame);
        stepCompleted[1] = true; // Segna lo step come completato
        nextStep = 2;
    }

    private void Step2()
    {
        if (stepCompleted[2]) return; // Se lo step è già completato, non fare nulla
        Destroy(GameObject.Find(waterFrame.name + "(Clone)"));
        Instantiate(fireFrame);
        stepCompleted[2] = true; // Segna lo step come completato
        nextStep = 3;
    }

    private void Step3()
    {
        if (stepCompleted[3]) return; // Se lo step è già completato, non fare nulla
        Destroy(GameObject.Find(fireFrame.name + "(Clone)"));
        Instantiate(combineElement);
        stepCompleted[3] = true; // Segna lo step come completato
        nextStep = 4;
    }

    private void Step4()
    {
        if (stepCompleted[4]) return; // Se lo step è già completato, non fare nulla
        Destroy(GameObject.Find(combineElement.name + "(Clone)"));
        Instantiate(unionElement);
        nextStepButton.SetActive(true);
        stepCompleted[4] = true; // Segna lo step come completato
        nextStep = 5;
    }

    private void Step5()
    {
        if (stepCompleted[5]) return; // Se lo step è già completato, non fare nulla
        Destroy(GameObject.Find(unionElement.name + "(Clone)"));
        Instantiate(openInventory);
        clickOpenMenu.SetActive(true);
        slider.SetActive(true);
        nextStepButton.SetActive(false);
        stepCompleted[5] = true; // Segna lo step come completato
        nextStep = 6;
    }

    private void Step6()
    {
        if (stepCompleted[6]) return; // Se lo step è già completato, non fare nulla
        Destroy(GameObject.Find(openInventory.name + "(Clone)"));
        clickOpenMenu.SetActive(false);
        clickOpenMenu2.SetActive(true);
        stepCompleted[6] = true; // Segna lo step come completato
        nextStep = 7;
    }

    private void Step7()
    {
        if (stepCompleted[7]) return; // Se lo step è già completato, non fare nulla
        clickOpenMenu2.SetActive(false);
        Instantiate(showAchievement);
        clickAchievementButton.SetActive(true);
        achievementButton.SetActive(true);
        stepCompleted[7] = true; // Segna lo step come completato
        nextStep = 8;
    }

    private void Step8()
    {
        if (stepCompleted[8]) return; // Se lo step è già completato, non fare nulla
        Destroy(GameObject.Find(showAchievement.name + "(Clone)"));
        clickAchievementButton.SetActive(false);
        clickCloseAchievementButton.SetActive(true);
        stepCompleted[8] = true; // Segna lo step come completato
        nextStep = 9;
    }

    private void Step9()
    {
        if (stepCompleted[9]) return; // Se lo step è già completato, non fare nulla
        clickCloseAchievementButton.SetActive(false);
        Instantiate(cardBoardExplanation);
        cardBoardButton.SetActive(true);
        clickCardBoardButton.SetActive(true);
        stepCompleted[9] = true; // Segna lo step come completato
        nextStep = 10;
    }

    private void Step10()
    {
        if (stepCompleted[10]) return; // Se lo step è già completato, non fare nulla
        Destroy(GameObject.Find(cardBoardExplanation.name + "(Clone)"));
        clickCardBoardButton.SetActive(false);
        clickCloseCardBoardButton.SetActive(true);
        stepCompleted[10] = true; // Segna lo step come completato
        nextStep = 11;
    }

    private void Step11()
    {
        if (stepCompleted[11]) return; // Se lo step è già completato, non fare nulla
        clickCloseCardBoardButton.SetActive(false);
        Instantiate(forChange);
        nextStepButton.SetActive(true);
        stepCompleted[11] = true; // Segna lo step come completato
        nextStep = 12;
    }

    private void Step12()
    {
        if (stepCompleted[12]) return; // Se lo step è già completato, non fare nulla
        Destroy(GameObject.Find(forChange.name + "(Clone)"));
        Instantiate(finalTutorial);
        stepCompleted[12] = true; // Segna lo step come completato
        nextStep = 13;
    }

    private void Step13()
    {
        if (stepCompleted[13]) return; // Se lo step è già completato, non fare nulla
        Destroy(GameObject.Find(finalTutorial.name + "(Clone)"));
        homeButton.SetActive(true);
        nextStepButton.SetActive(false);
        stepCompleted[13] = true; // Segna lo step come completato
        nextStep = 14;
    }

    public void NextStep()
    {
        switch(nextStep)
        {
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
            case 7:
                Step7();
                break;
            case 8:
                Step8();
                break;
            case 9:
                Step9();
                break;
            case 10:
                Step10();
                break;
            case 11:
                Step11();
                break;
            case 12:
                Step12();
                break;
            case 13:
                Step13();
                break;
            // Aggiungi altri case per step aggiuntivi
        }
    }

}
