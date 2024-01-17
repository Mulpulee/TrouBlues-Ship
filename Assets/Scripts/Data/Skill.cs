using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public int CoolDown;
    public int CoolCount;
    public abstract void DoSkill();
}
