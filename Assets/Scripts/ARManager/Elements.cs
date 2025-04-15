using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elements : MonoBehaviour
{
    // List of elements this one is interacting with
    private List<Elements> _elementsInteractingWith = new List<Elements>();

    private GameModeManager gameModeManager;

    // Instance of the instantiated prefab
    private GameObject instantiatedPrefab;

    private void Start()
    {
        gameModeManager = GameModeManager.Instance;
    }


    // Add the element that triggered with this one to the list
    private void OnTriggerEnter(Collider other)
    {
        Elements element = other.GetComponent<Elements>();
        if (element != null)
        {
            AddElement(element);
        }
    }

    // Remove the element is exiting from the trigger domain
    private void OnTriggerExit(Collider other)
    {
        Elements element = other.GetComponent<Elements>();
        if (element != null)
        {
           RemoveElement(element);
        }
    }

    private void OnDisable()
    {
        if (_elementsInteractingWith.Count > 0)
        {
            foreach (Elements element in _elementsInteractingWith)
            {
                element.RemoveElement(this);
            }
        }
    }

    private void AddElement(Elements element)
    {
        _elementsInteractingWith.Add(element);
        Interact(element);
    }

    private void RemoveElement(Elements element)
    {
        _elementsInteractingWith.Remove(element);
        if(_elementsInteractingWith.Count == 0)
        {
            Sleep();
        }
    }

    // Create Sleep action
    private void Sleep()
    {
        
    }
    
    private void Interact(Elements element)
    {
        ElementPair elementPair = new ElementPair(this.name, element.name);
        if (gameModeManager.GameMode == "CreateMarker") {
            MultipleImagesTrackingManager mitm = GameObject.Find("MultipleImagesTrackingManager").GetComponent<MultipleImagesTrackingManager>();
            if (ReadCSV.Instance.elementAssociations.TryGetValue(elementPair, out string resultPrefabName)) {
                mitm.ClearAndAddElement(resultPrefabName, element.gameObject);
            } else {
                ElementPair elementPairBis = new ElementPair(element.name, this.name);
                if (!ReadCSV.Instance.elementAssociations.TryGetValue(elementPairBis, out string resultPrefabNameBis))
                    mitm.SpawnPopUpNotExits();
            } 
        } else {
            VirtualPlaneManager.Instance.ClearAndAddElement(elementPair, gameObject, element.gameObject);
        }
    }


}