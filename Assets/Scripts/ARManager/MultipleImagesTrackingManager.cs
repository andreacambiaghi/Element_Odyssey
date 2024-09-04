using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.IO;
using System;

public class MultipleImagesTrackingManager : MonoBehaviour
{
    public static MultipleImagesTrackingManager Instance;
    
    //private GameObject[] prefabsToSpawn;
    private ARTrackedImageManager _arTrackedImageManager;

    private Dictionary<ARTrackedImage, GameObject> _imageObjectMap; // Marker name -> GameObject
    //private Dictionary<string, GameObject> _arObjects;

    public GameObject defaultObject;

    private ARTrackedImage SelectedImageObject;

    private CreateButtons _createButtonsComponent;

    [SerializeField] private GameObject slider;
    [SerializeField] private SliderMenuAnim menu;

    [SerializeField] private GameObject createButton;

    // Get the reference to the ARTrackedImageManager

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
        //_arObjects = new Dictionary<string, GameObject>();

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
        }

        GameObject newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
        newARObject.name = prefabName;
        newARObject.gameObject.SetActive(false);
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
        }

        
        GameObject newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
        newARObject.name = prefabName;
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

    public void ClearAndAddElement(string prefabName){

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
            if(first) {
                targetImage = entry.Key;
                first = false;
            }
            imagesToReset.Add(entry.Key);
            
        }

        foreach(ARTrackedImage image in imagesToReset){
            AssociateGameObjectToMarker(image, defaultObject.name);
        }

        //Debug.Log("---3");

        if(targetImage != null){
            AssociateGameObjectToMarker(targetImage, prefabName);
        }
        //Debug.Log("---4");

        AudioClip clip = Resources.Load<AudioClip>("Sounds/correct");

        string buttonLabel = char.ToUpper(prefabName[0]) + prefabName.Substring(1);
        
        if (_createButtonsComponent.buttonLabels.Contains(buttonLabel)){
            clip = Resources.Load<AudioClip>("Sounds/wrong");
        }

        else {   
            _createButtonsComponent.CreateButton(buttonLabel);
            _createButtonsComponent.buttonLabels.Add(buttonLabel);
            Debug.Log("ButtonLabels aggiornato con successo");

            //File.AppendAllText(Path.Combine(Application.dataPath, "Resources", "Founds.txt"), buttonLabel + Environment.NewLine);
        }

        GameObject tempAudioObject = new GameObject("TempAudioObject");
        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(tempAudioObject, clip.length);
    
    }
}