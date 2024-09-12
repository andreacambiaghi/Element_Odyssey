using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameObject go = Instantiate(Create3DText.Instance.GenerateText("Hello World!"), Vector3.zero, Quaternion.identity);
        Create3DText.Instance.GenerateText("LorenzoPuzzi");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
