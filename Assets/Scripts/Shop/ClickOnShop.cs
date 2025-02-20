using UnityEngine;

public class ClickOnShop : MonoBehaviour
{
    public void Click() {
        // Prende il figlio Balance e a suo volta prende il figlio Balance-txt e ci scrive
        GameObject.Find("Shop/Balance/Balance-txt").GetComponent<TMPro.TextMeshProUGUI>().text = ElementFilesManager.Instance.GetBalance().ToString();
    }
}
