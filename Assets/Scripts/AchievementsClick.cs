using UnityEngine;
using UnityEngine.UI;

public class AchievementsClick : MonoBehaviour
{
    public Color newColor = Color.red;
    private Color originalColor;
    private Image imageComponent;
    private bool isColorChanged = false;
    private static readonly string achievementTag = "Achievement";

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        if (imageComponent != null)
        {
            originalColor = imageComponent.color;
        }

        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        RestoreOriginalColors();

        if (imageComponent != null)
        {
            if (isColorChanged)
            {
                imageComponent.color = originalColor;
            }
            else
            {
                imageComponent.color = newColor;
            }
            isColorChanged = !isColorChanged;
        }
    }

    private void RestoreOriginalColors()
    {
        GameObject[] achievements = GameObject.FindGameObjectsWithTag(achievementTag);
        foreach (GameObject obj in achievements)
        {
            AchievementsClick script = obj.GetComponent<AchievementsClick>();
            if (script != null)
            {
                script.imageComponent.color = script.originalColor;
                script.isColorChanged = false;
            }
        }
    }

    private void OnDisable()
    {
        if (imageComponent != null)
        {
            imageComponent.color = originalColor;
        }
    }
}
