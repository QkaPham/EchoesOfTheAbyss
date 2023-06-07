using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public interface PoolableObject<T> where T : MonoBehaviour
{
    void SetPool(ObjectPool<T> pool);
    void Realease(float delay = 0f);
}