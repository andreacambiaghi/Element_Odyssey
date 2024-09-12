using UnityEngine;

public class Create3DText : MonoBehaviour
{
    public static Create3DText Instance { get; private set; }

    [SerializeField] private float spacing = .5f;

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

    public GameObject GenerateText(string text)
    {
        GameObject textContainer = new GameObject("TextContainer");
        textContainer.transform.SetParent(transform);

        Vector3 startPosition = Vector3.zero;

        for (int i = 0; i < text.Length; i++)
        {
            char character = text[i];
            GameObject letterPrefab = LoadLetterPrefab(character);

            if (letterPrefab != null)
            {
                GameObject letterInstance = Instantiate(letterPrefab, startPosition + new Vector3(i * spacing, 0, 0), Quaternion.Euler(-90, -180, 0), textContainer.transform);
            }
        }

        return textContainer;
    }

    private GameObject LoadLetterPrefab(char character)
    {
        string prefabName = character.ToString().ToUpper();
        return Resources.Load<GameObject>($"Alphabet/{prefabName}");
    }
}
