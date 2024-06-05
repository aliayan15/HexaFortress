using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IGameEvent
{
    
}
public static class EventManager
{
    static readonly Dictionary<Type, Action<IGameEvent>> s_Events = new Dictionary<Type, Action<IGameEvent>>();

    static readonly Dictionary<Delegate, Action<IGameEvent>> s_EventLookups =
        new Dictionary<Delegate, Action<IGameEvent>>();

    /// <summary>
    /// Add event listener.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="evt"></param>
    public static void AddListener<T>(Action<T> evt) where T : IGameEvent
    {
        if (!s_EventLookups.ContainsKey(evt))
        {
            Action<IGameEvent> newAction = (e) => evt((T)e);
            s_EventLookups[evt] = newAction;

            if (s_Events.TryGetValue(typeof(T), out Action<IGameEvent> internalAction))
                s_Events[typeof(T)] = internalAction += newAction;
            else
                s_Events[typeof(T)] = newAction;
        }
    }
    /// <summary>
    /// Remove event listener.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="evt"></param>
    public static void RemoveListener<T>(Action<T> evt) where T : IGameEvent
    {
        if (s_EventLookups.TryGetValue(evt, out var action))
        {
            if (s_Events.TryGetValue(typeof(T), out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                    s_Events.Remove(typeof(T));
                else
                    s_Events[typeof(T)] = tempAction;
            }

            s_EventLookups.Remove(evt);
        }
    }
    /// <summary>
    /// Invoke an event.
    /// </summary>
    /// <param name="evt"></param>
    public static void Broadcast(IGameEvent evt)
    {
        if (s_Events.TryGetValue(evt.GetType(), out var action))
            action.Invoke(evt);
    }
    /// <summary>
    /// Clear events.
    /// </summary>
    public static void Clear()
    {
        s_Events.Clear();
        s_EventLookups.Clear();
    }

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
    static void Reset()
    {
        Clear();
    }
#endif
}

