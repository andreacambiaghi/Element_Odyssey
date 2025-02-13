using UnityEngine;
using UnityEngine.UI;

public class ClickFloor : MonoBehaviour
{
    private Material outlineMaterial;
    private bool isSelected = false;

    void Start()
    {
        outlineMaterial = GetComponent<Image>().material;
        outlineMaterial.SetFloat("_OutlineThickness", 0f); // Contorno inizialmente spento
    }

    public void ToggleOutline()
    {
        isSelected = !isSelected;
        outlineMaterial.SetFloat("_OutlineThickness", isSelected ? 0.02f : 0f);
    }
}
