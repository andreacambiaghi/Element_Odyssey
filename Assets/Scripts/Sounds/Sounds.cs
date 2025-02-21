using UnityEngine;
using UnityEngine.UI;

public class Sounds : MonoBehaviour
{
    [SerializeField] private Image soundIcon;
    // [SerializeField] private Sprite soundOnSprite;
    // [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private GameObject volumeSlider;

    public void Click() {
        volumeSlider.SetActive(!volumeSlider.gameObject.activeSelf);
        // dopo 5 secondi nascondi il volumeSlider
        Invoke("HideVolumeSlider", 5);
    }
    void HideVolumeSlider() {
        volumeSlider.SetActive(false);
    }
    
}
