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
