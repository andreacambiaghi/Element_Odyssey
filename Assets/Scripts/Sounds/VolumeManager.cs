using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    void Awake()
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
}
