using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCode : MonoBehaviour
{
    public CollectibleItem colect;
    public ItemProfile profile;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var c = Instantiate(colect);
            c.item = new Item(profile, 1);
        }
    }
}
