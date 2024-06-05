using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

[InitializeOnLoad]
public static class SceneSelectViewer
{
    private static bool s_enabled;

    private static bool Enabled
    {
        get => s_enabled;
        set
        {
            s_enabled = value;
            EditorPrefs.SetBool("SceneSelectViewer", value);
        }
    }

    static SceneSelectViewer()
    {
        s_enabled = EditorPrefs.GetBool("SceneSelectViewer", false);
        
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    private static GenericMenu GetSceneMenu()
    {
        var menu = new GenericMenu();
        
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var path = SceneUtility.GetScenePathByBuildIndex(i);
            var scene = System.IO.Path.GetFileNameWithoutExtension(path);
            menu.AddItem(new GUIContent(scene), false, () =>
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(path);
                }
            });
        }

        return menu;
    }

    private static void OnToolbarGUI()
    {
        var tex = EditorGUIUtility.IconContent("d_SceneAsset Icon").image;

        GUI.changed = false;
        
        GUILayout.FlexibleSpace();

        if (!EditorApplication.isPlaying)
        {
            if (GUILayout.Button(new GUIContent(null, tex, "Open up selected scene from build settings"), "Command"))
            {
                GetSceneMenu().ShowAsContext();
                Enabled = !Enabled;
            }
        
            
        }
    }
}
