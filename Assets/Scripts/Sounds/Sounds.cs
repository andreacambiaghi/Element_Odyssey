using UnityEngine;
using UnityEngine.UI;

public class Sounds : MonoBehaviour
{
    [SerializeField] private Image soundIcon;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private void Start()
    {
        UpdateSoundIcon();
    }

    public void ToggleSound()
    {
        SoundManager.Instance.ToggleSound();
        UpdateSoundIcon();
    }

    private void UpdateSoundIcon()
    {
        if (soundIcon != null)
        {
            soundIcon.sprite = SoundManager.Instance.IsSoundOn() ? soundOnSprite : soundOffSprite;
        }
    }
}
