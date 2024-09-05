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
}
