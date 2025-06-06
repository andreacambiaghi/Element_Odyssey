using UnityEngine; 
using UnityEngine.XR.ARFoundation; 
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class VirtualPlaneManager : MonoBehaviour 
{ 

    public static VirtualPlaneManager Instance;

    private ElementFilesManager elementFilesManager;
    
    private CreateButtons createButtonsComponent;

    private List<string> _othersElements;

    [SerializeField] private GameObject slider;
    
    [SerializeField] private SliderMenuAnim menu;

    [SerializeField] private GameObject createButton;

    [SerializeField] private GameObject popUpElementCreated;
    [SerializeField] private GameObject popUpElementAlreadyFound;
    [SerializeField] private GameObject elementSelected;
    [SerializeField] private GameObject effectUnlocked; // effect unlocked animation between two elements


    private VirtualPlaneManager() { }

    [SerializeField] private GameObject XROrigin; 

    private ARPlaneManager planeManager; 

    private GameObject selectedObject;          // the object selected by touch interactions in the arplane

    private string selectedPrefab;    // the prefab selected from the menu

    private void Start() 
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

        _othersElements = ElementFilesManager.Instance.GetOthersElements();
        Debug.LogWarning("Others elements: " + _othersElements.Count);
        if (XROrigin == null) 
        { 
            Debug.LogError("XR Origin is not assigned"); 
            return; 
        } 
 
        planeManager = XROrigin.GetComponent<ARPlaneManager>(); 
        elementFilesManager = ElementFilesManager.Instance;
        createButtonsComponent = createButton.GetComponent<CreateButtons>();
    } 
 
    private void Update() 
    { 

        PlaneCoords planeTouchCoords = DetectPlaneTouch();
        if (planeTouchCoords != null)
        {
            Debug.Log("Plane Touched at " + planeTouchCoords);

            if(selectedObject != null){
                moveSelectedObject(planeTouchCoords.coords);
                return;
            }else{
                if(isDeletionInProgress){
                    isDeletionInProgress = false;
                    return;
                } 
                SpawnObject(planeTouchCoords.coords, planeTouchCoords.plane, selectedPrefab);
                return;
            }  
           
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
                    }
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
        if(prefabName == null) return;

        Debug.Log("Spawning a " + prefabName);
        
        GameObject newARObject;
        if (_othersElements.Contains(prefabName)) {
            newARObject = Instantiate(Resources.Load<GameObject>("other"), Vector3.zero, Quaternion.Euler(0, 0, 0));
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

        // AchievementsCheck.Instance.FoundedElement(prefabName);

    }

    private Color GetInvertedColor(GameObject gameObject){
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Texture2D texture = renderer.material.mainTexture as Texture2D;
        Color averageColor = Color.black;

        if (texture != null)
        {
            Color[] pixels = texture.GetPixels();
            foreach (Color pixel in pixels)
            {
                averageColor += pixel;
            }
            averageColor /= pixels.Length;
        }
        else
        {
            averageColor = renderer.material.color;
        }

        Color invertedColor = new Color(1 - averageColor.r, 1 - averageColor.g, 1 - averageColor.b);

        return invertedColor;
    }

    public void SetSelectedPrefab(string prefabName)
    {
        Debug.Log("Selected prefab: " + prefabName);
        selectedPrefab = prefabName;
    }

    public bool OnPrefabSelected(string prefabName){
        elementSelected.GetComponent<TextMeshProUGUI>().text = "You have selected " + prefabName;    
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


    bool isDeletionInProgress = false;

    public void SelectGameObject(GameObject gameObject)
    {

        if(gameObject == null) return;

        if(selectedObject == gameObject){
            isDeletionInProgress = true;

            DeselectSelectedGameObject();
            DeleteGameObject(gameObject);
            
            return;
        } 

        Debug.Log("SelectGameObject: " + gameObject.name);
        if(selectedObject != null){
            DeselectSelectedGameObject();
            return;
        }

        Debug.Log("SelectGameObject: " + gameObject.name);

        selectedObject = gameObject;

        Outline outline = selectedObject.AddComponent<Outline>();
        if(outline != null){
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = GetInvertedColor(selectedObject);
            outline.OutlineWidth = 10f;
        }
    }

    private static List<GameObject> interactingElements = new List<GameObject>();

    public void ClearAndAddElement(ElementPair elementPair, GameObject callingGameObject, GameObject otherObject, bool isSameElement = false){
        interactingElements.Add(callingGameObject);
        Debug.Log("Interacting elements list: " + string.Join(", ", interactingElements));

        if(interactingElements.Count > 1 ){
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
            GameObject effect = Instantiate(effectUnlocked, callingGameObject.transform.position, Quaternion.identity);

            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                float totalDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constantMax;
                Destroy(effect, totalDuration);
            }
            
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
                createButtonsComponent.ResetButtons();
                Debug.Log("ButtonLabels aggiornato con successo");
                SpawnPopUp(resultPrefabName);
                ElementFilesManager.Instance.SetBalance(ElementFilesManager.Instance.GetBalance() + 1);

                try{
                    AchievementsCheck.Instance.FoundedElement(resultPrefabName);
                } catch (System.Exception e){
                    Debug.LogError("Error in AchievementsCheck: " + e.Message);
                }
                
            }
            GameObject tempAudioObject = new GameObject("TempAudioObject");
            AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = Resources.Load<AudioMixer>("Mixer").FindMatchingGroups("Master")[0];

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
                if (_othersElements.Contains(prefabName)) {
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
        audioSourceTemp.outputAudioMixerGroup = Resources.Load<AudioMixer>("Mixer").FindMatchingGroups("Master")[0];

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

    void moveSelectedObject(Vector3 position){
        if(selectedObject != null){
            selectedObject.transform.position = position;
            DeselectSelectedGameObject();
        }
    }

    public void DeleteGameObject(GameObject gameObject)
    {
        if(gameObject == null) return;
        Debug.Log("DeleteGameObject: " + gameObject.name);
        gameObject.SetActive(false);
        Destroy(gameObject);

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


