using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StepTutorial : MonoBehaviour
{
    [SerializeField] private GameObject clickOpenMenu;
    [SerializeField] private GameObject waterFrame;
    [SerializeField] private GameObject fireFrame;
    [SerializeField] private GameObject achievementButton;
    [SerializeField] private GameObject cardBoardButton;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private GameObject slider;
    [SerializeField] private GameObject unionElement;
    [SerializeField] private GameObject firstElement;
    [SerializeField] private GameObject openInventory;
    [SerializeField] private GameObject clickAchievementButton;
    [SerializeField] private GameObject clickCardBoardButton;

    void Awake()
    {
        clickOpenMenu.SetActive(false);
        achievementButton.SetActive(false);
        cardBoardButton.SetActive(false);
        homeButton.SetActive(false);
        slider.SetActive(false);
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
        GameObject first = Instantiate(firstElement, firstElement.transform.position, Quaternion.identity);
        StartCoroutine(ExecuteAfterTime(3f, () => 
        {
            Destroy(first);
            GameObject inventory = Instantiate(openInventory, openInventory.transform.position, Quaternion.identity);
            StartCoroutine(ExecuteAfterTime(3f, () => 
            {
                Destroy(inventory);
                clickOpenMenu.SetActive(true);
                slider.SetActive(true);
                achievementButton.SetActive(true);
            }));
        }));
    }

    private IEnumerator ExecuteAfterTime(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
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
            // Aggiungi altri case per step aggiuntivi
        }
    }

}
