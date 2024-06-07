using System;
using UnityEngine;

namespace MyUtilities.EventChannel
{
    public class EventListener<T>:MonoBehaviour
    {
        [SerializeField] private EventChannel<T> eventChannel;
        public event Action<T> Event; 
        public void Raise(T value)
        {
            Event?.Invoke(value);
        }

        private void Awake()
        {
            eventChannel.Register(this);
        }

        private void OnDestroy()
        {
            eventChannel.DeRegister(this);
        }
    }
}