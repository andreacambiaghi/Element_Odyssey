using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScrollText : MonoBehaviour
{
    private float scrollSpeed = 100f;
    private RectTransform rectTransform;
    private bool hasExited = false;
    private float exitTime = 5f;
    private float timer = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (!hasExited && rectTransform.anchoredPosition.y > Screen.height)
        {
            hasExited = true;
        }

        if (hasExited)
        {
            timer += Time.deltaTime;
            if (timer >= exitTime)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
