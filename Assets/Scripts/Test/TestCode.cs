using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCode : MonoBehaviour
{
    [SerializeField]
    public Slider slider;

    private void Start()
    {
        slider.maxValue = 100;
        slider.value = 100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            slider.value -= 10;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            slider.value += 10;
        }
    }
}
