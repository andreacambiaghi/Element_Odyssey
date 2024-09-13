#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class PrefabModifier : MonoBehaviour
{

    public static PrefabModifier Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        Object[] prefabs = Resources.LoadAll("Prefab");

        foreach (Object prefab in prefabs)
        {
            string path = AssetDatabase.GetAssetPath(prefab);
            GameObject prefabInstance = PrefabUtility.LoadPrefabContents(path);

            if (prefabInstance != null && prefabInstance.name != "default")
            {
                ModifyPrefab(prefabInstance);

                PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);
                PrefabUtility.UnloadPrefabContents(prefabInstance);
            }
        }
    }

    private void ModifyPrefab(GameObject prefabInstance)
    {
        if (prefabInstance.tag != "Special") 
        {
            prefabInstance.transform.localScale = new Vector3(5f, 5f, 5f);
        }

        AddOrUpdateComponent<SphereCollider>(prefabInstance, sphereCollider =>
        {
            sphereCollider.radius *= 1.5f;
            sphereCollider.isTrigger = true;
        });

        AddOrUpdateComponent<Elements>(prefabInstance);
        AddOrUpdateComponent<ArTouchManager>(prefabInstance);
        AddOrUpdateComponent<Rigidbody>(prefabInstance, rigidbody =>
        {
            rigidbody.useGravity = false;
        });
    }

    private void AddOrUpdateComponent<T>(GameObject go, System.Action<T> configure = null) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        configure?.Invoke(component);
    }
}
#endif
