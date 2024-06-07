using System.Collections.Generic;
using UnityEngine;

namespace MyUtilities.EventChannel
{
    public abstract class EventChannel<T>:ScriptableObject
    {
        private readonly HashSet<EventListener<T>> observers = new();

        public void Invoke(T value)
        {
            foreach (var observer in observers)
            {
                observer.Raise(value);
            }
        }

        public void Register(EventListener<T> observer) => observers.Add(observer);
        public void DeRegister(EventListener<T> observer) => observers.Remove(observer);
    }
}