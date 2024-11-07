using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffectTMP : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public float delay = 0.05f;

    public void StartTyping(string fullText)
    {
        StartCoroutine(ShowText(fullText));
    }

    private IEnumerator ShowText(string fullText)
    {
        uiText.text = "";
        for (int i = 0; i <= fullText.Length; i++)
        {
            uiText.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
    }
}
