using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownTemplateBehavior : MonoBehaviour
{
    public DropDownBehavior DropDownBehavior;

    private void OnDestroy()
    {
        DropDownBehavior.isDroping = false;
    }
}
