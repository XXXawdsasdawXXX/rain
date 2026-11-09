#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class UpgradeFmodPluginMetas
{
    [MenuItem("FMOD/Upgrade FMOD PluginImporter metas")]
    static void Run()
    {
        var guids = AssetDatabase.FindAssets("", new[] { "Assets/Plugins/FMOD/platforms" });
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var importer = AssetImporter.GetAtPath(path) as PluginImporter;
            if (importer == null) continue;
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Done...  ");
    }
}
#endif