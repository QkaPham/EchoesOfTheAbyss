using System.Collections;
using UnityEngine;
using TMPro;
using TMPro.Examples;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro popupText;

    [SerializeField]
    private Color NonCritColor;
    [SerializeField]
    private float NonCritscale;

    [SerializeField]
    private Color CritColor;
    [SerializeField]
    private float Critscale;

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
    }


    public void SetUp(float damageAmount, Vector3 position, bool isCritHit)
    {
        popupText.text = ((int)damageAmount).ToString();
        transform.position = position;
        x0 = position.x;
        y0 = position.y;

        float scale;
        if (isCritHit)
        {
            popupText.color = CritColor;
            scale = Critscale;
        }
        else
        {
            popupText.color = NonCritColor;
            scale = NonCritscale;
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(scale, .3f));
        seq.Join(popupText.DOFade(1, .3f));
        seq.Append(transform.DOScale(0, .6f));
        seq.Join(popupText.DOFade(0, .6f));
        seq.OnComplete(()=> Destroy(gameObject));
    }

    private void Update()
    {
        float Vy = Vy0 + accelerate * (Time.time - spawnTime);
        float PositionY = (Time.time - spawnTime) * Vy;
        float PositonX = (Time.time - spawnTime) * Vx0;
        transform.localPosition = new Vector3(x0 + PositonX, y0 + PositionY, 0f);
    }
}
