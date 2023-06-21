using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Singleton<T> : SerializedMonoBehaviour
    where T : Singleton<T>
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError($"There needs to be one active {typeof(T)} script on a GameObject in your scene.");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && gameObject != null)
        {
            Debug.LogError($"There are some active {typeof(T)} script on a GameObject in your scene. Destroy!");
            Destroy(gameObject);
        }
        else
        {
            instance = this as T;
        }
    }
}