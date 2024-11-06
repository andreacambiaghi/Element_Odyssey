using UnityEngine;

public class ArTouchManager : MonoBehaviour
{
    private Color defaultColor;
    private Color selectedColor;

    private MultipleImagesTrackingManager mitm;

    private VirtualPlaneManager vpm;

    private VillagePlaneManager vlpm;

    private GameModeManager gameModeManager;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        mitm = MultipleImagesTrackingManager.Instance;
        defaultColor = objectRenderer.material.color;

        gameModeManager = GameModeManager.Instance;
        vpm = VirtualPlaneManager.Instance;

        vlpm = VillagePlaneManager.Instance;
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        Select();
                    }
                }
            }
        }
    }

    void Select()
    {
        //objectRenderer.material.color = new Color(Random.value, Random.value, Random.value);
        if(gameModeManager.GameMode == "CreateMarker")
            mitm.SelectGameObject(gameObject);
        else if(gameModeManager.GameMode == "VirtualPlane")
            vpm.SelectGameObject(gameObject);
        else if(gameModeManager.GameMode == "Village")
            vlpm.SelectGameObject(gameObject);
        else
            Debug.LogError("Game mode not recognized");
    }

    public void Deselect()
    {
        objectRenderer.material.color = defaultColor;
    }
}
