// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// //remember to add colliders to the objects you want to interact with
// public class ArTouchManager : MonoBehaviour
// {   
//     private Color _defaultColor;
//     public Color selectedColor;
//     private Material mat;
//     // Start is called before the first frame update
//     void Start()
//     {
//         mat = GetComponent<Renderer>().material;
//         _defaultColor = mat.color;
//         Debug.Log(gameObject.name +" Default color: " + _defaultColor);
//     }

//     void onTouchDown()
//     {
//         mat.color = selectedColor;
//         Debug.Log("Touched " + gameObject.name);
//     }

//     void onTouchUp()
//     {
//         mat.color = _defaultColor;
//     }

//     void onTouchStay()
//     {
//         mat.color = selectedColor;
//     }

//     void onTouchExit()
//     {
//         mat.color = _defaultColor;
//     }
// }


using UnityEngine;

public class ChangeColorOnTouch : MonoBehaviour
{
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        ChangeColor();
                    }
                }
            }
        }
    }

    void ChangeColor()
    {
        objectRenderer.material.color = new Color(Random.value, Random.value, Random.value);
    }
}
