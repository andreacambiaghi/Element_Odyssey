using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSceneManager : MonoBehaviour
{

    public static ARSceneManager Instance;

    protected ElementFilesManager elementFilesManager;

    protected CreateButtons createButtons;

    [SerializeField] private GameObject slider;
    
    [SerializeField] private SliderMenuAnim menu;

    [SerializeField] protected GameObject createButtonsComponent;

    [SerializeField] protected GameObject popUpElementCreated;

    [SerializeField] protected GameObject popUpElementAlreadyFound;

    protected List<string> otherElements = new List<string>();


    protected ARSceneManager(){}

    protected void Awake()
    {
        // if(Instance == null)
        // {
        //     Instance = this;
        // }
        // else
        // {
        //     Destroy(this);
        // }

        // elementFilesManager = ElementFilesManager.Instance;
        // createButtons = createButtonsComponent.GetComponent<CreateButtons>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
