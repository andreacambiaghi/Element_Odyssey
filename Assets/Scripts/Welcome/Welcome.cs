using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class Welcome : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tutorialTextDisplay;

    [SerializeField]
    private TypewriterEffectTMP typewriterEffect;

    private AudioSource audioSource;

    private void Start()
    {
        typewriterEffect.StartTyping(tutorialTextDisplay.text); // Inizia a scrivere il testo

        // Ottieni il componente AudioSource
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && audioSource.clip != null) // Assicurati che ci sia un clip assegnato
        {
            audioSource.Play(); // Riproduci il clip già assegnato
        }
        else
        {
            Debug.LogWarning("AudioSource o AudioClip non trovati su " + gameObject.name);
        }

        // Dopo 7 secondi il gameObject verrà distrutto
        Destroy(gameObject, 7f);
    }
}
