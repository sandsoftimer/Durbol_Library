#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class DurbolToolsMenuItems
{
    [MenuItem("Durbol_Tools/Delete Gameplay Data")]
    public static void DeleteBinaryData()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Durbol_Tools/Generate Conflict")]
    public static void GenerateConflict()
    {
        string path = "ConflictForGreaterGood.txt";
        string data = string.Format("GitConflictGeneration: {0}\n{1} - Device Name: {2}\n{3}\n{4}", System.DateTime.Now, System.DateTime.Now.Ticks, SystemInfo.deviceName, SystemInfo.deviceModel, SystemInfo.deviceUniqueIdentifier);
        StreamWriter sw = File.CreateText(path);
        sw.WriteLine(data);
        sw.Close();
        Debug.LogErrorFormat(data);
    }

    public static void CreateScriptableObject(string path, Type obj)
    {
        ScriptableObject so = ScriptableObject.CreateInstance(obj);
        Selection.activeObject = CreateIfDoesntExists(path, so);
    }

    public static Object CreateIfDoesntExists(string path, Object o)
    {
        var ap = AssetDatabase.LoadAssetAtPath(path, o.GetType());
        if (ap == null)
        {
            AssetDatabase.CreateAsset(o, path);
            ap = AssetDatabase.LoadAssetAtPath(path, o.GetType());
            AssetDatabase.Refresh();
            return ap;
        }
        return ap;
    }
}
#endif
