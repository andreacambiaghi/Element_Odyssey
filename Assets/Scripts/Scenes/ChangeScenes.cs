using UnityEngine.SceneManagement;
using UnityEngine;

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
        SceneManager.LoadScene("CreateMarkerMode");
    }

    public void MenuScene()
    {
        DestroyPersistentObjects();
        gameModeManager.GameMode = "Menu";
        gameModeManager.IsMenuOpen = false;
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
            if (obj.scene.buildIndex == -1) // Gli oggetti con -1 sono quelli che persistono tra le scene
            {
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
