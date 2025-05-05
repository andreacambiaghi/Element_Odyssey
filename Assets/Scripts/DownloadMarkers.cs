using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class DownloadMarkers : MonoBehaviour
{
    private string originalText;

    public void SavePdfFromResources()
    {
        TextAsset pdfAsset = Resources.Load<TextAsset>("Markers");

        if (pdfAsset == null)
        {
            Debug.LogError("Markers.pdf non trovato in Resources!");
            return;
        }

        string filePath = Path.Combine(Application.persistentDataPath, "Markers.pdf");
        File.WriteAllBytes(filePath, pdfAsset.bytes);
        Debug.Log("PDF salvato in: " + filePath);
    }

    public void DownloadAllImagesToGallery()
    {
        Debug.Log("Inizio download immagini...");
        // Load all textures from Resources/Image (Assets/Resources/Image)
        Object[] textures = Resources.LoadAll("Image", typeof(Texture2D));
        int count = 0;

        if (textures.Length == 0)
        {
            Debug.LogError("Nessuna immagine trovata in Resources/Image!");
            return;
        }
        
        foreach (Object obj in textures)
        {
            Texture2D tex = obj as Texture2D;
            if (tex != null)
            {
                // Use a unique filename for each image
                string fileName = $"Marker_{count}.png";
                // Save to gallery under album "MyAppImages"
                NativeGallery.SaveImageToGallery(tex, "ElementOdysseyMarkers", fileName, (success, path) =>
                {
                    Debug.Log($"Saved {fileName}: {success}, path: {path}");
                });
                count++;
            }
        }
    }

    public void ChangeNameButton()
    {
        TextMeshProUGUI childText = GetComponentInChildren<TextMeshProUGUI>();

        if (originalText == null)
        {
            originalText = childText.text;
        }

        childText.text = "Done!";
        StartCoroutine(RestoreTextAfterDelay(childText, 1f));
    }

    private IEnumerator RestoreTextAfterDelay(TextMeshProUGUI textMesh, float delay)
    {
        yield return new WaitForSeconds(delay);
        textMesh.text = originalText;
    }
}
