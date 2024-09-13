using UnityEngine;

public class Create3DText : MonoBehaviour
{
    public static Create3DText Instance { get; private set; }

    private float spacing = .5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject CreateTextObject(string text)
    {
        GameObject textContainer = new GameObject("TextContainer");

        Vector3 startPosition = Vector3.zero;

        for (int i = 0; i < text.Length; i++)
        {
            char character = text[i];
            GameObject letterInstance = CreateLetterInstance(character);

            if (letterInstance != null)
            {
                letterInstance.transform.position = startPosition + new Vector3(i * spacing, 0, 0);
                Debug.Log(letterInstance.transform.position);
                letterInstance.transform.rotation = Quaternion.Euler(-90, 180, 0);
                letterInstance.transform.SetParent(textContainer.transform);
            }
        }

        return textContainer;
    }

    private GameObject CreateLetterInstance(char character)
    {
        string prefabName = character.ToString().ToUpper();
        GameObject prefab = Resources.Load<GameObject>($"Alphabet/{prefabName}");
        return prefab != null ? Instantiate(prefab) : null;
    }
}
