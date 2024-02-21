using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JobType
{
    None,
    Captain,
    Engineer,
    Medic,
    Janitor,
    Controller
}

[CreateAssetMenu(fileName = "JobObject", menuName = "ScriptableObject/JobObject", order = 1)]
public class Job : ScriptableObject
{
    public JobType Type;
    public Sprite Icon;
    public string Name;
    public string Description;
    public Skill SpecialSkill;
}
