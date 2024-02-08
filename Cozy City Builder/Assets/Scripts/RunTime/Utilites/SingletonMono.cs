using UnityEngine;

namespace MyUtilities
{

    public class SingletonMono<T> : MonoBehaviour where T : class
    {
        public T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null) Instance = this as T;
            else Debug.LogError($"More that one instance of {typeof(T).Name} found.", this);
        }
    }

}
