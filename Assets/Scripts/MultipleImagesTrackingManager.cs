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
    private GameObject[] prefabsToSpawn;
    private ARTrackedImageManager _arTrackedImageManager;

    private Dictionary<ARTrackedImage, GameObject> _imageObjectMap; // Marker name -> GameObject
    //private Dictionary<string, GameObject> _arObjects;

    public GameObject defaultObject;

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
            //_arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            _imageObjectMap[trackedImage].SetActive(false);
            return;
        }

        // Show, hide or position the game object based on the tracking image
    
        // _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(true);
        // _arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        _imageObjectMap[trackedImage].gameObject.SetActive(true);
        _imageObjectMap[trackedImage].transform.position = trackedImage.transform.position;
    }

    public void OnPrefabSelected(string prefabName){
        //TODO: remove
    }
}