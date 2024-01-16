using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JobObject", menuName = "ScriptableObject/JobObject", order = 1)]
public class Job : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public string Description;
    public Skill SpecialSkill;
}
