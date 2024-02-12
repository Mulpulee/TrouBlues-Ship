using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NewsType
{
    Death,
    JobSkill,
    Scanner
}

public class News
{
    public Sprite Icon;
    public NewsType Type;
}

public class MorningBriefing : MonoBehaviour
{
    private Player m_murdered;
    private List<Job> m_jobSkills;
    private List<Player> m_scanners;
}
