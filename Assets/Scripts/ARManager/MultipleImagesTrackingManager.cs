using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.IO;
using System;
using TMPro;

public class MultipleImagesTrackingManager : MonoBehaviour
{
    public static MultipleImagesTrackingManager Instance;

    private ElementFilesManager _elementFilesManager;
    
    //private GameObject[] prefabsToSpawn;
    private ARTrackedImageManager _arTrackedImageManager;

    private Dictionary<ARTrackedImage, GameObject> _imageObjectMap; // Marker name -> GameObject
    //private Dictionary<string, GameObject> _arObjects;

    public GameObject defaultObject;

    private ARTrackedImage SelectedImageObject;

    private CreateButtons _createButtonsComponent;

    private List<string> _othersElements;

    [SerializeField] private GameObject slider;
    [SerializeField] private SliderMenuAnim menu;

    [SerializeField] private GameObject createButton;

    [SerializeField] private GameObject popUpElementCreated;
    [SerializeField] private GameObject popUpElementAlreadyFound;

    // Get the reference to the ARTrackedImageManager

    private bool _isSelecting = false;

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

        
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        _imageObjectMap = new Dictionary<ARTrackedImage, GameObject>();
        _createButtonsComponent = createButton.GetComponent<CreateButtons>();
        _elementFilesManager = ElementFilesManager.Instance;
        //_arObjects = new Dictionary<string, GameObject>();

        _othersElements = ElementFilesManager.Instance.GetOthersElements();
        Debug.LogWarning("Others elements: " + _othersElements.Count);

    }

    private void Start()
    {
        // Listen the event when the tracked images are changed
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

        // Spawn the default game object
        // GameObject newARObject = Instantiate(defaultObject, Vector3.zero, Quaternion.Euler(-90, 0, 0));
        // newARObject.name = prefab.name;
        // newARObject.gameObject.SetActive(false);
        // _arObjects.Add(newARObject.name, newARObject);
    }


    private void OnDestroy()
    {
        // Remove the listener when the script is destroyed
        _arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Identify the changes in the tracked images
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            // Spawn the prefab when the tracked image is found
            GameObject newARObject = Instantiate(defaultObject, Vector3.zero, Quaternion.Euler(-90, 0, 0));
            newARObject.name = defaultObject.name;
            newARObject.gameObject.SetActive(false);

            _imageObjectMap.Add(trackedImage, newARObject);
            Debug.Log("Added: " + trackedImage.referenceImage.name + " -> " + newARObject.name);

            UpdatedTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            // Update the prefab when the tracked image is updated
            UpdatedTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            // Remove the prefab when the tracked image is removed
            //_arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            _imageObjectMap[trackedImage].SetActive(false);
        }
    }

    private void AssociateGameObjectToMarker(ARTrackedImage trackedImage, string prefabName)   
    {
        if (_imageObjectMap.ContainsKey(trackedImage) && _imageObjectMap[trackedImage] != null)
        {
            _imageObjectMap[trackedImage].gameObject.SetActive(false);
            DeselectSelectedGameObject();
            _isSelecting = false;
        }

        GameObject newARObject;
        if (_othersElements.Contains(prefabName)) {
            newARObject = Instantiate(Resources.Load<GameObject>("other"), Vector3.zero, Quaternion.Euler(0, 0, 0));
            TextMeshProUGUI text = newARObject.GetComponentInChildren<TextMeshProUGUI>();
            text.text = prefabName;
        }
        else
        {
            newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
            newARObject.name = prefabName;
        }
        
        newARObject.SetActive(false);
        _imageObjectMap[trackedImage] = newARObject;
        UpdatedTrackedImage(trackedImage);

        if (slider != null)
        {
            if (menu.GetState())
            {
                Button button = slider.GetComponent<Button>();
                button.onClick.Invoke();
            }
        }

    }

    private void UpdatedTrackedImage(ARTrackedImage trackedImage)
    {
        // Check tracking status of the tracked image
        if (trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
        {
            _imageObjectMap[trackedImage].SetActive(false);
            return;
        }

        // Show, hide or position the game object based on the tracking image
    
        _imageObjectMap[trackedImage].gameObject.SetActive(true);
        _imageObjectMap[trackedImage].transform.position = trackedImage.transform.position;
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

    public void SelectGameObject(GameObject selectedObject)
    {

        // Se l'oggetto è già selezionato, deselezionalo e rimuovi il bordo
        if (_isSelecting && _imageObjectMap.ContainsValue(selectedObject) && SelectedImageObject != null && _imageObjectMap[SelectedImageObject] == selectedObject)
        {
            DeselectSelectedGameObject();
            _isSelecting = false;
            return;
        }
        
        //get the ar tracked image associated to this object
        foreach (KeyValuePair<ARTrackedImage, GameObject> entry in _imageObjectMap)
        {
            if(entry.Value == null || entry.Key == null)
            {
                continue;
            }
            Debug.Log("Entry: " + entry.Key.referenceImage.name + " -> " + entry.Value.name);
            if (entry.Value == selectedObject)
            {
                if(SelectedImageObject != null && SelectedImageObject != entry.Key) DeselectSelectedGameObject();

                SelectedImageObject = entry.Key;
                Debug.Log("Selected: " + SelectedImageObject.referenceImage.name);

                var outline = selectedObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                Color originalColor = selectedObject.GetComponent<Renderer>().material.color;
                outline.OutlineColor = GetInvertedColor(selectedObject);
                outline.OutlineWidth = 10f;

                _isSelecting = true;

                break;
            }
        }
    }

    public void DeselectSelectedGameObject()
    {
        if (SelectedImageObject == null || !_imageObjectMap.ContainsKey(SelectedImageObject)) return;

        Outline outline = _imageObjectMap[SelectedImageObject].GetComponent<Outline>();

        if (outline != null)
        {
            Destroy(outline);
        }

        Color color = _imageObjectMap[SelectedImageObject].GetComponent<Renderer>().material.color;
   
        SelectedImageObject = null;
    }

    public bool SetPrefabOnSelected(string prefabName)
    {
        Debug.Log("Loading -> " + prefabName);  
        if (SelectedImageObject == null) return false; 
        GameObject oldGO = _imageObjectMap[SelectedImageObject];
        ARTrackedImage aRTrackedImage = SelectedImageObject;

        if (_imageObjectMap[aRTrackedImage] != null ) {
            DeselectSelectedGameObject();
            _imageObjectMap[aRTrackedImage].gameObject.SetActive(false);
            _isSelecting = false;
        }

        
        GameObject newARObject;
        if (_othersElements.Contains(prefabName)) {
            newARObject = Instantiate(Resources.Load<GameObject>("other"), Vector3.zero, Quaternion.Euler(0, 0, 0));
            TextMeshProUGUI text = newARObject.GetComponentInChildren<TextMeshProUGUI>();
            text.text = prefabName;
        }
        else
        {
            newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
            newARObject.name = prefabName;
        }

        newARObject.gameObject.SetActive(false);
        _imageObjectMap[aRTrackedImage] = newARObject;
        UpdatedTrackedImage(aRTrackedImage);

        if (slider != null)
        {
            if (menu.GetState())
            {
                Button button = slider.GetComponent<Button>();
                button.onClick.Invoke();
            }
        }

        return true;
    }

    public bool OnPrefabSelected(string prefabName){
        if(SelectedImageObject == null) return false; //aggiungere errore nella gui che indica che è necessario fare una selezione
        return SetPrefabOnSelected(prefabName);
    }


    bool soundOn = true;

    bool sameElementSoundCount = false;

    public void ClearAndAddElement(string prefabName, bool isSameElement = false){    


        soundOn = (!sameElementSoundCount && isSameElement) || !isSameElement;

        sameElementSoundCount = !sameElementSoundCount && isSameElement;

        // if(isSameElement) sameElementSoundCount++;
        // else sameElementSoundCount = 0;

        bool elementAlreadyAdded = _elementFilesManager.AddFoundElement(prefabName.ToLower());

        DeselectSelectedGameObject();
        
        ARTrackedImage targetImage = null;
        bool first = true;
        List<ARTrackedImage> imagesToReset = new List<ARTrackedImage>();

        foreach (KeyValuePair<ARTrackedImage, GameObject> entry in _imageObjectMap)
        {
            if(entry.Value == null || entry.Key == null)
            {
                continue;
            }
            if (first) {
                targetImage = entry.Key;
                first = false;
            }
            imagesToReset.Add(entry.Key);
            
        }

        foreach(ARTrackedImage image in imagesToReset){
            AssociateGameObjectToMarker(image, defaultObject.name);
        }

        if(targetImage != null){
            AssociateGameObjectToMarker(targetImage, prefabName);
        }

        bool finded = false;
        AudioClip clip = Resources.Load<AudioClip>("Sounds/correct");

        //string buttonLabel = char.ToUpper(prefabName[0]) + prefabName.Substring(1);
        string buttonLabel = prefabName;
        // if (_createButtonsComponent.buttonLabels.Contains(buttonLabel)){
        //     clip = Resources.Load<AudioClip>("Sounds/wrong");
        // }
        // if (_elementFilesManager.GetFoundElements().Contains(prefabName.ToLower()))
        if (!elementAlreadyAdded)
        {
            clip = Resources.Load<AudioClip>("Sounds/wrong");
            Debug.Log("Elemento già trovato (MITM): " + prefabName);
            finded = true;
        } else {   
            // _createButtonsComponent.CreateButton(buttonLabel);
            // _createButtonsComponent.buttonLabels.Add(buttonLabel);

            _createButtonsComponent.ResetButtons();
            //_createButtonsComponent.ClearButtons();
            Debug.Log("ButtonLabels aggiornato con successo");
            finded = false;
            AchievementsCheck.Instance.FoundedElement(prefabName);
            Debug.Log("SONO QUIII :)" + prefabName);
        }

        GameObject tempAudioObject = new GameObject("TempAudioObject");
        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;

        if (soundOn && SoundManager.Instance.IsSoundOn()) {
            audioSource.Play();
            SpawnPopUp(prefabName, finded);
        }

        Destroy(tempAudioObject, clip.length);
    }

    void SpawnPopUp(string prefabName = "default", bool alreadyFound = false)
    {
        GameObject spawnedObject = null;
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



}