using TMPro;
using UnityEngine;

public class UpdateCardMenu : MonoBehaviour
{

    private ElementFilesManager _elementFilesManager;
    private ElementFilesManager.ArMarkerAssociations arMarkerAssociations;

    void Awake()
    {
        _elementFilesManager = ElementFilesManager.Instance;
    }
    void OnEnable()
    {
        // Prendo il nome del gameobject
        string type = gameObject.name;
        // Rimuovo la parola "Page" dal nome
        type = type.Replace("Page", "");

        Debug.Log("Button type" + type);

        // Prendo i figli del gameobject
        Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
        
        foreach (Transform child in children)
        {
            // Per ogni figlio rimuovo la parola "BigBox" dal nome
            string childName = child.name;
            childName = childName.Replace("BigBox", "");  
            arMarkerAssociations = _elementFilesManager.GetArMarkerAssociations();

            foreach (var assoc in arMarkerAssociations.associationList)
            {
                Debug.Log($"[ADM] Marker: {assoc.markerId} -> Element: {assoc.elementType}");
                if (assoc.markerId  == childName)
                {
                    Debug.Log($"[ADM] Found association: {assoc.markerId} -> {assoc.elementType}");
                    // Scrivo nel bottone assoc.elementType
                    // Prendo il figlio chiamato "Testo" e modifico il testo con type
                    GameObject textChild = child.Find("Testo").gameObject;
                    TextMeshProUGUI textMesh = textChild.GetComponent<TextMeshProUGUI>();
                    if (textMesh != null)
                    {
                        textMesh.text = assoc.elementType;
                        Debug.Log($"[ADM] Updated text to: {assoc.elementType}");
                    }
                    else
                    {
                        Debug.LogError($"[ADM] TextMeshProUGUI component not found in child: {childName}");
                    }


                }
                else
                {
                    Debug.Log($"[ADM] No association found for: {childName} -> {type}");
                }
            }
            
        }
    }
}
