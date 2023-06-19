using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<EventID, Action<Notify>> eventDictionary;
    private static EventManager eventManager;
    public static EventManager instance
    {
        get
        {
            if (eventManager == null)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (eventManager == null)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EventID, Action<Notify>>();
        }
    }

    public static void AddListener(EventID eventID, Action<Notify> action)
    {
        if (instance.eventDictionary.ContainsKey(eventID))
        {
            instance.eventDictionary[eventID] += action;
        }
        else
        {
            instance.eventDictionary.Add(eventID, action);
        }
    }

    public static void RemoveListener(EventID eventID, Action<Notify> action)
    {
        if (instance.eventDictionary == null) return;

        if (instance.eventDictionary.ContainsKey(eventID))
        {
            instance.eventDictionary[eventID] -= action;
        }
    }

    public static void Raise(EventID eventID, Notify notify)
    {
        if (instance.eventDictionary.TryGetValue(eventID, out var action))
        {
            action?.Invoke(notify);
        }
    }
}
