using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour
{
    public Image shortcutImage;
    public TextMeshProUGUI shortcutText;
    public TextMeshProUGUI buttonName;
    public TextMeshProUGUI cost;
    public Image fragmentImage;
    public Button button;

    public void MaxLevel()
    {
        buttonName.text = "Max Level";
        cost.gameObject.SetActive(false);
        fragmentImage.gameObject.SetActive(false);
    }
    public void SetCost(int cost)
    {
        SetCost(cost.ToString());
    }
    public void SetCost(string cost)
    {
        this.cost.text = cost;
    }
    public void Enable()
    {
        button.interactable = true;
    }
    public void Disable()
    {
        button.interactable = false;
    }

    public void onclick()
    {
        Debug.Log("Click");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetCost(10);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetCost(99);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Disable();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Enable();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            MaxLevel();
        }
    }
}
