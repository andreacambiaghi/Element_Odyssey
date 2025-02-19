using UnityEngine;
using TMPro;
using System;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI tutorialTextDisplay;

    [SerializeField]
    private TypewriterEffectTMP typewriterEffect;
    [SerializeField]
    private PlaySoundTutorial playSoundTutorial;
    private string[] tutorialTexts = new string[] { "Inquadra marker", "Tocca la palla", "Apri il menu", "Scegli un elemento" };
    private string[] tutorialAudios = new string[] { "welcome", "touch_ball", "open_menu", "choose_element" };

    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateText();
        playSoundTutorial.Play(tutorialAudios[0]);
    }

    public void Next()
    {
        if (currentIndex < tutorialTexts.Length - 1)
        {
            currentIndex++;
            UpdateText();
        }
    }

    public void Previous()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateText();
        }
    }

    private void UpdateText()
    {
        if (typewriterEffect != null && tutorialTexts.Length > 0)
        {
            typewriterEffect.StartTyping(tutorialTexts[currentIndex]);
        }
    }
}
