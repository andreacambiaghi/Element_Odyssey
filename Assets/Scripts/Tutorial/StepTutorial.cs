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
        GameObject first = Instantiate(firstElement);
        achievementButton.SetActive(true);
        StartCoroutine(ExecuteAfterTime(3f, () => 
        {
            Destroy(first);
            GameObject inventory = Instantiate(openInventory);
            StartCoroutine(ExecuteAfterTime(3f, () => 
            {
                Destroy(inventory);
                clickOpenMenu.SetActive(true);
            }));
        }));
    }

    private IEnumerator ExecuteAfterTime(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    




}
