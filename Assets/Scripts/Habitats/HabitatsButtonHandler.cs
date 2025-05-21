using UnityEngine;

public class HabitatsButtonHandler : MonoBehaviour
{
    [SerializeField] private string habitat;

    public void OnButtonClick()
    {

        // foreach (Transform child in transform)
        // {
        //     if (child.name == "Lock")
        //     {
        //         Debug.Log("Il bottone è bloccato!");
        //         return;
        //     }
        // }

        foreach (ElementFilesManager.Habitat h in ElementFilesManager.Instance.GetVillageHabitats().habitats)
        {
            if (h.Key == habitat)
            {
                if (h.Value == 0)
                {
                    Debug.Log("Il bottone è bloccato!");
                    return;
                }
            }
        }


        VillagePlaneManager vpm = VillagePlaneManager.Instance;

        if (vpm == null)
        {
            Debug.LogError("VillagePlaneManager non trovato!");
            return;
        }

        vpm.SetHabitatOnSelected(habitat);

    }
}