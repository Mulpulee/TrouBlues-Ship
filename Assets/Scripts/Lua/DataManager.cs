using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public interface Data
{
    int[] SpyPerPlayer { get; }
    [CSharpCallLua] List<int[]> ShipRequirements { get; }
    [CSharpCallLua] List<int[]> AIRequirements { get; }
    int[] ProvidedItems { get; }
    int[] SelectableItems { get; }
    int[] FailureProbability { get; }
    [CSharpCallLua] List<int[]> SearchedItemPercentage { get; }
}

public static class DataManager
{
    private static Data m_data;
    public static Data Data
    {
        get
        {
            if (m_data == null)
            {
                TextAsset text = Resources.Load<TextAsset>("LuaTexts/Data");
                LuaEnv luaEnv = new LuaEnv();
                luaEnv.DoString(text.text);
                m_data = luaEnv.Global.Get<Data>("Data");
            }
            return m_data;
        }
    }
}
