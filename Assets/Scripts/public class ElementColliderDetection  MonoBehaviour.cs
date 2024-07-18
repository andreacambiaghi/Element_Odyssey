using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementColliderDetection : MonoBehaviour
{
    [SerializeField] private GameObject interactionPrefab; // Prefab to instantiate
    [SerializeField] private float interactionDistance = 1.0f; // Distance to consider as interaction

    // List to keep track of interactions
    private List<(GameObject, GameObject)> _interactions = new List<(GameObject, GameObject)>();

    void Update()
    {
        CheckInteractions();
    }

    private void CheckInteractions()
    {
        // Find all objects with the tag "Element"
        GameObject[] elements = GameObject.FindGameObjectsWithTag("Element");
        Debug.Log("Found " + elements.Length + " elements");

        // Check each pair of elements for interaction
        for (int i = 0; i < elements.Length; i++)
        {
            for (int j = i + 1; j < elements.Length; j++)
            {
                GameObject element1 = elements[i];
                GameObject element2 = elements[j];

                // Check if they are within interaction distance and their names are "water" and "fire"
                if (Vector3.Distance(element1.transform.position, element2.transform.position) <= interactionDistance)
                {
                    if ((element1.name == "water" && element2.name == "fire") || (element1.name == "fire" && element2.name == "water"))
                    {
                        if (!_interactions.Contains((element1, element2)) && !_interactions.Contains((element2, element1)))
                        {
                            AddInteraction(element1, element2);
                        }
                    }
                }
                else
                {
                    RemoveInteraction(element1, element2);
                }
            }
        }
    }

    private void AddInteraction(GameObject element1, GameObject element2)
    {
        _interactions.Add((element1, element2));
        Interact(element1, element2);
    }

    private void RemoveInteraction(GameObject element1, GameObject element2)
    {
        _interactions.RemoveAll(interaction => (interaction.Item1 == element1 && interaction.Item2 == element2) ||
                                               (interaction.Item1 == element2 && interaction.Item2 == element1));
    }

    private void Interact(GameObject element1, GameObject element2)
    {
        if (interactionPrefab != null)
        {
            Vector3 midpoint = (element1.transform.position + element2.transform.position) / 2;
            Vector3 offset = new Vector3(0, 0.01f, 0); // Adjust the offset as needed
            Vector3 spawnPosition = midpoint + offset;

            Instantiate(interactionPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Instantiated prefab " + interactionPrefab.name + " at " + spawnPosition);
        }
    }
}
