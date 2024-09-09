using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScenes : MonoBehaviour
{
    public void CreateMarkerScene()
    {
        SceneManager.LoadScene("CreateMarkerMode");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void PlaneScene()
    {
        SceneManager.LoadScene("PlaneMode");
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
