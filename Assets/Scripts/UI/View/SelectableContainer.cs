using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectableContainer : MonoBehaviour
{
    [SerializeField]
    protected int maxColumn = 1;
    protected Selectable selectable;

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
        Setup();
    }

    protected void Setup()
    {
        List<Selectable> selectables = GetComponentsInChildren<Selectable>().ToList();
        selectables.RemoveAt(0);
        int itemCount = selectables.Count;

        int u, d;
        for (int i = 0; i < itemCount; i++)
        {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;
            u = i - maxColumn;
            d = i + maxColumn;
            if (u >= 0)
            {
                nav.selectOnUp = selectables[u];
            }
            else
            {
                nav.selectOnUp = selectables[itemCount - 1];
            }
            if (d < selectables.Count)
            {
                nav.selectOnDown = selectables[d];
            }
            else
            {
                nav.selectOnDown = selectables[0];
            }
            selectables[i].navigation = nav;
        }

        Navigation newNav = new Navigation();
        newNav.mode = Navigation.Mode.Explicit;
        newNav.selectOnDown = selectables[0];
        newNav.selectOnUp = selectables[itemCount - 1];

        selectable.navigation = newNav;
    }
}
