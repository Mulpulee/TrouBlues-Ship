using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;
using UnityEditor.Compilation;

[ScriptedImporter(1,"lua")]
public class LuaImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var asset = new TextAsset(File.ReadAllText(ctx.assetPath));
        ctx.AddObjectToAsset("Text", asset);
        ctx.SetMainObject(asset);
    }
}
