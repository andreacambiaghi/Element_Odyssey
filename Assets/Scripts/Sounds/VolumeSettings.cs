using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;
    const string VOLUME_KEY = "Volume";


    void Awake()
    {
        volumeSlider.onValueChanged.AddListener(SetMusicVolume);
        audioMixer.SetFloat(VOLUME_KEY, 0);
    }

    void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(VOLUME_KEY, Mathf.Log10(volume) * 20);
        volumeSlider.value = volume;
    }

}
