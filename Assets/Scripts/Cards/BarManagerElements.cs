using UnityEngine;

public class BarManagerElements : MonoBehaviour
{
    [SerializeField] private GameObject under; // Il GameObject "Under"
    
    private GameObject myCircle;

    void Start()
    {
        // Trova il proprio Circle, se esiste
        Transform circleTransform = transform.Find("Circle");
        if (circleTransform != null)
        {
            myCircle = circleTransform.gameObject;
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
    }
}
