using UnityEngine;

public class Test : MonoBehaviour
{
    public void Testare()
    {
        GameObject achievementObject = FindGameObjectWithUniqueID(5);

        if (achievementObject != null)
        {
            ProgressBar progressBar = achievementObject.GetComponent<ProgressBar>();

            if (progressBar != null)
            {
                progressBar.SetProgress(50);
            }
            else
            {
                Debug.Log("Il componente ProgressBar non è stato trovato sul GameObject con l'ID univoco 5.");
            }
        }
        else
        {
            Debug.Log("Nessun GameObject trovato con l'ID univoco 5.");
        }
    }

    GameObject FindGameObjectWithUniqueID(int uniqueID)
    {
        GameObject[] achievementObjects = GameObject.FindGameObjectsWithTag("Achievement");
        Debug.Log("achievementObjects.Length: " + achievementObjects.Length);
        foreach (GameObject obj in achievementObjects)
        {
            AchievementIdentifier identifier = obj.GetComponent<AchievementIdentifier>();
            if (identifier != null && identifier.uniqueID == uniqueID)
            {
                return obj;
            }
        }
        return null;
    }
}
