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
        gameModeManager.GameMode = "CreateMarker";
        SceneManager.LoadScene("CreateMarkerMode");
    }

    public void MenuScene()
    {
        gameModeManager.GameMode = "Menu";
        SceneManager.LoadScene("Menu");
    }

    public void VirtualPlaneScene()
    {
        gameModeManager.GameMode = "VirtualPlane";
        SceneManager.LoadScene("VirtualPlaneMode");
    }

    public void VillageScene()
    {
        gameModeManager.GameMode = "Village";
        SceneManager.LoadScene("Village");
    }

    public void CreditsScene()
    {
        gameModeManager.GameMode = "Credits";
        SceneManager.LoadScene("Credits");
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
