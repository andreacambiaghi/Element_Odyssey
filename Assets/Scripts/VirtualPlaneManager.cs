// using UnityEngine; 
// using UnityEngine.XR.ARFoundation; 
// using UnityEngine.EventSystems;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine.UI;
// using Unity.VisualScripting;

// public class VirtualPlaneManager : MonoBehaviour 
// { 

//     public static VirtualPlaneManager Instance;

//     private ElementFilesManager elementFilesManager;
    
//     private CreateButtons createButtonsComponent;

//     private List<GameObject> spawnedObjects = new List<GameObject>(); // TODO: gestire questa lista

//     private List<string> _othersElements;

//     [SerializeField] private GameObject slider;
    
//     [SerializeField] private SliderMenuAnim menu;

//     [SerializeField] private GameObject createButton;

//     [SerializeField] private GameObject popUpElementCreated;
//     [SerializeField] private GameObject popUpElementAlreadyFound;

//     private VirtualPlaneManager() { }

//     [SerializeField] private GameObject XROrigin; 

//     private ARPlaneManager planeManager; 

//     private string selectedPrefab = "water";
 

//     private void Awake()
//     {
//         if(Instance != null && Instance != this)
//         {
//            Destroy(this.gameObject);
//            return;
//         }
//         else
//         {
//             Instance = this;
//         }
        

//         _othersElements = ElementFilesManager.Instance.GetOthersElements();
//         Debug.LogWarning("Others elements: " + _othersElements.Count);
//     }
    
//     private void Start() 
//     { 

//         if (XROrigin == null) 
//         { 
//             Debug.LogError("XR Origin is not assigned"); 
//             return; 
//         } 
 
//         planeManager = XROrigin.GetComponent<ARPlaneManager>(); 
//         elementFilesManager = ElementFilesManager.Instance;
//         createButtonsComponent = createButton.GetComponent<CreateButtons>();
//     } 
 
//     private void Update() 
//     { 
//         // Vector3? planeTouchCoords = DetectPlaneTouch();
//         // if (planeTouchCoords != null)
//         // {
//         //     Debug.Log("Plane Touched at " + planeTouchCoords);
//         //     SpawnObject((Vector3)planeTouchCoords, "fire");
//         // }

//         PlaneCoords planeTouchCoords = DetectPlaneTouch();
//         if (planeTouchCoords != null)
//         {
//             Debug.Log("Plane Touched at " + planeTouchCoords);
//             // Debug.Log("Plane Touched at " + planeTouchCoords.plane.transform.position);
//             // Debug.Log("Plane Touched at " + planeTouchCoords.coords);
//             // if (selectedPrefab != null){}
//             //     SpawnObject(planeTouchCoords.coords, planeTouchCoords.plane, selectedPrefab);
//             // else
//             //     SpawnObject(planeTouchCoords.coords, planeTouchCoords.plane, "fire");

//             SpawnObject(planeTouchCoords.coords, planeTouchCoords.plane, selectedPrefab);
//         }

//     } 

//      /*
//         // if (planeManager.enabled == true && Input.touchCount > 0) 
//         // if (Input.touchCount > 0) 
//         // { 
//         //     Touch touch = Input.GetTouch(0); 
//         //     if (touch.phase == TouchPhase.Ended) 
//         //     { 
//         //         Vector3 clickedPlane = DetectPlaneTouch(touch); 
//         //         if (clickedPlane != null) 
//         //         { 
//         //             planeManager.enabled = false; // stopping plane search 
 
//         //             // handling planes 
//         //             foreach (ARPlane plane in planeManager.trackables) 
//         //             { 
//         //                 if (plane == clickedPlane) {
//         //                     //Garden.Instance.SetGardenPlane(clickedPlane);   
//         //                     //Debug.Log("Plane Touched at " + clickedPlane.transform.position); 
//         //                 }
//         //                 else 
//         //                 { 
//         //                     // plane.gameObject.SetActive(false); 
//         //                     // Destroy(plane.gameObject); 
//         //                 } 
//         //             } 
 
//         //             //this.enabled = false; // stopping the script 
//         //         } 
//         //     } 
//         // } 
//         */
 
//     private ARPlane DetectPlane(Touch touch) 
//     { 
//         Ray ray = Camera.main.ScreenPointToRay(touch.position); 
//         if (Physics.Raycast(ray, out RaycastHit hit)) 
//         { 
//             ARPlane hitPlane = hit.transform.GetComponent<ARPlane>(); 
//             if (hitPlane != null){
//                 // Debug.Log("Plane Touched at " + hitPlane.transform.position);
//                 // Debug.Log("Plane Touched at " + hit.transform.position);
//                 return hitPlane; 
//             }
 
//             return null; 
//         } 
 
//         return null; 
//     } 

//     private PlaneCoords DetectPlaneTouch()
//     {
//         if (Input.touchCount > 0)
//         {
//             Touch touch = Input.GetTouch(0);
//             if (touch.phase == TouchPhase.Ended)
//             {
//                 Ray ray = Camera.main.ScreenPointToRay(touch.position);
//                 if (Physics.Raycast(ray, out RaycastHit hit))
//                 {
//                     bool uiHit = IsPointerOverUI(touch.position);
                    
//                     ARPlane hitPlane = hit.transform.GetComponent<ARPlane>(); 
//                     if (hitPlane != null && !uiHit){
//                         return new PlaneCoords(hit.point, hitPlane); 
//                     }
//                     return null;
//                 }
//             }
//         }
//         return null;
//     }

//     // private Vector3? DetectPlaneTouch()
//     // {
//     //     if (Input.touchCount > 0)
//     //     {
//     //         Touch touch = Input.GetTouch(0);
//     //         if (touch.phase == TouchPhase.Ended)
//     //         {
//     //             Ray ray = Camera.main.ScreenPointToRay(touch.position);
//     //             if (Physics.Raycast(ray, out RaycastHit hit))
//     //             {
//     //                 bool uiHit = IsPointerOverUI(touch.position);
                    

//     //                 if (!uiHit)
//     //                     return hit.point;
//     //                 else
//     //                     Debug.Log("UI Hit");
                    
//     //                 return null;
//     //             }
//     //         }
//     //     }
//     //     return null;
//     // }

//     private bool IsPointerOverUI(Vector2 pos)
//     {
//         PointerEventData eventData = new PointerEventData(EventSystem.current)
//         {
//             position = pos
//         };

//         List<RaycastResult> results = new List<RaycastResult>();
//         EventSystem.current.RaycastAll(eventData, results);

//         return results.Count > 0;
//     }


//     private void SpawnObject(Vector3 position, ARPlane plane, string prefabName)
//     {
//         Debug.Log("Spawning a " + prefabName);
        
//         GameObject newARObject;
//         if (_othersElements.Contains(prefabName)) {
//             newARObject = Instantiate(Resources.Load<GameObject>("other"), Vector3.zero, Quaternion.Euler(0, 0, 0));
//             // newARObject = Create3DText.Instance.CreateTextObject(prefabName.ToUpper());
//             // ComponentAdder ca = new();
//             // ca.AddComponentsToGameObject(newARObject);
//             TextMeshProUGUI[] texts = newARObject.GetComponentsInChildren<TextMeshProUGUI>();
//             foreach (TextMeshProUGUI text in texts)
//             {
//                 text.text = prefabName;
//             }
//         }
//         else
//         {
//             newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
//         }
        
//         newARObject.name = prefabName;
//         newARObject.transform.position = position;

//         //move the object higher the height of itself so that it is not inside the plane
//         // newARObject.transform.position = new Vector3(newARObject.transform.position.x, plane.transform.position.y + newARObject.transform.localScale.y/2, newARObject.transform.position.z); 

//         newARObject.SetActive(true);
//         // Instantiate(prefab, position, Quaternion.identity);
//     }

//     public void SetSelectedPrefab(string prefabName)
//     {
//         Debug.Log("Selected prefab: " + prefabName);
//         selectedPrefab = prefabName;
//     }

//     public bool OnPrefabSelected(string prefabName){
//         SetSelectedPrefab(prefabName.ToLower());

//         return false;
//     }

//     public void SelectGameObject(GameObject gameObject)
//     {
//         // TODO: implementare la selezione di un oggetto
//         // DeselectAllObjects();
//         // selectedObject = gameObject;
//         // objectRenderer.material.color = selectedColor;
//     }

//     private static List<GameObject> interactingElements = new List<GameObject>();

//     public void ClearAndAddElement(GameObject callingGameObject, string prefabName, bool isSameElement = false){
//         interactingElements.Add(callingGameObject);
//         Debug.Log("Interacting elements list: " + string.Join(", ", interactingElements));

//         if(interactingElements.Count > 1 ){
            
//             bool newElementAdded = elementFilesManager.AddFoundElement(prefabName.ToLower());
//             //alreadyDone = true;
//             SpawnObject(callingGameObject.transform.position, callingGameObject.GetComponent<ARPlane>(), prefabName);

//             foreach(GameObject element in interactingElements){
//                 element.SetActive(false);
//                 Destroy(element);
//             }

//             interactingElements.Clear();
//             //callingGameObject.SetActive(false);
            

//             AudioClip clip = Resources.Load<AudioClip>("Sounds/correct");

//         if (!newElementAdded)
//         {
//             clip = Resources.Load<AudioClip>("Sounds/wrong");
//             Debug.Log("Elemento già trovato (VirtualPlaneManager): " + prefabName);
//         } else {   
//             createButtonsComponent.ResetButtons();
//             Debug.Log("ButtonLabels aggiornato con successo");
//             Debug.Log("ECCOMI FRA" + AchievementsCheck.Instance);
//             AchievementsCheck.Instance.FoundedElement(prefabName);
//             SpawnPopUp(prefabName);
//         }

//         GameObject tempAudioObject = new GameObject("TempAudioObject");
//         AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
//         audioSource.clip = clip;

//         audioSource.Play();

//         Destroy(tempAudioObject, clip.length);

//         }else{
//             //interactingElements.Add(callingGameObject);
//         }



//         // callingGameObject.SetActive(false);
//         //Destroy(callingGameObject);
        
//     }

//     public void ClearAndAddElement(ElementPair elementPair, GameObject callingGameObject, GameObject otherObject, bool isSameElement = false){
//         interactingElements.Add(callingGameObject);
//         Debug.Log("Interacting elements list: " + string.Join(", ", interactingElements));

//         if(interactingElements.Count > 1 ){

//             // ReadCSV.Instance.elementAssociations.TryGetValue(elementPair, out resultPrefabName);
//             // ReadCSV.Instance.elementAssociations.TryGetValue(new ElementPair(elementPair.Element2, elementPair.Element1), out resultPrefabName);
//             string resultPrefabName = ReadCSV.Instance.elementAssociations.GetValueOrDefault(elementPair);
//             if (resultPrefabName == null) {

//                 resultPrefabName = ReadCSV.Instance.elementAssociations.GetValueOrDefault(new ElementPair(elementPair.Element2, elementPair.Element1));

//                 ElementPair elementPairBis = new ElementPair(elementPair.Element2, elementPair.Element1);
//                 if (!ReadCSV.Instance.elementAssociations.TryGetValue(elementPairBis, out string resultPrefabNameBis))
//                     SpawnPopUpNotExits();

//             }

//             if (resultPrefabName == null) {
//                 Debug.Log("Elemento non trovato...");
//                 foreach(GameObject element in interactingElements){
//                     element.SetActive(false);
//                     Destroy(element);
//                 }

//                 interactingElements.Clear();
//                 return;
//             }

            
//             bool newElementAdded = elementFilesManager.AddFoundElement(resultPrefabName.ToLower());
//             SpawnObject(callingGameObject.transform.position, callingGameObject.GetComponent<ARPlane>(), resultPrefabName);

//             foreach(GameObject element in interactingElements){
//                 element.SetActive(false);
//                 Destroy(element);
//             }

//             interactingElements.Clear();
            

//             AudioClip clip = Resources.Load<AudioClip>("Sounds/correct");

//             if (!newElementAdded)
//             {
//                 clip = Resources.Load<AudioClip>("Sounds/wrong");
//                 SpawnPopUp(resultPrefabName, true);
//                 Debug.Log("Elemento già trovato (VirtualPlaneManager): " + resultPrefabName);
//             } else {   
//                 createButtonsComponent.ResetButtons();
//                 Debug.Log("ButtonLabels aggiornato con successo");
//                 SpawnPopUp(resultPrefabName);
//             }
//             GameObject tempAudioObject = new GameObject("TempAudioObject");
//             AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
//             audioSource.clip = clip;

//             audioSource.Play();

//             Destroy(tempAudioObject, clip.length);

//         }else{
//             //interactingElements.Add(callingGameObject);
//         }



//         // callingGameObject.SetActive(false);
//         //Destroy(callingGameObject);
        
//     }


//     void SpawnPopUp(string prefabName = "default", bool alreadyFound = false)
//     {
//         GameObject spawnedObject;
//         if (alreadyFound) {
//             spawnedObject = Instantiate(popUpElementAlreadyFound, transform.position, Quaternion.identity);
//         }
//         else
//         {
//             spawnedObject = Instantiate(popUpElementCreated, transform.position, Quaternion.identity);
//         }
//         Transform backgroundTransform = FindInChildren(spawnedObject.transform, "IconElement");
//         if (backgroundTransform != null)
//         {
//             Image backgroundImage = backgroundTransform.GetComponent<Image>();
//             if (backgroundImage != null)
//             {
//                 Sprite loadedSprite;
//                 if (_othersElements.Contains(prefabName)) {
//                     loadedSprite = Resources.Load<Sprite>("Icon/other");
//                 }
//                 else
//                 {
//                     loadedSprite = Resources.Load<Sprite>("Icon/" + prefabName);
//                 }

//                 if (loadedSprite != null)
//                 {
//                     backgroundImage.sprite = loadedSprite;
//                     // Debug.Log("Loaded sprite: " + loadedSprite.name);
//                 }
//             }
//         }
//         else
//         {
//             Debug.Log("Background not found");
//         }

//         Transform nameElementTransform = FindInChildren(spawnedObject.transform, "NameElement");
//         if (nameElementTransform != null)
//         {
//             TextMeshProUGUI nameText = nameElementTransform.GetComponent<TextMeshProUGUI>();
//             if (nameText != null)
//             {
//                 nameText.text = char.ToUpper(prefabName[0]) + prefabName[1..];
//                 // Debug.Log("Loaded name: " + nameText.text);
//             }
//         }
//         else
//         {
//             Debug.Log("NameElement not found");
//         }

//         Destroy(spawnedObject, 3f);
//     }

//     public void SpawnPopUpNotExits() {
//         GameObject spawnedObject = Instantiate(Resources.Load<GameObject>("ElementNotExist"), transform.position, Quaternion.identity);
            
//         AudioClip sound = Resources.Load<AudioClip>("Sounds/notexist");
//         GameObject audioObject = new GameObject("TempAudioObject");
//         AudioSource audioSourceTemp = audioObject.AddComponent<AudioSource>();
//         audioSourceTemp.clip = sound;

//         audioSourceTemp.Play();

//         Destroy(audioObject, sound.length);

//         Destroy(spawnedObject, 3f);
//     }

//     Transform FindInChildren(Transform parent, string name)
//     {
//         foreach (Transform child in parent)
//         {
//             if (child.name == name)
//                 return child;

//             Transform result = FindInChildren(child, name);
//             if (result != null)
//                 return result;
//         }
//         return null;
//     }


//     private class PlaneCoords
//     {
//         public Vector3 coords { get; set; }
//         public ARPlane plane { get; set; }

//         public PlaneCoords(Vector3 coords, ARPlane plane)
//         {
//             this.coords = coords;
//             this.plane = plane;
//         }

//         public override string ToString()
//         {
//             return $"Coords: {coords.ToString()}, Plane: {plane.ToString()}";
//         }
//     }

// }

using UnityEngine; 
using UnityEngine.XR.ARFoundation; 
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class VirtualPlaneManager : ARSceneManager
{ 

    new public static VirtualPlaneManager Instance;

    private List<GameObject> spawnedObjects = new List<GameObject>(); // TODO: gestire questa lista


    private VirtualPlaneManager() { }

    [SerializeField] private GameObject XROrigin; 

    private ARPlaneManager planeManager; 

    private string selectedPrefab = "water";
 

    new private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        planeManager = XROrigin.GetComponent<ARPlaneManager>(); 

        elementFilesManager = ElementFilesManager.Instance;
        createButtons = createButtonsComponent.GetComponent<CreateButtons>();
    }
    
    private void Start() 
    { 
        if (XROrigin == null) 
        { 
            Debug.LogError("XR Origin (ArPlane) is not assigned"); 
            return; 
        } 
 
    } 
 
    private void Update() 
    { 
        PlaneCoords planeTouchCoords = DetectPlaneTouch();
        if (planeTouchCoords != null)
        {
            Debug.Log("Plane Touched at " + planeTouchCoords);
            SpawnObject(planeTouchCoords.coords, planeTouchCoords.plane, selectedPrefab);
        }

    } 

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
                Debug.Log("Plane Touched at !" + touch.position);
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
        Debug.Log("Spawning a " + prefabName);
        
        GameObject newARObject;
        if (otherElements.Contains(prefabName)) {
            newARObject = Instantiate(Resources.Load<GameObject>("other"), Vector3.zero, Quaternion.Euler(0, 0, 0));
            // newARObject = Create3DText.Instance.CreateTextObject(prefabName.ToUpper());
            // ComponentAdder ca = new();
            // ca.AddComponentsToGameObject(newARObject);
            TextMeshProUGUI[] texts = newARObject.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                text.text = prefabName;
            }
        }
        else
        {
            newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
        }
        
        newARObject.name = prefabName;
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
        SetSelectedPrefab(prefabName.ToLower());

        return false;
    }

    public void SelectGameObject(GameObject gameObject)
    {
        // TODO: implementare la selezione di un oggetto
        // DeselectAllObjects();
        // selectedObject = gameObject;
        // objectRenderer.material.color = selectedColor;
    }

    private static List<GameObject> interactingElements = new List<GameObject>();

    public void ClearAndAddElement(GameObject callingGameObject, string prefabName, bool isSameElement = false){
        interactingElements.Add(callingGameObject);
        Debug.Log("Interacting elements list: " + string.Join(", ", interactingElements));

        if(interactingElements.Count > 1 ){
            
            bool newElementAdded = elementFilesManager.AddFoundElement(prefabName.ToLower());
            //alreadyDone = true;
            SpawnObject(callingGameObject.transform.position, callingGameObject.GetComponent<ARPlane>(), prefabName);

            foreach(GameObject element in interactingElements){
                element.SetActive(false);
                Destroy(element);
            }

            interactingElements.Clear();
            //callingGameObject.SetActive(false);
            

            AudioClip clip = Resources.Load<AudioClip>("Sounds/correct");

        if (!newElementAdded)
        {
            clip = Resources.Load<AudioClip>("Sounds/wrong");
            Debug.Log("Elemento già trovato (VirtualPlaneManager): " + prefabName);
        } else {   
            createButtons.ResetButtons();
            Debug.Log("ButtonLabels aggiornato con successo");
            Debug.Log("ECCOMI FRA" + AchievementsCheck.Instance);
            AchievementsCheck.Instance.FoundedElement(prefabName);
            SpawnPopUp(prefabName);
        }

        GameObject tempAudioObject = new GameObject("TempAudioObject");
        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;

        audioSource.Play();

        Destroy(tempAudioObject, clip.length);

        }else{
            //interactingElements.Add(callingGameObject);
        }



        // callingGameObject.SetActive(false);
        //Destroy(callingGameObject);
        
    }

    public void ClearAndAddElement(ElementPair elementPair, GameObject callingGameObject, GameObject otherObject, bool isSameElement = false){
        interactingElements.Add(callingGameObject);
        Debug.Log("Interacting elements list: " + string.Join(", ", interactingElements));

        if(interactingElements.Count > 1 ){

            // ReadCSV.Instance.elementAssociations.TryGetValue(elementPair, out resultPrefabName);
            // ReadCSV.Instance.elementAssociations.TryGetValue(new ElementPair(elementPair.Element2, elementPair.Element1), out resultPrefabName);
            string resultPrefabName = ReadCSV.Instance.elementAssociations.GetValueOrDefault(elementPair);
            if (resultPrefabName == null) {

                resultPrefabName = ReadCSV.Instance.elementAssociations.GetValueOrDefault(new ElementPair(elementPair.Element2, elementPair.Element1));

                ElementPair elementPairBis = new ElementPair(elementPair.Element2, elementPair.Element1);
                if (!ReadCSV.Instance.elementAssociations.TryGetValue(elementPairBis, out string resultPrefabNameBis))
                    SpawnPopUpNotExits();

            }

            if (resultPrefabName == null) {
                Debug.Log("Elemento non trovato...");
                foreach(GameObject element in interactingElements){
                    element.SetActive(false);
                    Destroy(element);
                }

                interactingElements.Clear();
                return;
            }

            
            bool newElementAdded = elementFilesManager.AddFoundElement(resultPrefabName.ToLower());
            SpawnObject(callingGameObject.transform.position, callingGameObject.GetComponent<ARPlane>(), resultPrefabName);

            foreach(GameObject element in interactingElements){
                element.SetActive(false);
                Destroy(element);
            }

            interactingElements.Clear();
            

            AudioClip clip = Resources.Load<AudioClip>("Sounds/correct");

            if (!newElementAdded)
            {
                clip = Resources.Load<AudioClip>("Sounds/wrong");
                SpawnPopUp(resultPrefabName, true);
                Debug.Log("Elemento già trovato (VirtualPlaneManager): " + resultPrefabName);
            } else {   
                createButtons.ResetButtons();
                Debug.Log("ButtonLabels aggiornato con successo");
                SpawnPopUp(resultPrefabName);
            }
            GameObject tempAudioObject = new GameObject("TempAudioObject");
            AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
            audioSource.clip = clip;

            audioSource.Play();

            Destroy(tempAudioObject, clip.length);

        }else{
            //interactingElements.Add(callingGameObject);
        }



        // callingGameObject.SetActive(false);
        //Destroy(callingGameObject);
        
    }

    void SpawnPopUp(string prefabName = "default", bool alreadyFound = false)
    {
        GameObject spawnedObject;
        if (alreadyFound) {
            spawnedObject = Instantiate(popUpElementAlreadyFound, transform.position, Quaternion.identity);
        }
        else
        {
            spawnedObject = Instantiate(popUpElementCreated, transform.position, Quaternion.identity);
        }
        Transform backgroundTransform = FindInChildren(spawnedObject.transform, "IconElement");
        if (backgroundTransform != null)
        {
            Image backgroundImage = backgroundTransform.GetComponent<Image>();
            if (backgroundImage != null)
            {
                Sprite loadedSprite;
                if (otherElements.Contains(prefabName)) {
                    loadedSprite = Resources.Load<Sprite>("Icon/other");
                }
                else
                {
                    loadedSprite = Resources.Load<Sprite>("Icon/" + prefabName);
                }

                if (loadedSprite != null)
                {
                    backgroundImage.sprite = loadedSprite;
                    // Debug.Log("Loaded sprite: " + loadedSprite.name);
                }
            }
        }
        else
        {
            Debug.Log("Background not found");
        }

        Transform nameElementTransform = FindInChildren(spawnedObject.transform, "NameElement");
        if (nameElementTransform != null)
        {
            TextMeshProUGUI nameText = nameElementTransform.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = char.ToUpper(prefabName[0]) + prefabName[1..];
                // Debug.Log("Loaded name: " + nameText.text);
            }
        }
        else
        {
            Debug.Log("NameElement not found");
        }

        Destroy(spawnedObject, 3f);
    }

    public void SpawnPopUpNotExits() {
        GameObject spawnedObject = Instantiate(Resources.Load<GameObject>("ElementNotExist"), transform.position, Quaternion.identity);
            
        AudioClip sound = Resources.Load<AudioClip>("Sounds/notexist");
        GameObject audioObject = new GameObject("TempAudioObject");
        AudioSource audioSourceTemp = audioObject.AddComponent<AudioSource>();
        audioSourceTemp.clip = sound;

        audioSourceTemp.Play();

        Destroy(audioObject, sound.length);

        Destroy(spawnedObject, 3f);
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


