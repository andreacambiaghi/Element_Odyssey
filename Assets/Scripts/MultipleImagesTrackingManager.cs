using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagesTrackingManager : MonoBehaviour
{   
    public static MultipleImagesTrackingManager Instance;
    private GameObject[] _prefabsToSpawn;
    private ARTrackedImageManager _arTrackedImageManager;
    private Dictionary<string, GameObject> _arObjects;
    private Dictionary<string, GameObject> _activePrefabs;
    private HashSet<string> _processedMarkers; // HashSet to track processed markers

    // UI Elements
    public GameObject menuPanel; // Reference to the menu panel GameObject

    private ARTrackedImage currentTrackedImage;

    // Initialize ARTrackedImageManager and AR objects
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
        _arObjects = new Dictionary<string, GameObject>();
        _processedMarkers = new HashSet<string>(); // Initialize the HashSet
        _activePrefabs = new Dictionary<string, GameObject>(); // Dictionary to keep track of active prefabs
        _prefabsToSpawn = Resources.LoadAll<GameObject>("Prefab");
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Hide the menu at the start
        menuPanel.SetActive(false);

        // Subscribe to the trackedImagesChanged event
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

        // Instantiate and set up AR objects
        foreach (GameObject prefab in _prefabsToSpawn)
        {
            GameObject newARObject = Instantiate(prefab, Vector3.zero, Quaternion.Euler(-90, 0, 0));
            newARObject.name = prefab.name;
            newARObject.gameObject.SetActive(false);
            _arObjects.Add(newARObject.name, newARObject);
            Debug.Log("Added " + newARObject.name + " to the dictionary");
        }
    }

    // Unsubscribe from the trackedImagesChanged event
    private void OnDestroy()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Handle changes in tracked images
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            ShowMenu(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                ShowMenu(trackedImage);
                UpdatePrefabPosition(trackedImage);
            }
            else
            {
                HidePrefab(trackedImage);
            }
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            HidePrefab(trackedImage);
        }
    }

    // Show the menu when an image is tracked, if it hasn't been processed
    private void ShowMenu(ARTrackedImage trackedImage)
    {
        // Check if this marker has already been processed
        if (_processedMarkers.Contains(trackedImage.referenceImage.name))
        {
            return; // Do not show the menu if the marker has already been processed
        }

        currentTrackedImage = trackedImage;
        menuPanel.SetActive(true);
    }

    // Hide the menu
    private void HideMenu()
    {
        Debug.Log("Hiding menu");
        menuPanel.SetActive(false);
    }

    // Handle prefab selection from the menu
    public void OnPrefabSelected(string prefabName)
{
    Debug.Log("Prefab selected: " + prefabName);
    Debug.Log(_arObjects);
    if (currentTrackedImage != null && _arObjects.ContainsKey(prefabName))
    {
        Debug.Log("Prefab selected: " + prefabName);
        var selectedPrefab = _arObjects[prefabName];
        selectedPrefab.transform.position = currentTrackedImage.transform.position;
        selectedPrefab.transform.rotation = currentTrackedImage.transform.rotation;
        selectedPrefab.gameObject.SetActive(true);

        // Update the prefab position immediately after activation
        UpdatePrefabPosition(currentTrackedImage);

        // Mark this marker as processed
        _processedMarkers.Add(currentTrackedImage.referenceImage.name);
        _activePrefabs[currentTrackedImage.referenceImage.name] = selectedPrefab; // Keep track of the active prefab
    }

    HideMenu(); // Hide the menu after a prefab is selected
}

// Update the position of the prefab to follow the tracked image
private void UpdatePrefabPosition(ARTrackedImage trackedImage)
{
    if (_activePrefabs.ContainsKey(trackedImage.referenceImage.name))
    {
        var prefab = _activePrefabs[trackedImage.referenceImage.name];
        prefab.transform.position = trackedImage.transform.position;
        prefab.transform.rotation = trackedImage.transform.rotation;
        prefab.gameObject.SetActive(true);
    }
}


    // Hide the prefab associated with the tracked image
    private void HidePrefab(ARTrackedImage trackedImage)
    {
        if (_activePrefabs.ContainsKey(trackedImage.referenceImage.name))
        {
            var prefab = _activePrefabs[trackedImage.referenceImage.name];
            prefab.gameObject.SetActive(false);
        }
    }
}
