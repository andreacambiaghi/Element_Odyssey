using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Sounds : MonoBehaviour
{
    [SerializeField] private Image soundIcon;
    // [SerializeField] private Sprite soundOnSprite;
    // [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private GameObject volumeSlider;
    [SerializeField] private Slider slider;

    void Start()
    {
        // mette lo slider al livello giusto rispetto al volume del mixer Resources.Load<AudioMixer>("Mixer").FindMatchingGroups("Master")[0];
        Resources.Load<AudioMixer>("Mixer").GetFloat("Volume", out float volume);
        // Debug.Log(volume + " volumeeeeeeeeeeeee");
        slider.value = Mathf.Pow(10, volume / 20f);
    }

    public void Click() {
        volumeSlider.SetActive(!volumeSlider.gameObject.activeSelf);
        Resources.Load<AudioMixer>("Mixer").GetFloat("Volume", out float volume);
        // Debug.Log(volume + " volumeeeeeeeeeeeee");
        // dopo 5 secondi nascondi il volumeSlider
        Invoke("HideVolumeSlider", 5);
    }
    void HideVolumeSlider() {
        volumeSlider.SetActive(false);
    }
    
}
