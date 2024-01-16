using UnityEditor;
using System.IO;
public class LuaMenuItem
{
    [MenuItem("Assets/Create/Lua Script")]
    private static void CreateLuaScript()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
            path = "Assets";

        string filename = "NewLuaScript.lua";

        string filePath = Path.Combine(path, filename);

        File.Create(filePath).Dispose();

        AssetDatabase.Refresh();
    }
}
