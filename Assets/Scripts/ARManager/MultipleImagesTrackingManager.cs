using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.IO;
using System;
using TMPro;
using UnityEngine.Audio;

public class MultipleImagesTrackingManager : MonoBehaviour
{

    private ARDataManager _aRDataManager;
    private ARTrackedImageManager _arTrackedImageManager;

    private ElementFilesManager _elementFilesManager;

    public GameObject defaultObject;

    private ARTrackedImage SelectedImageObject;

    private CreateButtons _createButtonsComponent;

    [SerializeField] private GameObject slider;

    [SerializeField] private SliderMenuAnim menu;

    [SerializeField] private GameObject createButton;

    [SerializeField] private GameObject popUpElementCreated;
    [SerializeField] private GameObject popUpElementAlreadyFound;

    [SerializeField] private GameObject xrOrigin;

    [SerializeField] private GameObject elementFileManagerPrefab;

    private bool _isSelecting = false;

    private void Awake()
    {
        if (xrOrigin == null)
        {
            Debug.LogError("XR Origin not assigned in the inspector.");
            return;
        }
        else
        {
            Debug.Log($"xrOrigin.name = {xrOrigin.name}, {xrOrigin.ToString()}");
        }

        _elementFilesManager = ElementFilesManager.Instance;
        // _elementFilesManager = elementFileManagerPrefab.GetComponent<ElementFilesManager>();
        // _arTrackedImageManager = xrOrigin.GetComponent<ARTrackedImageManager>();
        _arTrackedImageManager = xrOrigin.GetComponent<ARTrackedImageManager>();
        _createButtonsComponent = createButton.GetComponent<CreateButtons>();
        // _aRDataManager = GameObject.Find("ARDataManager").GetComponent<ARDataManager>();
        _aRDataManager = ARDataManager.Instance;


        if (_aRDataManager == null)
        {
            Debug.LogError("ARDataManager not found in the scene.");
        }
        if (_elementFilesManager == null)
        {
            Debug.LogError("ElementFilesManager not found in the scene.");
        }
        if (_createButtonsComponent == null)
        {
            Debug.LogError("CreateButtons component not found in the scene.");
        }
        if (_arTrackedImageManager == null)
        {
            Debug.LogError("ARTrackedImageManager not found in the scene.");
        }

        _aRDataManager.ReloadMarkerImageAssociations();
    }

    private void Start()
    {
        // Listen the event when the tracked images are changed
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDestroy()
    {
        // Remove the listener when the script is destroyed
        _arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        Debug.Log("MITM destroyed, removing listener.");
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (_aRDataManager == null)
        {
            Debug.LogError("ARDataManager is null in OnTrackedImagesChanged.");
            return;
        }
        if (_aRDataManager.imageObjectMap == null)
        {
            Debug.LogError("imageObjectMap is null in OnTrackedImagesChanged.");
            return;
        }
        if (_aRDataManager.arMarkerAssociations == null || _aRDataManager.arMarkerAssociations.associations == null)
        {
            Debug.LogError("ARDataManager associations not loaded in OnTrackedImagesChanged. Reloading...");
            _aRDataManager.ReloadMarkerImageAssociations(); // Attempt to reload
            if (_aRDataManager.arMarkerAssociations == null || _aRDataManager.arMarkerAssociations.associations == null)
            {
                Debug.LogError("Failed to load ARDataManager associations. Cannot process tracked images.");
                return; // Exit if still null
            }
        }


        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            if (_aRDataManager.arMarkerAssociations.associations.TryGetValue(trackedImage.referenceImage.name, out string prefabName))
            {
                AssociateGameObjectToMarker(trackedImage, prefabName);
            }
            else
            {
                Debug.LogWarning($"Detected image '{trackedImage.referenceImage.name}' not found in associations. Skipping association.");
                AssociateGameObjectToMarker(trackedImage, defaultObject.name);
            }
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (!_aRDataManager.imageObjectMap.ContainsKey(trackedImage))
            {
                continue;
            }

            UpdatedTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            if (_aRDataManager.imageObjectMap.TryGetValue(trackedImage, out GameObject objToDisable))
            {
                if (objToDisable != null)
                {
                    objToDisable.SetActive(false);
                }

                // _aRDataManager.imageObjectMap.Remove(trackedImage); 
            }
            else
            {
                Debug.LogWarning($"Tried to remove trackedImage '{trackedImage.referenceImage.name}' but it wasn't found in imageObjectMap.");
            }
        }
    }

    private void AssociateGameObjectToMarker(ARTrackedImage trackedImage, string prefabName)
    {
        if (_aRDataManager.imageObjectMap.ContainsKey(trackedImage) && _aRDataManager.imageObjectMap[trackedImage] != null)
        {
            _aRDataManager.imageObjectMap[trackedImage].gameObject.SetActive(false);
            DeselectSelectedGameObject();
            _isSelecting = false;
        }

        GameObject newARObject;
        if (_aRDataManager.othersElements.Contains(prefabName))
        {
            newARObject = GetGameObject("other");
            // newARObject = Instantiate(Resources.Load<GameObject>("other"), Vector3.zero, Quaternion.Euler(0, 0, 0));
            TextMeshProUGUI[] texts = newARObject.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                text.text = prefabName;
            }
        }
        else
        {
            // newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
            newARObject = GetGameObject(prefabName);
        }

        newARObject.name = prefabName;
        newARObject.SetActive(false);
        _aRDataManager.imageObjectMap[trackedImage] = newARObject;
        _aRDataManager.AssociateMarkerImageAndUpdate(trackedImage.referenceImage.name, prefabName);

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
            _aRDataManager.imageObjectMap[trackedImage].SetActive(false);
            return;
        }

        _aRDataManager.imageObjectMap[trackedImage].gameObject.SetActive(true);
        _aRDataManager.imageObjectMap[trackedImage].transform.position = trackedImage.transform.position;
    }

    private GameObject GetGameObject(string prefabName)
    {
        GameObject newARObject = Instantiate(Resources.Load<GameObject>("Prefab/" + prefabName), Vector3.zero, Quaternion.Euler(-90, 0, 0));
        newARObject.name = prefabName;
        newARObject.SetActive(false);

        return newARObject;
    }






















    private Color GetInvertedColor(GameObject gameObject)
    {
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
        if (_isSelecting && _aRDataManager.imageObjectMap.ContainsValue(selectedObject) && SelectedImageObject != null && _aRDataManager.imageObjectMap[SelectedImageObject] == selectedObject)
        {
            DeselectSelectedGameObject();
            _isSelecting = false;
            return;
        }

        //get the ar tracked image associated to this object
        foreach (KeyValuePair<ARTrackedImage, GameObject> entry in _aRDataManager.imageObjectMap)
        {
            if (entry.Value == null || entry.Key == null)
            {
                continue;
            }
            Debug.Log("Entry: " + entry.Key.referenceImage.name + " -> " + entry.Value.name);
            if (entry.Value == selectedObject)
            {
                if (SelectedImageObject != null && SelectedImageObject != entry.Key) DeselectSelectedGameObject();

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
        if (SelectedImageObject == null || !_aRDataManager.imageObjectMap.ContainsKey(SelectedImageObject)) return;

        Outline outline = _aRDataManager.imageObjectMap[SelectedImageObject].GetComponent<Outline>();

        if (outline != null)
        {
            Destroy(outline);
        }

        Color color = _aRDataManager.imageObjectMap[SelectedImageObject].GetComponent<Renderer>().material.color;

        SelectedImageObject = null;
    }

    public bool SetPrefabOnSelected(string prefabName)
    {
        Debug.Log("Loading -> " + prefabName);
        if (SelectedImageObject == null) return false;
        GameObject oldGO = _aRDataManager.imageObjectMap[SelectedImageObject];
        ARTrackedImage aRTrackedImage = SelectedImageObject;

        if (_aRDataManager.imageObjectMap[aRTrackedImage] != null)
        {
            DeselectSelectedGameObject();
            _aRDataManager.imageObjectMap[aRTrackedImage].gameObject.SetActive(false);
            _isSelecting = false;
        }


        GameObject newARObject;
        if (_aRDataManager.othersElements.Contains(prefabName))
        {
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
        newARObject.SetActive(false);
        _aRDataManager.imageObjectMap[aRTrackedImage] = newARObject;
        _aRDataManager.AssociateMarkerImageAndUpdate(aRTrackedImage.referenceImage.name, prefabName);
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

    public bool OnPrefabSelected(string prefabName)
    {
        if (SelectedImageObject == null) return false; //aggiungere errore nella gui che indica che è necessario fare una selezione
        return SetPrefabOnSelected(prefabName);
    }


    bool soundOn = true;

    bool sameElementSoundCount = false;

    public void ClearAndAddElement(string prefabName, bool isSameElement = false)
    {

        soundOn = (!sameElementSoundCount && isSameElement) || !isSameElement;

        sameElementSoundCount = !sameElementSoundCount && isSameElement;

        bool elementAlreadyAdded = _elementFilesManager.AddFoundElement(prefabName.ToLower());

        DeselectSelectedGameObject();

        // ARTrackedImage targetImage = null;
        // bool first = true;
        // List<ARTrackedImage> imagesToReset = new();

        // foreach (KeyValuePair<ARTrackedImage, GameObject> entry in _aRDataManager.imageObjectMap)
        // {
        //     if (entry.Value == null || entry.Key == null)
        //     {
        //         continue;
        //     }
        //     if (first)
        //     {
        //         targetImage = entry.Key;
        //         first = false;
        //     }
        //     imagesToReset.Add(entry.Key);

        // }

        // foreach (ARTrackedImage image in imagesToReset)
        // {
        //     AssociateGameObjectToMarker(image, defaultObject.name);
        // }

        // if (targetImage != null)
        // {
        //     AssociateGameObjectToMarker(targetImage, prefabName);
        // }

        bool finded = false;
        AudioClip clip = Resources.Load<AudioClip>("Sounds/correct");

        if (!elementAlreadyAdded)
        {
            clip = Resources.Load<AudioClip>("Sounds/wrong");
            Debug.Log("Elemento già trovato (MITM): " + prefabName);
            finded = true;
        }
        else
        {
            _createButtonsComponent.ResetButtons();
            Debug.Log("ButtonLabels aggiornato con successo");
            finded = false;
            AchievementsCheck.Instance.FoundedElement(prefabName);
            ElementFilesManager.Instance.SetBalance(ElementFilesManager.Instance.GetBalance() + 1);
        }

        GameObject tempAudioObject = new GameObject("TempAudioObject");
        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = Resources.Load<AudioMixer>("Mixer").FindMatchingGroups("Master")[0];
        audioSource.clip = clip;

        if (soundOn)
        {
            audioSource.Play();
            SpawnPopUp(prefabName, finded);
        }

        Destroy(tempAudioObject, clip.length);
    }

    void SpawnPopUp(string prefabName = "default", bool alreadyFound = false)
    {
        GameObject spawnedObject;
        if (alreadyFound)
        {
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
                if (_aRDataManager.othersElements.Contains(prefabName))
                {
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

    public void SpawnPopUpNotExits()
    {
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

}