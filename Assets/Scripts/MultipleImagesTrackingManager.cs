using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagesTrackingManager : MonoBehaviour
{
    public static MultipleImagesTrackingManager Instance;
    
    //private GameObject[] prefabsToSpawn;
    private ARTrackedImageManager _arTrackedImageManager;

    private Dictionary<ARTrackedImage, GameObject> _imageObjectMap; // Marker name -> GameObject
    //private Dictionary<string, GameObject> _arObjects;

    public GameObject defaultObject;

    private ARTrackedImage SelectedImageObject;

//hello


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
            //_arObjects.Add(newARObject.name, newARObject);

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
                if(SelectedImageObject != null && SelectedImageObject != entry.Key) deselectSelectedGameObject();

                SelectedImageObject = entry.Key;
                Debug.Log("Selected: " + SelectedImageObject.referenceImage.name);
                selectedObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            }
        }
        //Debug.Log("Selected image object: " + SelectedImageObject);
    }

    public void deselectSelectedGameObject()
    {
        if (SelectedImageObject == null || !_imageObjectMap.ContainsKey(SelectedImageObject)) return;
        _imageObjectMap[SelectedImageObject].GetComponent<ArTouchManager>().Deselect();
        SelectedImageObject = null;
    }


    public bool SetPrefabOnSelected(string prefabName)
    {
        Debug.Log("Loading -> " + prefabName);  
        if (SelectedImageObject == null) return false; 
        GameObject oldGO = _imageObjectMap[SelectedImageObject];
        ARTrackedImage aRTrackedImage = SelectedImageObject;

        if (_imageObjectMap[aRTrackedImage] != null ) {
            deselectSelectedGameObject();
            _imageObjectMap[aRTrackedImage].gameObject.SetActive(false);
        }

        GameObject newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
        newARObject.name = prefabName;
        newARObject.gameObject.SetActive(false);
        _imageObjectMap[aRTrackedImage] = newARObject;
        UpdatedTrackedImage(aRTrackedImage);

        return true;
    }

    public bool OnPrefabSelected(string prefabName){
        if(SelectedImageObject == null) return false; //aggiungere errore nella gui che indica che è necessario fare una selezione
        return SetPrefabOnSelected(prefabName);
    }

    public void clearAndAddElement(string prefabName){
        deselectSelectedGameObject();
        ARTrackedImage targetImage = null;
        bool first = true;
        List<ARTrackedImage> imagesToReset = new List<ARTrackedImage>();

        Debug.Log("---1");
        foreach (KeyValuePair<ARTrackedImage, GameObject> entry in _imageObjectMap)
        {
            Debug.Log("---2");
            if(entry.Value == null || entry.Key == null)
            {
                Debug.Log("---21");
                continue;
            }
            if(first) {
                Debug.Log("---22");
                targetImage = entry.Key;
                first = false;
            }
            Debug.Log("---23");
            imagesToReset.Add(entry.Key);
            
        }

        foreach(ARTrackedImage image in imagesToReset){
            _imageObjectMap[image].gameObject.SetActive(false);
            //_imageObjectMap.Remove(image);
        }
        Debug.Log("---3");

        if(targetImage != null){
            GameObject newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
            newARObject.name = prefabName;
            newARObject.gameObject.SetActive(false);
            _imageObjectMap[targetImage] = newARObject;
            UpdatedTrackedImage(targetImage);
        }
        Debug.Log("---4");

    }
}