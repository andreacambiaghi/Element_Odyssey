using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elements : MonoBehaviour
{
    // List of elements this one is interacting with
    private List<Elements> _elementsInteractingWith = new List<Elements>();

    // Instance of the instantiated prefab
    private GameObject instantiatedPrefab;

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

    // Remove this element when it is hidden/disabled from the cars this is interacting with
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

    // Create Interact action
    private void Interact(Elements element)
    {
        Vector3 midpoint = (transform.position + element.transform.position) / 2;
        Vector3 offset = new Vector3(0, 0.01f, 0); // Adjust the offset as needed
        Vector3 spawnPosition = midpoint + offset;

        if (instantiatedPrefab == null)
        {
            ElementPair elementPair = new ElementPair(this.name, element.name);
            Debug.Log("Element pair: " + elementPair.ToString());

            if (ReadCSV.Instance.elementAssociations.TryGetValue(elementPair, out string resultPrefabName))
            {
                MultipleImagesTrackingManager.Instance.ClearAndAddElement(resultPrefabName);
                // GameObject resultPrefab = Resources.Load<GameObject>($"Prefab/{resultPrefabName}");
                // if (resultPrefab != null)
                // {
                //     instantiatedPrefab = Instantiate(resultPrefab, spawnPosition, Quaternion.identity);
                //     Debug.Log($"Instantiated prefab: {resultPrefabName}");
                // }
                // else
                // {
                //     Debug.LogError($"Prefab '{resultPrefabName}' not found in Resources/Prefab.");
                // }
            }
            else
            {
                Debug.Log($"No association found for element pair: {elementPair.ToString()}");
            }
        }
    }


}