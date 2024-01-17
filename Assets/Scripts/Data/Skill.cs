using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public int CoolDown;
    public int CoolCount;
    public string Script;
    public abstract void DoSkill();
}
