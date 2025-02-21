using System;
using UnityEngine;
using UnityEngine.Audio;

public class PlaySoundTutorial : MonoBehaviour
{
    public void Play(string fileName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"AudioTutorial/{fileName}");

        if (clip != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = Resources.Load<AudioMixer>("Mixer").FindMatchingGroups("Master")[0];

            if (audioSource != null)
            {
                audioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogError("AudioSource component is missing on this GameObject.");
            }
        }
        else
        {
            Debug.LogError($"Audio file '{fileName}' not found in Resources/AudioTutorial.");
        }
    }
}
