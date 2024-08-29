#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class PrefabModifier : MonoBehaviour
{
    void Start()
    {
        Object[] prefabs = Resources.LoadAll("Prefab");

        foreach (Object prefab in prefabs)
        {
            GameObject prefabInstance = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(prefab));

            if (prefabInstance != null)
            {
                SphereCollider sphereCollider = prefabInstance.GetComponent<SphereCollider>();
                if (sphereCollider == null)
                {
                    sphereCollider = prefabInstance.AddComponent<SphereCollider>();
                    sphereCollider.radius *= 1.5f;
                }

                sphereCollider.isTrigger = true;

                if (prefabInstance.GetComponent<Elements>() == null)
                {
                    prefabInstance.AddComponent<Elements>();
                }

                if (prefabInstance.GetComponent<ArTouchManager>() == null)
                {
                    prefabInstance.AddComponent<ArTouchManager>();
                }

                PrefabUtility.SaveAsPrefabAsset(prefabInstance, AssetDatabase.GetAssetPath(prefab));
                PrefabUtility.UnloadPrefabContents(prefabInstance);
            }
        }
    }
}
#endif
