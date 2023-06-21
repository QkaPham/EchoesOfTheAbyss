using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<EventID, Action<Notify>> eventDictionary;
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (instance == null)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    instance.Init();
                }
            }
            return instance;
        }
    }

    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EventID, Action<Notify>>();
        }
    }

    public void AddListener(EventID eventID, Action<Notify> action)
    {
        if (eventDictionary.ContainsKey(eventID))
        {
            eventDictionary[eventID] += action;
        }
        else
        {
            eventDictionary.Add(eventID, action);
        }
    }

    public void RemoveListener(EventID eventID, Action<Notify> action)
    {
        if (eventDictionary == null) return;

        if (eventDictionary.ContainsKey(eventID))
        {
            eventDictionary[eventID] -= action;
        }
    }

    public void Raise(EventID eventID, Notify notify)
    {
        if (eventDictionary.TryGetValue(eventID, out var action))
        {
            action?.Invoke(notify);
        }
    }
}
