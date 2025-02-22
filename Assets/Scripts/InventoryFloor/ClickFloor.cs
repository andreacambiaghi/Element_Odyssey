using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ClickFloor : MonoBehaviour
{
    private Button floorButton;

    public XROrigin xrOrigin;

    void Start()
    {
        floorButton = GetComponent<Button>();

        if (floorButton != null)
        {
            floorButton.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        Debug.Log("Bottone cliccato: "+ gameObject.GetComponent<Image>().sprite.texture.name.Split('_')[0]);
        // UpdateARPlane(gameObject.GetComponent<Image>().sprite.texture.name.Split('_')[0]);
        VillagePlaneManager.Instance.changeVillagePlane(gameObject.GetComponent<Image>().sprite.texture.name.Split('_')[0]);
        Debug.Log("Piano cambiato: "+ ElementFilesManager.Instance.getVillageSaveData().floor);

        // floors è l'oggetto con tag Floors nella gerarchia
        GameObject floors = GameObject.FindGameObjectWithTag("Floors");

        // a tutti i figli di floors, prendo i loro relativi figli e li disattivo
        foreach (Transform child in floors.transform)
        {
            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(false);
            }
        }

        // prendo il figlio di floors con il nome del bottone cliccato e attivo il suo unico figlio
        foreach (Transform child in floors.transform)
        {
            if (child.name == gameObject.GetComponent<Image>().sprite.texture.name.Split('_')[0])
            {
                foreach (Transform grandChild in child)
                {
                    grandChild.gameObject.SetActive(true);
                }
            }
        }

    }


    // void UpdateARPlane(string selectedFloor) {
    //     xrOrigin = FindObjectOfType<XROrigin>();

    //     GameObject newPlane = Resources.Load<GameObject>("Floor/Planes/"+selectedFloor);
    //     if(newPlane == null) {
    //         Debug.LogWarning("Piano non trovato: "+selectedFloor);
    //         return;
    //     }

    //     xrOrigin.GetComponent<ARPlaneManager>().planePrefab = newPlane;
        
    // }
}
