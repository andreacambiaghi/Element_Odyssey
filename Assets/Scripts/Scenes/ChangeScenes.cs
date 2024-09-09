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
}
