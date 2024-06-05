using UnityEngine;

namespace MyUtilities
{

    public class SingletonMono<T> : MonoBehaviour where T : class
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null) Instance = this as T;
            else
            {
                Debug.LogWarning($"More that one instance of {typeof(T).Name} found.", this);
                Destroy(this);
            }
        }
    }

}
