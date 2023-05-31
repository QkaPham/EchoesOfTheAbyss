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

    private float spawnTime;

    [SerializeField]
    private float accelerate;
    [SerializeField]
    private float Vy0;
    [SerializeField]
    private float Vx0;

    private float x0;
    private float y0;
    private void Awake()
    {
        popupText = GetComponent<TextMeshPro>();
    }
    void OnEnable()
    {
        Vx0 = Random.Range(-1f, 1f) * Vx0;
        spawnTime = Time.time;
        Destroy(gameObject, destroyTime);
    }


    public void SetUp(float damageAmount, Vector3 position, bool isCritHit)
    {
        popupText.text = damageAmount.ToString();
        transform.position = position;
        x0 = position.x;
        y0 = position.y;
        if (isCritHit)
        {
            popupText.color = CritColor;
        }
        else
        {
            popupText.color = NonCritColor;
        }
    }

    private void Update()
    {
        float Vy = Vy0 + accelerate * (Time.time - spawnTime);
        float PositionY = (Time.time - spawnTime) * Vy;
        float PositonX = (Time.time - spawnTime) * Vx0;

        transform.localPosition = new Vector3(x0 + PositonX, y0 + PositionY, 0f);
    }
}
