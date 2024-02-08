using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public static class CreatScriptMenu
{

    [MenuItem("Assets/Create Script/New MonoBehaviour")]
    static void CreateMonoScript()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create Mono Behaviour", GetCurrentPath(), "NewMonoBehaviour.cs", "cs");
        string pathToTemplate = Application.dataPath + "/Scripts/Editor/ScriptMenu/MonoBehaviourTemplate.txt";

        CreatScript(pathToNewFile, pathToTemplate);
    }

    [MenuItem("Assets/Create Script/New Custom Editor")]
    static void CreateCustomEditorScript()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create Custom Editor", GetCurrentPath(), "NewEditor.cs", "cs");
        string pathToTemplate = Application.dataPath + "/Scripts/Editor/ScriptMenu/EditorTemplate.txt";

        CreatScript(pathToNewFile, pathToTemplate);
    }

    [MenuItem("Assets/Create Script/New ScriptableObject")]
    static void CreateScriptableObjectrScript()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create ScriptableObject", GetCurrentPath(), "NewScriptableObject.cs", "cs");
        string pathToTemplate = Application.dataPath + "/Scripts/Editor/ScriptMenu/ScriptableObjectTemplate.txt";

        CreatScript(pathToNewFile, pathToTemplate);
    }

    // Get current folder path
    static string GetCurrentPath()
    {
        string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        if (path.Contains("."))
        {
            int index = path.LastIndexOf("/");
            path = path.Substring(0, index);
        }
        return path;
    }

    static void CreatScript(string pathToNewFile, string pathToTemplate)
    {
        if (!string.IsNullOrWhiteSpace(pathToNewFile))
        {
            FileInfo fileInfo = new FileInfo(pathToNewFile);
            string nameOfScript = Path.GetFileNameWithoutExtension(fileInfo.Name);
            string text = File.ReadAllText(pathToTemplate);
            text = text.Replace("#SCRIPTNAME#", nameOfScript);
            text = text.Replace("#SCRIPTNAMEWITHOUTEDITOR#", nameOfScript.Replace("Editor", ""));
            File.WriteAllText(pathToNewFile, text);
            AssetDatabase.Refresh();
        }
    }
}
