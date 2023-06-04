//using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] propPrefabs;
    public float propRate = 0.05f;
    [SerializeField]
    private GameObject borderPrefabs;


    public GameObject obstaclePrefab;
    public float obstacleRate = 0.05f;

    public int width = 10;
    public int height = 10;
    public int exclusionArea = 2;

    public bool isUseSeed;
    public int seed = 12345;

    private void OnEnable()
    {
        GameManager.OnStartGame += GenerateObstacles;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= GenerateObstacles;
    }
    private void GenerateObstacles()
    {
        DestroyAllChild();
        if (isUseSeed) Random.InitState(seed);

        for (int x = -width / 2; x <= width / 2; x += 2)
        {
            for (int y = -height / 2; y <= height / 2; y += 2)
            {
                if (x == -width / 2 || x == width / 2 || y == -height / 2 || y == height / 2)
                {
                    Instantiate(borderPrefabs, new Vector2(x, y) + Random.insideUnitCircle, Quaternion.identity, transform);
                    continue;
                }
                if (x >= -exclusionArea && x <= exclusionArea && y >= -exclusionArea && y <= exclusionArea)
                {
                    continue;
                }
                float rand = Random.value;
                if (rand < propRate)
                {
                    int index = Random.Range(0, propPrefabs.Length - 1);
                    Instantiate(propPrefabs[index], new Vector2(x, y) + Random.insideUnitCircle, Quaternion.identity, transform);
                }

                if (rand >= propRate && rand < propRate + obstacleRate)
                {
                    Instantiate(obstaclePrefab, new Vector2(x, y) + Random.insideUnitCircle, Quaternion.identity, transform);
                }
            }
        }
    }

    private void DestroyAllChild()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}