using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagesTrackingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;
    private ARTrackedImageManager _arTrackedImageManager;
    private Dictionary<string, GameObject> _arObjects;

    // Get the reference to the ARTrackedImageManager

    private void Awake()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        _arObjects = new Dictionary<string, GameObject>();

    }

    private void Start()
    {
        // Listen the event when the tracked images are changed
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

        // Spawn the game objects for the existing tracked images
        foreach (GameObject prefab in prefabsToSpawn)
        {
            GameObject newARObject = Instantiate(prefab, Vector3.zero, Quaternion.Euler(-90, 0, 0));
            newARObject.name = prefab.name;
            newARObject.gameObject.SetActive(false);
            _arObjects.Add(newARObject.name, newARObject);
        }
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
            UpdatedTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdatedTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
        }
    }

    private void UpdatedTrackedImage(ARTrackedImage trackedImage)
    {
        // Check tracking statsu of the tracked image
        if (trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            return;
        }

        // Show, hide or position the game object based on the tracking image
        if (prefabsToSpawn != null)
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(true);
            _arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        }
    }
}
