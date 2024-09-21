using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ButtonForVillage : MonoBehaviour
{
    [SerializeField] private Button buttonPrefab; // Prefab del bottone da clonare
    [SerializeField] public List<string> buttonLabels; // Lista di etichette per i bottoni
    [SerializeField] private GameObject elementSelected; // Elemento selezionato 

    private GameModeManager gameModeManager;

    private ElementFilesManager elementFilesManager;

    public void Start()
    {
        elementFilesManager = ElementFilesManager.Instance;
        gameModeManager = GameModeManager.Instance;

        ResetButtons();

        // foreach (string label in buttonLabels)
        // {
        //     CreateButton(label);
        // }
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

        if (gameModeManager.GameMode == "CreateMarker")
            newButton.onClick.AddListener(() => MultipleImagesTrackingManager.Instance.OnPrefabSelected(buttonLabelLowercase));
        else if (gameModeManager.GameMode == "VirtualPlane")
            newButton.onClick.AddListener(() => {
                VirtualPlaneManager.Instance.OnPrefabSelected(buttonLabelLowercase);
                elementSelected.GetComponentInChildren<TextMeshProUGUI>().text = "You selected: " + buttonLabelLowercase;
            });
            
        else
            Debug.LogError("Game mode not recognized");

        // Carica lo sprite corrispondente dal percorso Resources/Icon
        Sprite sprite = Resources.Load<Sprite>($"Icon/{buttonLabelLowercase}");

        if (sprite != null)
        {
            // Trova l'oggetto figlio 'IconElement' nel bottone appena creato
            Transform iconElementTransform = newButton.transform.Find("Image/IconElement");
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

    public void ClearButtons()
    {
        buttonLabels = new List<string>();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetButtons()
    {
        ClearButtons();

        Debug.LogWarning("Button labels cleared: " + string.Join(", ", buttonLabels));
        buttonLabels = elementFilesManager.GetInitialElements();
        buttonLabels.AddRange(elementFilesManager.GetFoundElements());

        // TODO: aggiustare
        buttonLabels = buttonLabels.Distinct().ToList();

        Debug.LogWarning("Button labels to update: " + string.Join(", ", buttonLabels));
    
        foreach (string label in buttonLabels)
        {
            CreateButton(label);
        }
    }

}
