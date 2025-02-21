using UnityEngine;
using UnityEngine.Audio;

public class PlaySoundOnClick : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = Resources.Load<AudioMixer>("Mixer").FindMatchingGroups("Master")[0];

    }

    void OnMouseDown()
    {
        if (audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
