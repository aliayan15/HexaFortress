using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public static class GamePropertiesViewer 
{
    private static bool s_enabled;

    private static bool Enabled
    {
        get => s_enabled;
        set
        {
            s_enabled = value;
            EditorPrefs.SetBool("ExposedFieldViewer", value);
        }
    }

    static GamePropertiesViewer()
    {
        s_enabled = EditorPrefs.GetBool("ExposedFieldViewer", false);

        ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
    }


    private static void OnToolbarGUI()
    {
        var tex = EditorGUIUtility.IconContent("d_ScriptableObject Icon").image;

        GUI.changed = false;

        if (GUILayout.Button(new GUIContent(null, tex, "Open up exposed field data"), "Command"))
        {
            var guids = AssetDatabase.FindAssets("Game Propertes", null);
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var exposeGlobalData = (SOGameProperties)AssetDatabase.LoadAssetAtPath(path, typeof(SOGameProperties));
            AssetDatabase.OpenAsset(exposeGlobalData);
            Enabled = !Enabled;
        }
    }
}
