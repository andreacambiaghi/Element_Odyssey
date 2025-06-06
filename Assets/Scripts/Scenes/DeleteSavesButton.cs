using System.Collections;
using TMPro;
using UnityEngine;

public class DeleteSavesButton : MonoBehaviour
{
    private string originalText;
    public void DeleteSaves()
    {
        ElementFilesManager.Instance.ResetFoundElements();
        ElementFilesManager.Instance.ResetVillageData();
        ElementFilesManager.Instance.ResetVillageObjects();
        ElementFilesManager.Instance.ResetBuyFloor();
        ElementFilesManager.Instance.ResetAchievements();
        ElementFilesManager.Instance.ResetBalance();
        ElementFilesManager.Instance.DeleteTutorialFile();
        ElementFilesManager.Instance.DeleteTutorialVillageFile();
        // ElementFilesManager.Instance.DeleteMakerAssociations();

        Debug.Log("Saves deleted!");
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
}