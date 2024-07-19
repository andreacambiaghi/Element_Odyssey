using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateButtons : MonoBehaviour
{
    public Button buttonPrefab; // Prefab del bottone da clonare
    public List<string> buttonLabels; // Lista di etichette per i bottoni
    public float spacing = 10f; // Spazio tra i bottoni

    public void Start()
    {
        CreateButtonsDynamic();
    }

    public void CreateButtonsDynamic()
    {
        int buttonCount = buttonLabels.Count;
        RectTransform parentRectTransform = GetComponent<RectTransform>();

        // Calcola la dimensione totale necessaria per i bottoni
        float totalHeight = (buttonPrefab.GetComponent<RectTransform>().sizeDelta.y + spacing) * buttonCount - spacing;
        float startY = -totalHeight / 2;

        for (int i = 0; i < buttonLabels.Count; i++)
        {
            // Creazione del bottone
            Button newButton = Instantiate(buttonPrefab, transform);

            // Modifica dell'etichetta del bottone
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            Image buttonImage = GetComponent<Image>();

            if (buttonText != null)
            {
                buttonText.text = buttonLabels[i];
            }

            // Aggiungi il listener al bottone
            string buttonLabelLowercase = buttonLabels[i].ToLower();
            newButton.onClick.AddListener(() => MultipleImagesTrackingManager.Instance.OnPrefabSelected(buttonLabelLowercase));

            // Posiziona il bottone
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = new Vector2(0, startY + i * (buttonRectTransform.sizeDelta.y + spacing));

            newButton.gameObject.SetActive(true);
        }
    }
}
