using UnityEngine;
using UnityEngine.SceneManagement;

public class BackHome : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
