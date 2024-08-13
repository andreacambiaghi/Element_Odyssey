using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Initialize : MonoBehaviour
{
    [SerializeField] private GameObject createButton;
    private List<string> labelsToAdd = new List<string>();


    // Start is called before the first frame update
    void Awake()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("InitialElements");
        if (textAsset != null)
        {
            // Dividi il contenuto del file in righe
            string[] lines = textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            labelsToAdd.AddRange(lines);
        }
        else
        {
            Debug.LogError("File non trovato");
        }

        CreateButtons createButtonsComponent = createButton.GetComponent<CreateButtons>();
        if (createButtonsComponent != null)
        {
            createButtonsComponent.buttonLabels.AddRange(labelsToAdd);
            Debug.Log("ButtonLabels aggiornato con successo");
        }
        else
        {
            Debug.LogError("Il GameObject target non ha il componente 'CreateButtons'");
        }
    }


    // static void EnableReadWrite()
    // {
    //     string[] guids = AssetDatabase.FindAssets("t:Mesh", new[] { "Assets/YourMeshFolder" });
    //     foreach (string guid in guids)
    //     {
    //         string path = AssetDatabase.GUIDToAssetPath(guid);
    //         ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
    //         if (importer != null)
    //         {
    //             importer.isReadable = true;
    //             AssetDatabase.ImportAsset(path);
    //         }
    //     }
    //     Debug.Log("Read/Write Enabled on all meshes in the specified folder.");
    // }

}
