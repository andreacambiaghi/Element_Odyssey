using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCoin : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI coinText;
    void Update()
    {
        coinText.text = ElementFilesManager.Instance.GetBalance().ToString();
    }
}
