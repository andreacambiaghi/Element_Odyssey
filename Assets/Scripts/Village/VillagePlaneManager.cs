using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using UnityEngine.UIElements;

public class VillagePlaneManager : MonoBehaviour
{

    public static VillagePlaneManager Instance;

    private ElementFilesManager elementFilesManager;

    private CreateButtons createButtonsComponent;

    [SerializeField] private GameObject slider;

    [SerializeField] private SliderMenuAnim menu;

    [SerializeField] private GameObject habitatsMenu;

    [SerializeField] private GameObject createButton;

    [SerializeField] private GameObject popUpElementCreated;
    [SerializeField] private GameObject popUpElementAlreadyFound;
    [SerializeField] private GameObject elementSelected;

    private VillagePlaneManager() { }

    [SerializeField] private GameObject XROrigin;

    private ARPlaneManager planeManager;

    private GameObject selectedObject;          // the object selected by touch interactions in the arplane

    private string selectedPrefab = null;    // the prefab selected from the menu

    private bool isPlanePlaced = false;

    private bool isPlacementEnabled = true;

    private GameObject VillagePlane;
    private string currentFloor = "black";

    private List<GameObject> placedObjects = new List<GameObject>();

    public bool menuOpen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
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

        // createButtonsComponent = createButton.GetComponent<CreateButtons>();
        // createButtonsComponent.ResetButtons();
    }


    private void Update()
    {

        if (menuOpen) return;

        PlaneCoords planeTouchCoords = DetectPlaneTouch();
        if (planeTouchCoords != null && isPlacementEnabled && !GameModeManager.Instance.IsMenuOpen)
        {
            Debug.Log("Plane Touched at " + planeTouchCoords);

            if (!isPlanePlaced)
            {
                createVillagePlane(planeTouchCoords.coords);
                isPlanePlaced = true;
                ARPlaneManager planeManager = FindObjectOfType<ARPlaneManager>();
                if (planeManager != null)
                {
                    planeManager.enabled = false;
                    foreach (ARPlane plane in planeManager.trackables)
                    {
                        plane.gameObject.SetActive(false);
                    }
                }
            }
            // else
            // {
            //     if (selectedObject != null)
            //     {
            //         moveSelectedObject(planeTouchCoords.coords);
            //         return;
            //     }
            //     else
            //     {
            //         // if (isDeletionInProgress)
            //         // {
            //         //     isDeletionInProgress = false;
            //         //     return;
            //         // }
            //         SpawnObject(planeTouchCoords.coords, planeTouchCoords.plane, selectedPrefab);
            //         return;
            //     }
            // }
        }

    }

    private PlaneCoords DetectPlaneTouch()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Debug.Log("Touched UI element, ignoring plane touch.");
                    return null; // Return null if a UI element was touched
                }

                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    //bool uiHit = IsPointerOverUI(touch.position);
                    //bool uiHit = IsPointerOverUI();
                    bool uiHit = IsTouchOverNonPlaneObject(hit);

                    ARPlane hitPlane = hit.transform.GetComponent<ARPlane>();
                    if (hitPlane != null && !uiHit)
                    {
                        return new PlaneCoords(hit.point, hitPlane);
                    }
                    else if (hit.transform.gameObject == VillagePlane)
                    {
                        return new PlaneCoords(hit.point, null);
                    }
                    else return null;
                }
            }
        }
        return null;
    }

    private bool IsPointerOverUI()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return true;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
        }
        return false;
    }

    private bool IsTouchOverNonPlaneObject(RaycastHit hit)
    {
        if (hit.transform.GetComponent<ARPlane>() == null && hit.transform.gameObject != VillagePlane)
        {
            return true;
        }
        return false;
    }

    private void SpawnObject(Vector3 position, ARPlane plane, string prefabName)
    {
        Debug.Log("Spawning a " + prefabName);

        if (prefabName == null) return;
        GameObject resource = Resources.Load<GameObject>("Habitats/" + prefabName);

        if (resource == null)
        {
            Debug.LogError("Resource cannot be spawned (null): " + prefabName);
            return;
        }

        // GameObject newARObject = Instantiate(resource, new Vector3(0, 1f, 0), Quaternion.Euler(0, 180, 0));
        // GameObject newARObject = Instantiate(resource, resource.transform.position, resource.transform.rotation);

        // --- MODIFICA: Istanzia come figlio del VillagePlane ---
        // Usiamo Instantiate con il quarto parametro (parent)
        // La posizione passata (position) è nel mondo, Instantiate con parent calcola la localPosition corretta
        // Vector3 spawnPos = position + Vector3.up * 0.1f;
        GameObject newARObject = Instantiate(resource, position, Quaternion.Euler(0, 180, 0), VillagePlane.transform);
        // --- FINE MODIFICA ---

        newARObject.name = prefabName; // Imposta il nome

        // La riga seguente è ora gestita direttamente dall'Instantiate che accetta il parent
        // e non va messa DOPO se la posizione è già world space.
        // newARObject.transform.position = position;

        // Le linee di scaling commentate rimangono come nel tuo originale
        // Vector3 referenceSize = new Vector3(0.2f, 0.2f, 0.2f);

        // // Calculate the scale factor
        // Vector3 prefabSize = GetPrefabSize(newARObject);
        // Vector3 scaleFactor = new Vector3(
        //     referenceSize.x / prefabSize.x,
        //     referenceSize.y / prefabSize.y,
        //     referenceSize.z / prefabSize.z
        // );

        // // Apply the scale factor
        // newARObject.transform.localScale = scaleFactor;


        placedObjects.Add(newARObject);

        newARObject.SetActive(true);

        saveCurrentConfiguration();
    }

    private Vector3 GetPrefabSize(GameObject prefab)
    {
        Renderer renderer = prefab.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Prefab does not have a Renderer component: " + prefab.name);
            return Vector3.one;
        }

        return renderer.bounds.size;
    }

    private Color GetInvertedColor(GameObject gameObject)
    {
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

    public bool OnPrefabSelected(string prefabName)
    {
        elementSelected.GetComponent<TextMeshProUGUI>().text = "You have selected " + prefabName;
        DeselectSelectedGameObject();
        SetSelectedPrefab(prefabName.ToLower());
        return false;
    }

    public void DeselectSelectedGameObject()
    {
        if (selectedObject != null)
        {
            Outline outline = selectedObject.GetComponent<Outline>();
            if (outline != null) Destroy(outline);

            selectedObject = null;
            HideHabitatsMenu();

        }
    }

    public void DeleteGameObject(GameObject gameObject)
    {
        if (gameObject == null) return;
        Debug.Log("DeleteGameObject: " + gameObject.name);
        placedObjects.Remove(gameObject);
        gameObject.SetActive(false);
        Destroy(gameObject);

        saveCurrentConfiguration();
    }

    // private bool isDeletionInProgress = false;

    public void SelectGameObject(GameObject gameObject)
    {

        if (menuOpen) return;

        if (gameObject == null) return;
        if (selectedObject == gameObject)
        {
            // isDeletionInProgress = true;

            DeselectSelectedGameObject();
            // DeleteGameObject(gameObject);

            return;
        }
        if (selectedObject != null)
        {
            DeselectSelectedGameObject();
            return;
        }

        Debug.Log("SelectGameObject: " + gameObject.name);

        // -- TEST --

        // Debug.Log("Checking if habitat is unlocked for: " + gameObject.name);

        string objectName = gameObject.name;

        // if (elementFilesManager == null)
        // {
        //     Debug.LogError("ElementFilesManager instance is null. Cannot check unlocked status.");
        //     return;
        // }

        ElementFilesManager.VillageHabitats villageHabitats = elementFilesManager.GetVillageHabitats();

        bool isUnlocked = false;
        if (villageHabitats != null && villageHabitats.habitats != null)
        {
            foreach (var habitatEntry in villageHabitats.habitats)
            {
                if (habitatEntry.Key == objectName)
                {
                    if (habitatEntry.Value > 0)
                    {
                        isUnlocked = true;
                        break;
                    }
                    else
                    {
                        isUnlocked = false;
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("VillageHabitats data is null or empty in ElementFilesManager. Cannot check unlocked status.");
            isUnlocked = false;
        }

        if(objectName.ToLower() == "default" || objectName.ToLower() == "cube")
        {
            isUnlocked = true; 
        }

        if (!isUnlocked)
        {
            Debug.Log($"Tentativo di selezionare habitat bloccato: '{objectName}'. Interazione bloccata.");
            return;
        }

        // -- FINE TEST --

        selectedObject = gameObject;

        Outline outline = selectedObject.AddComponent<Outline>();
        if (outline != null)
        {
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = GetInvertedColor(selectedObject);
            outline.OutlineWidth = 10f;
        }

        if(isUnlocked)
            ShowHabitatsMenu();
    }

    public void SetHabitatOnSelected(string habitat)
    {
        if (selectedObject == null) return;

        Debug.Log("SetHabitatOnSelected: " + selectedObject.name + " to habitat: " + habitat);

        SpawnObject(selectedObject.transform.position, null, habitat);

        placedObjects.Remove(selectedObject);
        selectedObject.SetActive(false);
        Destroy(selectedObject);

        DeselectSelectedGameObject();
    }

    public void ShowHabitatsMenu()
    {
        habitatsMenu.SetActive(true);
        menuOpen = true;
    }

    public void HideHabitatsMenu()
    {
        habitatsMenu.SetActive(false);
        menuOpen = false;
    }

    public void createVillagePlane(Vector3 position)
    {
        ElementFilesManager.VillageSaveData villageSaveData = elementFilesManager.getVillageSaveData();

        GameObject newARObject = Instantiate(Resources.Load<GameObject>("VillagePlaneTest"), Resources.Load<GameObject>("VillagePlaneTest").transform.position, Quaternion.Euler(0, 180, 0));

        newARObject.name = "VillagePlane";
        newARObject.transform.position = position;

        newARObject.SetActive(true);

        VillagePlane = newARObject;

        if (villageSaveData != null)
        {
            string floor = villageSaveData.floor;
            changeVillagePlane(floor);

            if (villageSaveData.villageObjects == null) return;

            foreach (ElementFilesManager.VillageObjectSaveData villageObject in villageSaveData.villageObjects)
            {
                SpawnObject(VillagePlane.transform.TransformPoint(villageObject.position), null, villageObject.objectName);
            }
        }
    }

    public void changeVillagePlane(string prefabName)
    {
        if (VillagePlane == null) return;

        GameObject newPlane = Resources.Load<GameObject>("Floor/Planes/" + prefabName);
        if (prefabName == null)
        {
            Debug.LogWarning("Piano non trovato: " + prefabName);
            return;
        }

        Material newMaterial = Resources.Load<Material>("Floor/Materials/" + prefabName);

        VillagePlane.GetComponent<MeshRenderer>().material = newMaterial;

        currentFloor = prefabName;

        saveCurrentConfiguration();
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

    void moveSelectedObject(Vector3 position)
    {
        if (selectedObject != null)
        {
            selectedObject.transform.position = position;
            DeselectSelectedGameObject();

            saveCurrentConfiguration();
        }
    }

    private void saveCurrentConfiguration()
    {
        ElementFilesManager.VillageSaveData villageSaveData = new ElementFilesManager.VillageSaveData();
        villageSaveData.floor = currentFloor;
        List<ElementFilesManager.VillageObjectSaveData> villageObjectSaveData = new List<ElementFilesManager.VillageObjectSaveData>();

        for (int i = 0; i < placedObjects.Count; i++)
        {
            ElementFilesManager.VillageObjectSaveData villageObject = new ElementFilesManager.VillageObjectSaveData();
            villageObject.objectName = placedObjects[i].name;
            villageObject.position = VillagePlane.transform.InverseTransformPoint(placedObjects[i].transform.position);

            villageObjectSaveData.Add(villageObject);
        }

        villageSaveData.villageObjects = villageObjectSaveData;

        elementFilesManager.UpdateVillageSaveData(villageSaveData);

    }

    public void resetConfiguration()
    {
        foreach (GameObject placedObject in placedObjects)
        {
            Destroy(placedObject);
        }
        placedObjects.Clear();

        saveCurrentConfiguration();
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
            if (plane == null) return $"Coords: {coords.ToString()}";
            return $"Coords: {coords.ToString()}, Plane: {plane.ToString()}";
        }
    }

}


