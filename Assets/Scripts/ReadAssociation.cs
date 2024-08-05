using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadCSV : MonoBehaviour
{

    public Dictionary<ElementPair, string> elementAssociations;
    public static ReadCSV Instance;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
           Destroy(this.gameObject);
           return;
        }
        else
        {
            Instance = this;
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadElementAssociations();
        foreach(KeyValuePair<ElementPair, string> entry in elementAssociations)
        {
            Debug.Log("PairFound: " + entry.Key.ToString() + " -> " + entry.Value);
        }
    }

    private void LoadElementAssociations()
    {
        elementAssociations = new Dictionary<ElementPair, string>();

        TextAsset csvFile = Resources.Load<TextAsset>("fusions");
        if (csvFile == null)
        {
            Debug.LogError("CSV file not found.");
            return;
        }

        using (StringReader reader = new StringReader(csvFile.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(';');
                if (parts.Length == 3)
                {
                    string element1 = parts[0].Trim();
                    string element2 = parts[1].Trim();
                    string result = parts[2].Trim();

                    ElementPair pair = new ElementPair(element1, element2);
                    elementAssociations[pair] = result;
                }
                else
                {
                    Debug.LogWarning("Invalid CSV line format: " + line);
                }
            }
        }
    }

    public string GetResultForElements(string element1, string element2)
    {
        ElementPair pair = new ElementPair(element1, element2);
        if (elementAssociations.TryGetValue(pair, out string result))
        {
            return result;
        }
        return null;
    }
}
