using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClickAnimation : MonoBehaviour
{
    [Header("Impostazioni")]
    [SerializeField] private Sprite[] frames; // Inserisci i due sprite qui
    private readonly float frameDelay = 0.5f;
    private readonly bool loop = true;

    private SpriteRenderer spriteRenderer;
    private Image uiImage;
    private int currentFrame = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiImage = GetComponent<Image>();

        if (spriteRenderer == null && uiImage == null)
        {
            Debug.LogError("Nessun componente SpriteRenderer o UI Image trovato!");
            return;
        }

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        do
        {
            for (currentFrame = 0; currentFrame < frames.Length; currentFrame++)
            {
                if (spriteRenderer != null)
                    spriteRenderer.sprite = frames[currentFrame];
                else if (uiImage != null)
                    uiImage.sprite = frames[currentFrame];

                yield return new WaitForSeconds(frameDelay);
            }
        }
        while (loop);
    }
}
