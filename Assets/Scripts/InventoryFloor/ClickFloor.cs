using UnityEngine;
using UnityEngine.UI;

public class ClickFloor : MonoBehaviour
{
    private Button floorButton;

    void Start()
    {
        floorButton = GetComponent<Button>();

        if (floorButton != null)
        {
            floorButton.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        Debug.Log("Bottone cliccato!");
    }
}
