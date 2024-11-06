using UnityEngine;

public class UISpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefabBelow;
    [SerializeField] private GameObject objectPrefabAbove;

    private GameObject spawnedObjectBelow;
    private GameObject spawnedObjectAbove;

    void Start()
    {
        SpawnUIElements();
    }

    private void SpawnUIElements()
    {
        spawnedObjectBelow = Instantiate(objectPrefabBelow, transform);
        spawnedObjectAbove = Instantiate(objectPrefabAbove, transform);

        Canvas canvasBelow = spawnedObjectBelow.GetComponent<Canvas>();
        Canvas canvasAbove = spawnedObjectAbove.GetComponent<Canvas>();

        if (canvasBelow != null && canvasAbove != null)
        {
            canvasBelow.sortingOrder = 0;
            canvasAbove.sortingOrder = 1;
        }
        else
        {
            spawnedObjectBelow.transform.SetAsFirstSibling();
            spawnedObjectAbove.transform.SetAsLastSibling();
        }
    }
}
