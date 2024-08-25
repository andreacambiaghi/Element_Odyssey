using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateButtons : MonoBehaviour
{
    [SerializeField] private Button buttonPrefab; // Prefab del bottone da clonare
    [SerializeField] public List<string> buttonLabels; // Lista di etichette per i bottoni

    public void Start()
    {
        foreach (string label in buttonLabels)
        {
            CreateButton(label);
        }
    }

    public void CreateButton(string label)
    {
        // Creazione del bottone
        Button newButton = Instantiate(buttonPrefab, transform);

        // Modifica dell'etichetta del bottone
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = label;
        }

        // Aggiungi il listener al bottone
        string buttonLabelLowercase = label.ToLower();
        newButton.onClick.AddListener(() => MultipleImagesTrackingManager.Instance.OnPrefabSelected(buttonLabelLowercase));

        // Posiziona il bottone
        RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();

        newButton.gameObject.SetActive(true);
    }
}
