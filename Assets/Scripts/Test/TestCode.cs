using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestCode : MonoBehaviour
{
    public Button button;
    public Image image;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            button.onClick.Invoke();
        }
    }

    public void Func()
    {
        Debug.Log("Click");
        image.color = Color.gray;
    }
}
