using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private bool isSoundOn = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene l'istanza tra i cambi di scena
        }
        else
        {
            Destroy(gameObject); // Distrugge i duplicati
        }
    }

    public bool IsSoundOn()
    {
        return isSoundOn;
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
    }

    public void SetSound(bool isOn)
    {
        isSoundOn = isOn;
    }
}
