#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ProjectSettingsManager
{
    private const string CanInstantiateSystemBoost = "MyBooleanProperty";

    public static bool MyCanInstantiateSystemBoost
    {
        get { return EditorPrefs.GetBool(CanInstantiateSystemBoost, false); }
        set { EditorPrefs.SetBool(CanInstantiateSystemBoost, value); }
    }


    [MenuItem("My Section/Set Can Instantiate SystemBoost", priority = 1)]
    private static void SetCanInstantiateSystemBoost()
    {
        MyCanInstantiateSystemBoost = !MyCanInstantiateSystemBoost;
        Debug.Log("Can Instantiate SystemBoost: " + MyCanInstantiateSystemBoost);
    }
}
#endif