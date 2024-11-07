using UnityEngine; 
using UnityEngine.XR.ARFoundation; 
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class VillagePlaneManager : MonoBehaviour 
{ 

    public static VillagePlaneManager Instance;

    private ElementFilesManager elementFilesManager;
    
    private CreateButtons createButtonsComponent;

    private List<GameObject> spawnedObjects = new List<GameObject>(); // TODO: gestire questa lista

    [SerializeField] private GameObject slider;
    
    [SerializeField] private SliderMenuAnim menu;

    [SerializeField] private GameObject createButton;

    [SerializeField] private GameObject popUpElementCreated;
    [SerializeField] private GameObject popUpElementAlreadyFound;

    private VillagePlaneManager() { }

    [SerializeField] private GameObject XROrigin; 

    private ARPlaneManager planeManager; 

    private GameObject selectedObject;          // the object selected by touch interactions in the arplane

    private string selectedPrefab = null;    // the prefab selected from the menu

    private bool isPlanePlaced = false;

    private GameObject VillagePlane;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
           Destroy(this.gameObject);
           return;
        }
        else
        {
            Instance = this;
        }

    }
    
    private void Start() 
    {
        if (XROrigin == null) 
        { 
            Debug.LogError("XR Origin is not assigned"); 
            return; 
        } 
        planeManager = XROrigin.GetComponent<ARPlaneManager>(); 
        elementFilesManager = ElementFilesManager.Instance;
        
        Debug.Log("Current gamemode: " + GameModeManager.Instance.GameMode);
        elementFilesManager.RefreshVillageData();

        ElementFilesManager.VillageData villageData = elementFilesManager.GetVillageData();
        if (villageData != null)
        {
            Debug.Log("Village data loaded: " + villageData.toString());
        }
        else
        {
            Debug.LogError("Failed to load village data");
        }

        createButtonsComponent = createButton.GetComponent<CreateButtons>();
        createButtonsComponent.ResetButtons();
    } 
 

    private void Update() 
    { 
        PlaneCoords planeTouchCoords = DetectPlaneTouch();
        if (planeTouchCoords != null)
        {
            Debug.Log("Plane Touched at " + planeTouchCoords);

            if(!isPlanePlaced){
                createVillagePlane(planeTouchCoords.coords);
                isPlanePlaced = true;
                ARPlaneManager planeManager = FindObjectOfType<ARPlaneManager>();
                ARPlane[] planes = new ARPlane[planeManager.trackables.count];
                if (planeManager != null)
                {
                    planeManager.enabled = false;
                    foreach (ARPlane plane in planeManager.trackables)
                    {
                        plane.gameObject.SetActive(false);
                    }
                }

            }else if(selectedObject != null) moveSelectedObject(planeTouchCoords.coords);
            else  SpawnObject(planeTouchCoords.coords, planeTouchCoords.plane, selectedPrefab);
           
        }

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
                    } else if (hit.transform.gameObject == VillagePlane) {
                        return new PlaneCoords(hit.point, null);
                    }
                    } else {
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

    private void SpawnObject(Vector3 position, ARPlane plane, string prefabName)
    {       
        Debug.Log("Spawning a " + prefabName);
        
        if (prefabName == null) return; 

        GameObject newARObject = Instantiate(Resources.Load<GameObject>("VillagePrefabs/" + prefabName), Vector3.zero, Quaternion.Euler(0, 0, 0));
        
        newARObject.name = prefabName;
        newARObject.transform.position = position;

        newARObject.SetActive(true);
    }

    private Color GetInvertedColor(GameObject gameObject){
        // Renderer renderer = gameObject.GetComponent<Renderer>();
        // Texture2D texture = renderer.material.mainTexture as Texture2D;
        // Color averageColor = Color.black;

        // if (texture != null)
        // {
        //     Color[] pixels = texture.GetPixels();
        //     foreach (Color pixel in pixels)
        //     {
        //         averageColor += pixel;
        //     }
        //     averageColor /= pixels.Length;
        // }
        // else
        // {
        //     averageColor = renderer.material.color;
        // }

        // Color invertedColor = new Color(1 - averageColor.r, 1 - averageColor.g, 1 - averageColor.b);

        Color invertedColor = new Color(1, 1, 1);
        return invertedColor;
    }

    public void SetSelectedPrefab(string prefabName)
    {
        Debug.Log("Selected prefab: " + prefabName);
        selectedPrefab = prefabName;
    }

    public bool OnPrefabSelected(string prefabName){
        DeselectSelectedGameObject();
        SetSelectedPrefab(prefabName.ToLower());
        return false;
    }

    public void DeselectSelectedGameObject(){
        if(selectedObject != null) {
            Outline outline = selectedObject.GetComponent<Outline>();
            if (outline != null) Destroy(outline);
            selectedObject = null;
        }
    }

    public void SelectGameObject(GameObject gameObject)
    {
        Debug.Log("SelectGameObject: " + gameObject.name);
        if(selectedObject != null) DeselectSelectedGameObject();

        selectedObject = gameObject;
        Outline outline = selectedObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = GetInvertedColor(selectedObject);
        outline.OutlineWidth = 10f;
    }

    public void createVillagePlane(Vector3 position){
        GameObject newARObject = Instantiate(Resources.Load<GameObject>("VillagePlane"), Vector3.zero, Quaternion.Euler(0, 0, 0));
        
        newARObject.name = "VillagePlane";
        newARObject.transform.position = position;

        newARObject.SetActive(true);

        VillagePlane = newARObject;
    }
    Transform FindInChildren(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform result = FindInChildren(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    void moveSelectedObject(Vector3 position){
        if(selectedObject != null){
            selectedObject.transform.position = position;
            DeselectSelectedGameObject();
        }
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
            if(plane == null) return $"Coords: {coords.ToString()}";
            return $"Coords: {coords.ToString()}, Plane: {plane.ToString()}";
        }
    }

}


