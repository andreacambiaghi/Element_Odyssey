using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateButtons : MonoBehaviour
{
    public Button buttonPrefab; // Prefab del bottone da clonare
    public List<string> buttonLabels; // Lista di etichette per i bottoni

    public void Start()
    {
        CreateButtonsDynamic();
    }
    public void CreateButtonsDynamic()
    {
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
            newButton.gameObject.SetActive(true);
        }
    }
}
