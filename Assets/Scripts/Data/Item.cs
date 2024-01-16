using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemIndex
{
    Scrap,
    Bolt,
    Wire,
    SuperConductor,
    Scanner
}

[CreateAssetMenu(fileName = "ItemObject", menuName = "ScriptableObject/ItemObject", order = 1)]
public class Item : ScriptableObject
{
    public ItemIndex Index;
    public string Name;
    public Sprite Icon;
}
