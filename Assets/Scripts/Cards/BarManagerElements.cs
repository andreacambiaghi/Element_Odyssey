using UnityEngine;

public class BarManagerElements : MonoBehaviour
{
    [SerializeField] private GameObject under; // Il GameObject "Under"
    [SerializeField] private GameObject container; // Il GameObject "Container" che contiene le pagine

    private GameObject myCircle;
    private GameObject myPage;

    void Start()
    {
        // Trova il proprio Circle, se esiste
        Transform circleTransform = transform.Find("Circle");
        if (circleTransform != null)
        {
            myCircle = circleTransform.gameObject;
        }

        // Trova la propria pagina dentro il Container, se esiste
        if (container != null)
        {
            string pageName = gameObject.name + "Page"; // Es: "Water" → "WaterPage"
            Transform pageTransform = container.transform.Find(pageName);
            if (pageTransform != null)
            {
                myPage = pageTransform.gameObject;
            }
        }
    }

    public void OnClick()
    {
        if (myCircle == null) return; // Se non ha un Circle, esce

        // Disattiva tutti i Circle dentro "Under"
        foreach (Transform element in under.transform)
        {
            Transform circleTransform = element.Find("Circle");
            if (circleTransform != null)
            {
                circleTransform.gameObject.SetActive(false);
            }
        }

        // Attiva solo il proprio Circle
        myCircle.SetActive(true);

        // Se il Container esiste, disattiva tutte le pagine
        if (container != null)
        {
            foreach (Transform page in container.transform)
            {
                page.gameObject.SetActive(false);
            }
        }

        // Attiva solo la propria pagina
        if (myPage != null)
        {
            myPage.SetActive(true);
        }
    }
}
