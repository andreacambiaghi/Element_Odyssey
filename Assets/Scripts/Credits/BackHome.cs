using UnityEngine;
using UnityEngine.SceneManagement;

public class BackHome : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("Menu");
    }
}
