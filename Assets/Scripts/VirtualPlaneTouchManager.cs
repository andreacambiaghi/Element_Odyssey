using UnityEngine; 
using UnityEngine.XR.ARFoundation; 
using UnityEngine.EventSystems;
using System.Collections.Generic;
 
public class VirtualPlaneManager : MonoBehaviour 
{ 

    private static VirtualPlaneManager instance;

    public static VirtualPlaneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new VirtualPlaneManager();
            }
            return instance;
        }
    }
    private VirtualPlaneManager() { }

    [SerializeField] private GameObject XROrigin; 

    private ARPlaneManager planeManager; 

    private string selectedPrefab;
 
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
        // Vector3? planeTouchCoords = DetectPlaneTouch();
        // if (planeTouchCoords != null)
        // {
        //     Debug.Log("Plane Touched at " + planeTouchCoords);
        //     SpawnObject((Vector3)planeTouchCoords, "fire");
        // }

        PlaneCoords planeTouchCoords = DetectPlaneTouch();
        if (planeTouchCoords != null)
        {
            Debug.Log("Plane Touched at " + planeTouchCoords);
            // Debug.Log("Plane Touched at " + planeTouchCoords.plane.transform.position);
            // Debug.Log("Plane Touched at " + planeTouchCoords.coords);
            if (selectedPrefab != null)
                SpawnObject(planeTouchCoords.coords, planeTouchCoords.plane, selectedPrefab);
            else
                SpawnObject(planeTouchCoords.coords, planeTouchCoords.plane, "fire");
        }

    } 


     /*
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
        */
 
    private ARPlane DetectPlane(Touch touch) 
    { 
        Ray ray = Camera.main.ScreenPointToRay(touch.position); 
        if (Physics.Raycast(ray, out RaycastHit hit)) 
        { 
            ARPlane hitPlane = hit.transform.GetComponent<ARPlane>(); 
            if (hitPlane != null){
                // Debug.Log("Plane Touched at " + hitPlane.transform.position);
                // Debug.Log("Plane Touched at " + hit.transform.position);
                return hitPlane; 
            }
 
            return null; 
        } 
 
        return null; 
    } 

    private PlaneCoords DetectPlaneTouch()
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
                    
                    ARPlane hitPlane = hit.transform.GetComponent<ARPlane>(); 
                    if (hitPlane != null && !uiHit){
                        return new PlaneCoords(hit.point, hitPlane); 
                    }
                    return null;
                }
            }
        }
        return null;
    }

    // private Vector3? DetectPlaneTouch()
    // {
    //     if (Input.touchCount > 0)
    //     {
    //         Touch touch = Input.GetTouch(0);
    //         if (touch.phase == TouchPhase.Ended)
    //         {
    //             Ray ray = Camera.main.ScreenPointToRay(touch.position);
    //             if (Physics.Raycast(ray, out RaycastHit hit))
    //             {
    //                 bool uiHit = IsPointerOverUI(touch.position);
                    

    //                 if (!uiHit)
    //                     return hit.point;
    //                 else
    //                     Debug.Log("UI Hit");
                    
    //                 return null;
    //             }
    //         }
    //     }
    //     return null;
    // }

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


    private void SpawnObject(Vector3 position, ARPlane plane, string prefabName)
    {
        GameObject newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
        newARObject.transform.position = position;

        //move the object higher the height of itself so that it is not inside the plane
        // newARObject.transform.position = new Vector3(newARObject.transform.position.x, plane.transform.position.y + newARObject.transform.localScale.y/2, newARObject.transform.position.z); 

        newARObject.SetActive(true);
        // Instantiate(prefab, position, Quaternion.identity);
    }

    
    public void SetSelectedPrefab(string prefabName)
    {
        Debug.Log("Selected prefab: " + prefabName);
        selectedPrefab = prefabName;
    }

    public bool OnPrefabSelected(string prefabName){
        SetSelectedPrefab(prefabName);

        return false;
    }

    public void SelectGameObject(GameObject gameObject)
    {
        // DeselectAllObjects();
        // selectedObject = gameObject;
        // objectRenderer.material.color = selectedColor;
    }


    private class PlaneCoords
    {
        public Vector3 coords { get; set; }
        public ARPlane plane { get; set; }

        public PlaneCoords(Vector3 coords, ARPlane plane)
        {
            this.coords = coords;
            this.plane = plane;
        }

        public override string ToString()
        {
            return $"Coords: {coords.ToString()}, Plane: {plane.ToString()}";
        }
    }
}


