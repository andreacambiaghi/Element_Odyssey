using UnityEngine;
using System.Collections.Generic;

public class MaterialToggle : MonoBehaviour
{
    public List<GameObject> targetObjects = new List<GameObject>();
    public Material grayMaterial;

    // Mappa: ogni oggetto → lista dei suoi materiali originali per ogni renderer
    private Dictionary<GameObject, Material[][]> savedMaterials = new Dictionary<GameObject, Material[][]>();
    private HashSet<GameObject> modifiedObjects = new HashSet<GameObject>();

    public void ReplaceWithGrayMaterial(int index)
    {
        if (grayMaterial == null)
        {
            Debug.LogWarning("Gray material non assegnato!");
            return;
        }

        if (index < 0 || index >= targetObjects.Count) return;

        GameObject target = targetObjects[index];
        if (modifiedObjects.Contains(target)) return;

        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        Material[][] originalMats = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer rend = renderers[i];

            // Salva materiali originali
            originalMats[i] = rend.sharedMaterials;

            // Crea e assegna array grigio
            Material[] grayArray = new Material[rend.sharedMaterials.Length];
            for (int j = 0; j < grayArray.Length; j++)
                grayArray[j] = grayMaterial;

            rend.sharedMaterials = grayArray;
        }

        savedMaterials[target] = originalMats;
        modifiedObjects.Add(target);
    }

    public void RestoreMaterials(int index)
    {
        if (index < 0 || index >= targetObjects.Count) return;

        GameObject target = targetObjects[index];
        if (!modifiedObjects.Contains(target)) return;
        if (!savedMaterials.ContainsKey(target)) return;

        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        Material[][] originalMats = savedMaterials[target];

        for (int i = 0; i < renderers.Length; i++)
        {
            if (i < originalMats.Length)
            {
                renderers[i].sharedMaterials = originalMats[i];
            }
        }

        savedMaterials.Remove(target);
        modifiedObjects.Remove(target);
    }
}
