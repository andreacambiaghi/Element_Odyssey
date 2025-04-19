using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StepTutorial : MonoBehaviour
{
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

    void Awake()
    {
        clickOpenMenu.SetActive(false);
        achievementButton.SetActive(false);
        cardBoardButton.SetActive(false);
        homeButton.SetActive(false);
        slider.SetActive(false);
        nextStepButton.SetActive(false);
        clickOpenMenu2.SetActive(false);
    }

    private void Step1()
    {
        Instantiate(waterFrame);
    }

    private void Step2()
    {
        Destroy(GameObject.Find(waterFrame.name + "(Clone)"));
        Instantiate(fireFrame);
    }

    private void Step3()
    {
        Destroy(GameObject.Find(fireFrame.name + "(Clone)"));
        Instantiate(combineElement);
    }

    private void Step4()
    {
        Destroy(GameObject.Find(combineElement.name + "(Clone)"));
        Instantiate(unionElement);
        nextStepButton.SetActive(true);
    }

    private void Step5()
    {
        Destroy(GameObject.Find(unionElement.name + "(Clone)"));
        Instantiate(openInventory);
        clickOpenMenu.SetActive(true);
        slider.SetActive(true);
        nextStepButton.SetActive(false);
    }

    private void Step6()
    {
        Destroy(GameObject.Find(openInventory.name + "(Clone)"));
        clickOpenMenu.SetActive(false);
        clickOpenMenu2.SetActive(true);
    }

    private void Step7()
    {
        clickOpenMenu2.SetActive(false);
        Instantiate(showAchievement);
        clickAchievementButton.SetActive(true);
        achievementButton.SetActive(true);
    }

    private void Step8()
    {
        Destroy(GameObject.Find(showAchievement.name + "(Clone)"));
        clickAchievementButton.SetActive(false);
    }

    private void Step9()
    {
        clickCloseAchievementButton.SetActive(false);
        Instantiate(cardBoardExplanation);
        cardBoardButton.SetActive(true);
        clickCardBoardButton.SetActive(true);
    }

    private void Step10()
    {
        Destroy(GameObject.Find(cardBoardExplanation.name + "(Clone)"));
        clickCardBoardButton.SetActive(false);
        clickCloseCardBoardButton.SetActive(true);
    }

    private void Step11()
    {
        clickCloseCardBoardButton.SetActive(false);
        Instantiate(forChange);
        nextStepButton.SetActive(true);
    }

    private void Step12()
    {
        Destroy(GameObject.Find(forChange.name + "(Clone)"));
        Instantiate(finalTutorial);
    }

    private void Step13()
    {
        Destroy(GameObject.Find(finalTutorial.name + "(Clone)"));
        homeButton.SetActive(true);
        nextStepButton.SetActive(false);
    }
    private int currentStep = 0;

    public void NextStep()
    {
        currentStep++;
        
        switch(currentStep)
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
