using UnityEngine;
using UnityEngine.EventSystems;

public class HeaderFilter : MonoBehaviour, IPointerClickHandler
{
    public Transform container;

    private Transform circle;

    private void Awake()
    {
        circle = transform.Find("Circle");

        if (circle != null)
        {
            circle.gameObject.SetActive(false); // disattiva all'avvio
        }
        else
        {
            Debug.LogWarning("Nessun figlio chiamato 'Circle' trovato su " + gameObject.name);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (container == null)
        {
            Debug.LogError("Container non assegnato su " + gameObject.name);
            return;
        }

        bool isCurrentlyActive = circle != null && circle.gameObject.activeSelf;

        // Disattiva tutti i Circle
        foreach (Transform child in container)
        {
            Transform c = child.Find("Circle");
            if (c != null)
            {
                c.gameObject.SetActive(false);
            }
        }

        // Se il Circle non era attivo, lo attiva (e gli altri rimangono disattivati)
        if (circle != null && !isCurrentlyActive)
        {
            circle.gameObject.SetActive(true);
        }
    }
}
