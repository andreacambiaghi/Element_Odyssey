using UnityEngine;

public class ComponentAdder : MonoBehaviour
{
    public void AddComponentsToGameObject(GameObject go)
    {
        AddOrUpdateComponent<SphereCollider>(go, sphereCollider =>
        {
            sphereCollider.radius *= 1.5f;
            sphereCollider.isTrigger = true;
        });

        AddOrUpdateComponent<Elements>(go);
        AddOrUpdateComponent<ArTouchManager>(go);
        AddOrUpdateComponent<Rigidbody>(go, rigidbody =>
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
