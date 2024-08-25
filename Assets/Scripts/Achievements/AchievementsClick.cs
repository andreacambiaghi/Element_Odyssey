using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementsClick : MonoBehaviour
{
    public Color newColor = Color.red;
    private Color originalColor;
    private Image imageComponent;
    private bool isColorChanged = false;
    private static readonly string achievementTag = "Achievement";
    private GameObject panelInfo;
    private AchievementsInfo achievementsInfo = new();
    private AchievementIdentifier achievementIdentifier;
    private RectTransform rectTransform;
    private int id;
    private TextMeshProUGUI textComponent;

    private void Start()
    {
        achievementIdentifier = GetComponent<AchievementIdentifier>();
        id = achievementIdentifier.uniqueID;

        panelInfo = GameObject.FindWithTag("Info");
        rectTransform = panelInfo.GetComponent<RectTransform>();
        textComponent = panelInfo.GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(rectTransform.rect.width);

        imageComponent = GetComponent<Image>();
        if (imageComponent != null)
        {
            originalColor = imageComponent.color;
        }
        else
        {
            Debug.Log("Il prefab non ha un componente Image");
        }

        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
        else
        {
            Debug.Log("Il prefab non ha un componente Button");
        }
    }

    private void OnClick()
    {
        Debug.Log("Achievement " + id + " clicked");
        RestoreOriginalColors();
        if (imageComponent != null)
        {
            if (isColorChanged)
            {
                imageComponent.color = originalColor;
                rectTransform.sizeDelta = new Vector2(0, rectTransform.rect.height);
                textComponent.text = "";
            }
            else
            {
                imageComponent.color = newColor;
                rectTransform.sizeDelta = new Vector2(550, rectTransform.rect.height);
                textComponent.text = achievementsInfo.GetAchievementInfo(id);

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
            if (script != null && script != this)
            {
                script.imageComponent.color = script.originalColor;
                script.isColorChanged = false;
                rectTransform.sizeDelta = new Vector2(0, rectTransform.rect.height);
                textComponent.text = "";
            }
        }
    }

    private void OnDisable()
    {
        if (imageComponent != null)
        {
            imageComponent.color = originalColor;
            rectTransform.sizeDelta = new Vector2(0, rectTransform.rect.height);
            textComponent.text = "";
            isColorChanged = false;
        }
    }
}
