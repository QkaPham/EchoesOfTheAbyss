using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStars : MonoBehaviour
{
    public void ShowStar(int number)
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (i < number)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
