using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ChangeScenes : MonoBehaviour
{

    private GameModeManager gameModeManager;

    private void Awake()
    {
        gameModeManager = GameModeManager.Instance;
    }

    public void CreateMarkerScene()
    {
        DestroyPersistentObjects();
        gameModeManager.GameMode = "CreateMarker";
        gameModeManager.IsMenuOpen = false;

        // MultipleImagesTrackingManager instance = MultipleImagesTrackingManager.Instance;

        // if(instance == null){
        //     instance = new MultipleImagesTrackingManager();
        //     // instance.isSceneBeingStarted = true;    
        // }
        // if (instance != null)
        // {
        //     instance.isSceneBeingStarted = true;
        //     // instance.Reset();
        // }
        // else
        // {
        //     Debug.LogError("Ouch, the mitm instance is null..");
        // }

        SceneManager.LoadScene("CreateMarkerMode");
    }

    public void MenuScene()
    {
        DestroyPersistentObjects();
        gameModeManager.GameMode = "Menu";
        gameModeManager.IsMenuOpen = false;
        SceneManager.LoadScene("Menu");
    }

    public void MenuSceneResetMarkers()
    {
        DestroyPersistentObjects();
        gameModeManager.GameMode = "Menu";
        gameModeManager.IsMenuOpen = false;
        // MultipleImagesTrackingManager instance = MultipleImagesTrackingManager.Instance;

        // if (instance != null)
        // {
        //     instance.isSceneBeingStarted = true;
        //     instance.Reset();
        // }
        // else
        // {
        //     Debug.LogError("Ouch, the mitm instance is null..");
        // }
        ARSession arSession = FindObjectOfType<ARSession>(); // Or get a reference however you manage it
        if (arSession != null)
        {
            Debug.Log("Resetting AR Session.");
            arSession.Reset();
        }
        else
        {
            Debug.LogWarning("AR Session component not found. Cannot reset tracking.");
        }
        
        SceneManager.LoadScene("Menu");
    }

    public void VirtualPlaneScene()
    {
        DestroyPersistentObjects();
        gameModeManager.GameMode = "VirtualPlane";
        gameModeManager.IsMenuOpen = false;
        SceneManager.LoadScene("VirtualPlaneMode");
    }

    public void VillageScene()
    {
        DestroyPersistentObjects();
        gameModeManager.GameMode = "Village";
        gameModeManager.IsMenuOpen = false;
        SceneManager.LoadScene("Village");
    }

    public void CreditsScene()
    {
        DestroyPersistentObjects();
        gameModeManager.GameMode = "Credits";
        gameModeManager.IsMenuOpen = false;
        SceneManager.LoadScene("Credits");
    }

    private void DestroyPersistentObjects()
    {
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.scene.buildIndex == -1 && obj.name != "ARDataManager") // Gli oggetti con -1 sono quelli che persistono tra le scene
            {
                Debug.Log("Destroying persistent object: " + obj.name);
                Destroy(obj);
            }
        }
    }
}


// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class ChangeScenes : MonoBehaviour
// {
//     private string currentScene = "Menu";

//     public void CreateMarkerScene()
//     {
//         SwitchToScene("CreateMarkerMode");
//     }

//     public void PlaneScene()
//     {
//         SwitchToScene("PlaneMode");
//     }

//     public void MenuScene()
//     {
//         SwitchToScene("Menu");
//     }

//     private void SwitchToScene(string sceneToLoad)
//     {
//         if (currentScene != sceneToLoad)
//         {
//             SceneManager.LoadSceneAsync(sceneToLoad);

//             currentScene = sceneToLoad;
//         }
//     }
// }
