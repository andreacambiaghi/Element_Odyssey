using System.IO;
using UnityEngine;

public class DownloadMarkers : MonoBehaviour
{
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
}
