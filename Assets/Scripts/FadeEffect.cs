using UnityEngine;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    public float fadeDuration = 5.0f; // Duration of the fade effect
    private Renderer objectRenderer;
    private Color originalColor;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("Renderer component not found!");
            return;
        }
        originalColor = objectRenderer.material.color;


        
        StartFading();
    }

    public void StartFading()
    {
        Debug.Log("Start fading.");
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {  
        while(true){
            // Fade out
        
            yield return StartCoroutine(Fade(1, 0));

            // Fade in
            yield return StartCoroutine(Fade(0, 1));
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;
        Color color = objectRenderer.material.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            objectRenderer.material.color = new Color(color.r, color.g, color.b, alpha);

            Debug.Log("Fade value -> " + alpha);
            yield return null;
        }

        objectRenderer.material.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
