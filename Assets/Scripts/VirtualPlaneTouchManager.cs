using UnityEngine; 
using UnityEngine.XR.ARFoundation; 
using UnityEngine.EventSystems;
using System.Collections.Generic;
 
public class VirtualPlaneManager : MonoBehaviour 
{ 
    [SerializeField] private GameObject XROrigin; 
    private ARPlaneManager planeManager; 
 
    private void Start() 
    { 
        if (XROrigin == null) 
        { 
            Debug.LogError("XR Origin is not assigned"); 
            return; 
        } 
 
        planeManager = XROrigin.GetComponent<ARPlaneManager>(); 
    } 
 
    private void Update() 
    { 
        Vector3? planeTouchCoords = DetectPlaneTouch();
        if (planeTouchCoords != null)
        {
            Debug.Log("Plane Touched at " + planeTouchCoords);
        }
        
        // if (planeManager.enabled == true && Input.touchCount > 0) 
        // if (Input.touchCount > 0) 
        // { 
        //     Touch touch = Input.GetTouch(0); 
        //     if (touch.phase == TouchPhase.Ended) 
        //     { 
        //         Vector3 clickedPlane = DetectPlaneTouch(touch); 
        //         if (clickedPlane != null) 
        //         { 
        //             planeManager.enabled = false; // stopping plane search 
 
        //             // handling planes 
        //             foreach (ARPlane plane in planeManager.trackables) 
        //             { 
        //                 if (plane == clickedPlane) {
        //                     //Garden.Instance.SetGardenPlane(clickedPlane);   
        //                     //Debug.Log("Plane Touched at " + clickedPlane.transform.position); 
        //                 }
        //                 else 
        //                 { 
        //                     // plane.gameObject.SetActive(false); 
        //                     // Destroy(plane.gameObject); 
        //                 } 
        //             } 
 
        //             //this.enabled = false; // stopping the script 
        //         } 
        //     } 
        // } 
    } 
 
    // private ARPlane DetectPlaneTouch(Touch touch) 
    // { 
    //     Ray ray = Camera.main.ScreenPointToRay(touch.position); 
    //     if (Physics.Raycast(ray, out RaycastHit hit)) 
    //     { 
    //         ARPlane hitPlane = hit.transform.GetComponent<ARPlane>(); 
    //         if (hitPlane != null){
    //             Debug.Log("Plane Touched at " + hitPlane.transform.position);
    //             Debug.Log("Plane Touched at " + hit.transform.position);
    //             return hitPlane; 
    //         }
 
    //         return null; 
    //     } 
 
    //     return null; 
    // } 

    private Vector3? DetectPlaneTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    bool uiHit = IsPointerOverUI(touch.position);
                    //bool gardenHit = hit.collider.gameObject == plane.GetComponent<MeshCollider>().gameObject;

                    if (!uiHit)
                        return hit.point;
                    else
                        Debug.Log("UI Hit");
                    
                    return null;
                }
            }
        }
        return null;
    }

    private bool IsPointerOverUI(Vector2 pos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = pos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

}


