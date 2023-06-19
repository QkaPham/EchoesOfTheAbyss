using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCode : MonoBehaviour
{

    public string hexColor = "0x00FFFFFF";
    public Color color = Color.white; // Default color

    private void Awake()
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            color = newColor;
        }
    }
}
