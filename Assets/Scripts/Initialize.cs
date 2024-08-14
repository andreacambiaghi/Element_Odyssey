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
        LoadLabelsFromFile("InitialElements");
        LoadLabelsFromFile("Founds");

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

    private void LoadLabelsFromFile(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            labelsToAdd.AddRange(lines);
        }
        else
        {
            Debug.LogError($"File '{fileName}' non trovato");
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
