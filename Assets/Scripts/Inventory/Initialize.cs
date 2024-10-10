using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    [SerializeField] private GameObject createButton;
    private List<string> labelsToAdd = new List<string>();
    ElementFilesManager elementFilesManager;


    // Start is called before the first frame update
    void Awake()
    {
        //elementFilesManager = ElementFilesManager.GetInstance();
        elementFilesManager = ElementFilesManager.Instance;

        if(elementFilesManager == null)
        {
            Debug.LogError("ElementFilesManager non trovato ->>>>>>>");
            return;
        }   

        InitializeButtons();
    }
 
    private void InitializeButtons(){
        labelsToAdd.AddRange(elementFilesManager.GetInitialElements());
        labelsToAdd.AddRange(elementFilesManager.GetFoundElements());
        // LoadLabelsFromFile("InitialElements");
        // LoadLabelsFromFile("Founds");

        CreateButtons createButtonsComponent = createButton.GetComponent<CreateButtons>();
        if (createButtonsComponent != null)
        {
            createButtonsComponent.buttonLabels.AddRange(labelsToAdd);
            Debug.Log("ButtonLabels aggiornato con successo");
        }
        else
        {
            Debug.LogError("Il GameObject target non ha il componente 'CreateButtons'");
        }
    }
}
