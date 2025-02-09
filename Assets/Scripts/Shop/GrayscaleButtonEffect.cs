using UnityEngine;
using UnityEngine.UI;

public class GrayscaleButtonEffect : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Material grayscaleMaterial;

    private void Start()
    {
        if (targetObject != null)
        {
            Button[] buttons = targetObject.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                Image image = button.GetComponent<Image>();
                if (image != null)
                {
                    image.material = grayscaleMaterial;
                }
            }
        }
    }
}
