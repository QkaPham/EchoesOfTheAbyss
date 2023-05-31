using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamagePopup : MonoBehaviour
{
    public GameObject DamagePopupPrefabs;

    public float spawnTime;
    public float elapsedSpawnTime;
    private void Update()
    {
        elapsedSpawnTime += Time.deltaTime;
        if(elapsedSpawnTime >= spawnTime)
        {
            elapsedSpawnTime = 0;
            GameObject g= Instantiate(DamagePopupPrefabs, transform);
            g.SetActive(true);
        }
    }
}
