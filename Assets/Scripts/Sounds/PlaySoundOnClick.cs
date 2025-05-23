using UnityEngine;
using UnityEngine.Audio;

public class PlaySoundOnClick : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private Material grayMaterial;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Imposta Mixer se disponibile
        AudioMixer mixer = Resources.Load<AudioMixer>("Mixer");
        if (mixer != null)
            audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[0];
    }

    void OnMouseDown()
    {
        if (VillagePlaneManager.Instance.menuOpen) return;

        if (audioSource.clip == null) return;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null || renderer.sharedMaterial == null) return;

        // Se il materiale è il grigio, non riprodurre
        if (grayMaterial != null && renderer.sharedMaterial == grayMaterial) return;

        audioSource.PlayOneShot(audioSource.clip);
    }
}
