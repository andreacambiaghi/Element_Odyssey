using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HabitatsUI : MonoBehaviour
{
    private HashSet<string> completedHabitats;

    void OnEnable()
    {
        LoadCompletedHabitats();
        CheckChildrenLocks();
    }

    void LoadCompletedHabitats()
    {
        completedHabitats = new HashSet<string>();

        // TextAsset textAsset = Resources.Load<TextAsset>("HabitatsDone");
        // if (textAsset == null)
        // {
        //     Debug.LogWarning("HabitatsDone.txt not found in Resources folder.");
        //     return;
        // }

        // string[] lines = textAsset.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        // foreach (string line in lines)
        // {
        //     completedHabitats.Add(line.Trim());
        // }

        ElementFilesManager.Instance.RefreshVillageHabitats();
        ElementFilesManager.VillageHabitats villageHabitats = ElementFilesManager.Instance.GetVillageHabitats();
        foreach(ElementFilesManager.Habitat habitat in villageHabitats.habitats)
        {
            if (habitat.Value == 0) continue; // Se il valore è 0, non lo aggiungere
            completedHabitats.Add(habitat.Key);
        }
    }

    void CheckChildrenLocks()
    {
        foreach (Transform child in transform)
        {
            if (completedHabitats.Contains(child.name))
            {
                Transform lockTransform = child.Find("Lock");
                if (lockTransform != null)
                {
                    lockTransform.gameObject.SetActive(false);
                }
            }
        }
    }
}
