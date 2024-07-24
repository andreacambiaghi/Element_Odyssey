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

        for (int i = 0; i < buttonLabels.Count; i++)
        {
            // Creazione del bottone
            Button newButton = Instantiate(buttonPrefab, transform);

            // Modifica dell'etichetta del bottone
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                buttonText.text = buttonLabels[i];
            }

            // Aggiungi il listener al bottone
            string buttonLabelLowercase = buttonLabels[i].ToLower();
            newButton.onClick.AddListener(() => MultipleImagesTrackingManager.Instance.OnPrefabSelected(buttonLabelLowercase));

            // Posiziona il bottone
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();

            newButton.gameObject.SetActive(true);
        }
    }
}
