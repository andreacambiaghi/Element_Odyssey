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

        // Carica lo sprite corrispondente dal percorso Resources/Icon
        Sprite sprite = Resources.Load<Sprite>($"Icon/{buttonLabelLowercase}");

        if (sprite != null)
        {
            // Trova l'oggetto figlio 'IconElement' nel bottone appena creato
            Transform iconElementTransform = newButton.transform.Find("IconElement");
            if (iconElementTransform != null)
            {
                // Ottieni il componente Image del figlio 'IconElement'
                Image iconImage = iconElementTransform.GetComponent<Image>();
                if (iconImage != null)
                {
                    iconImage.sprite = sprite; // Assegna lo sprite al componente Image
                }
                else
                {
                    Debug.LogError("Il GameObject 'IconElement' non ha un componente Image");
                }
            }
            else
            {
                Debug.LogError("Il GameObject 'IconElement' non è stato trovato come figlio del bottone");
            }
        }
        else
        {
            Debug.LogError($"Sprite '{buttonLabelLowercase}' non trovato nella cartella Resources/Icon");
        }

        // Posiziona il bottone
        RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();

        newButton.gameObject.SetActive(true);
    }
}
