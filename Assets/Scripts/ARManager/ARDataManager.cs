using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARDataManager : Singleton<ARDataManager>
{
    public Dictionary<ARTrackedImage, GameObject> imageObjectMap;

    private ElementFilesManager _elementFilesManager;

    // [SerializeField]
    // public GameObject elementFilesManagerPrefab;

    [System.NonSerialized]
    public ElementFilesManager.ArMarkerAssociations arMarkerAssociations;

    [System.NonSerialized]
    public List<string> othersElements;

    private void Awake()
    {
        if (FindObjectsOfType(typeof(ARDataManager)).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);

            imageObjectMap = new Dictionary<ARTrackedImage, GameObject>();
            // _elementFilesManager = elementFilesManagerPrefab.GetComponent<ElementFilesManager>();
            _elementFilesManager = ElementFilesManager.Instance;
            othersElements = ElementFilesManager.Instance.GetOthersElements();

            
            Debug.LogWarning("Others elements: " + othersElements.Count);

            // if (elementFilesManagerPrefab == null)
            // {
            //     Debug.LogError("ElementFilesManagerPrefab is null");
            //     return;
            // }
            if (othersElements == null)
            {
                Debug.LogError("Others elements is null");
            }

            while (_elementFilesManager == null)
            {
                Debug.LogError("ElementFilesManager is null");
                _elementFilesManager = ElementFilesManager.Instance;
                if (_elementFilesManager != null)
                {
                    Debug.LogError("ElementFilesManager is NOT null anymore!");
                    break;
                }
            }

            if (_elementFilesManager == null)
            {
                Debug.LogError("ElementFilesManager is still null after waiting!");
            }
            else
            {
                Debug.LogWarning("ElementFilesManager is NOT null anymore!");
            }


            Debug.LogError($"ELEMENT TEST: {_elementFilesManager.GetElementType("sky")}");
            ReloadMarkerImageAssociations();

        }
    }

    public void ReloadMarkerImageAssociations()
    {
        if (_elementFilesManager == null)
        {
            Debug.LogError("ElementFilesManager is null");
            return;
        }

        arMarkerAssociations = _elementFilesManager.GetArMarkerAssociations();
        if (arMarkerAssociations == null)
        {
            Debug.LogError("(ADM) ArMarkerAssociations is null");
            return;
        }
        Debug.LogWarning($"Reloaded {arMarkerAssociations.associations.Count} image associations");
    }

    public void AssociateMarkerImageAndUpdate(string markerName, string prefabName)
    {

        Debug.Log($"[ADM] Before update - Association count: {arMarkerAssociations.associationList.Count}");
        Debug.Log($"[ADM] Adding association: {markerName} -> {prefabName}");

        arMarkerAssociations.AddAssociation(markerName, prefabName);

        Debug.Log($"[ADM] After update - Association count: {arMarkerAssociations.associationList.Count}");
        Debug.Log($"[ADM] Dumping all associations:");
        foreach (var assoc in arMarkerAssociations.associationList)
        {
            Debug.Log($"[ADM] Marker: {assoc.markerId} -> Element: {assoc.elementType}");
        }

        _elementFilesManager.UpdateMarkerAssociations(arMarkerAssociations);
    }


}