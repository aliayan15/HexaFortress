#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


public static class Bootstrapper
{


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        if (ProjectSettingsManager.MyCanInstantiateSystemBoost)
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("SystemBoost")));
    }
}
#endif

