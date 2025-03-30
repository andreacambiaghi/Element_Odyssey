// using UnityEngine;
// using UnityEngine.UI;

// public class InventoryManager : MonoBehaviour
// {

//     [SerializeField] private Button menuButton;

//     private void Awake()
//     {
//         if (menuButton != null)
//             menuButton.onClick.AddListener(SwitchMenuState);
//         else
//             Debug.LogError("Menu button is not assigned in the inspector.");
//     }

//     public void SwitchMenuState()
//     {
//         GameModeManager.Instance.IsMenuOpen = !GameModeManager.Instance.IsMenuOpen;
//         Debug.LogError("Menu state changed to: " + GameModeManager.Instance.IsMenuOpen);
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private Button menuButton;
    private bool isProcessing = false;

    private void Awake()
    {
        if (menuButton != null)
            menuButton.onClick.AddListener(SwitchMenuState);
        else
            Debug.LogError("Menu button is not assigned in the inspector.");
    }

    public void SwitchMenuState()
    {
        if (!isProcessing)
            StartCoroutine(SwitchMenuStateWithDelay());
    }

    private IEnumerator SwitchMenuStateWithDelay()
    {
        isProcessing = true;
        if(GameModeManager.Instance.IsMenuOpen) yield return new WaitForSeconds(0.5f);
        GameModeManager.Instance.IsMenuOpen = !GameModeManager.Instance.IsMenuOpen;
        Debug.LogError("Menu state changed to: " + GameModeManager.Instance.IsMenuOpen);
        isProcessing = false;
    }
}