using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    [SerializeField] private GameObject createButton;
    private List<string> labelsToAdd = new List<string>();
    ElementFilesManager elementFilesManager;

    void Awake()
    {
        elementFilesManager = ElementFilesManager.Instance;

        if(elementFilesManager == null)
        {
            Debug.LogError("ElementFilesManager non trovato ->>>>>>>");
            return;
        }   

        InitializeButtons();
    }
 
    private void InitializeButtons(){
        if(GameModeManager.Instance.GameMode == "createMarker" || GameModeManager.Instance.GameMode == "VirtualPlane" || GameModeManager.Instance.GameMode == "Village"){
            labelsToAdd.AddRange(elementFilesManager.GetInitialElements());
            labelsToAdd.AddRange(elementFilesManager.GetFoundElements());

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

}
