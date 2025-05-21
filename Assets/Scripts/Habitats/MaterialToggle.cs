using UnityEngine;
using System.Collections.Generic;

public class MaterialStripper : MonoBehaviour
{
    [SerializeField] public Material grayMaterial;
    private HashSet<string> excludedRoots = new HashSet<string>();

    void Start()
    {
        if (grayMaterial == null)
        {
            Debug.LogWarning("Gray material non assegnato!");
            return;
        }

        LoadExcludedRoots();

        foreach (Transform child in transform) // Solo figli diretti
        {
            if (excludedRoots.Contains(child.name)) continue; // Se è escluso, salta tutto il sotto-albero

            ApplyStrip(child.gameObject);
        }
    }

    private void ApplyStrip(GameObject obj)
    {
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

    private void LoadExcludedRoots()
    {
        // TextAsset txt = Resources.Load<TextAsset>("HabitatsDone");
        // if (txt == null)
        // {
        //     Debug.LogWarning("File 'HabitatsDone.txt' non trovato in Resources.");
        //     return;
        // }

        // string[] lines = txt.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        // foreach (string line in lines)
        // {
        //     excludedRoots.Add(line.Trim());
        // }
        ElementFilesManager.Instance.RefreshVillageHabitats();
        ElementFilesManager.VillageHabitats villageHabitats = ElementFilesManager.Instance.GetVillageHabitats();
        foreach(ElementFilesManager.Habitat habitat in villageHabitats.habitats)
        {
            if (habitat.Value == 0) continue; // Se il valore è 0, non lo aggiungere
            excludedRoots.Add(habitat.Key);
        }


    }
}
