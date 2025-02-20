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
