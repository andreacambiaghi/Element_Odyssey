using System.Collections;
using TMPro;
using UnityEngine;

public class DeleteSavesButton : MonoBehaviour
{
    [SerializeField] private GameObject deleteSavesButton;
    private string originalText;
    public void DeleteSaves()
    {
        ElementFilesManager.Instance.ResetFoundElements();
    }

    public void ChangeNameButton()
    {
        TextMeshProUGUI childText = GetComponentInChildren<TextMeshProUGUI>();

        if (originalText == null)
        {
            originalText = childText.text;
        }

        childText.text = "Deleted!";
        StartCoroutine(RestoreTextAfterDelay(childText, 1f));
    }

    private IEnumerator RestoreTextAfterDelay(TextMeshProUGUI textMesh, float delay)
    {
        yield return new WaitForSeconds(delay);
        textMesh.text = originalText;
    }

    public void DeleteAchievements()
    {
        Debug.Log("Mariooo --1");
        ElementFilesManager.Instance.ResetAchievements();
    }
}
