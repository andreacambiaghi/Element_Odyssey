using UnityEngine;
using System.Collections.Generic;

public class MaterialStripper : MonoBehaviour
{
    [SerializeField] private List<GameObject> targetObjects = new List<GameObject>();
    [SerializeField] public Material grayMaterial;

    void Start()
    {
        if (grayMaterial == null)
        {
            Debug.LogWarning("Gray material non assegnato!");
            return;
        }

        foreach (GameObject obj in targetObjects)
        {
            if (obj == null) continue;

            // 🔹 Cambia materiali
            foreach (Renderer rend in obj.GetComponentsInChildren<Renderer>())
            {
                Material[] grayArray = new Material[rend.sharedMaterials.Length];
                for (int i = 0; i < grayArray.Length; i++)
                    grayArray[i] = grayMaterial;

                rend.sharedMaterials = grayArray;
            }

            // 🔸 Disattiva Animator
            foreach (Animator animator in obj.GetComponentsInChildren<Animator>())
            {
                animator.enabled = false;
            }

            // 🔸 Ferma audio
            foreach (AudioSource audio in obj.GetComponentsInChildren<AudioSource>())
            {
                audio.Stop();
                audio.enabled = false;
            }
        }
    }
}
