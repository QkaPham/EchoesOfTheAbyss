using System.Collections;
using UnityEngine;
using TMPro;
using TMPro.Examples;

public class DamagePopup : MonoBehaviour
{
    public float destroyTime = 1f;
    private TextMeshPro popupText;

    [SerializeField]
    private Color NonCritColor;
    
    [SerializeField]
    private Color CritColor;

    private void Awake()
    {
        popupText = GetComponent<TextMeshPro>();
    }
    void OnEnable()
    {
        Destroy(gameObject, destroyTime);
    }


    public void SetUp(float damageAmount, Vector3 position, bool isCritHit)
    {
        popupText.text = damageAmount.ToString();
        transform.position = position;
        if(isCritHit)
        {
            popupText.color = CritColor;
        }
        else
        {
            popupText.color = NonCritColor;
        }
    }
}
