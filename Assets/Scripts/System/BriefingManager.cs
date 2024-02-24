using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NewsType
{
    Death,
    JobSkill,
    Scanner
}

public struct News
{
    public Sprite Icon;
    public string Script;
    public NewsType Type;
}

public static class BriefingManager
{
    private static Player m_murdered;
    private static List<Job> m_jobSkills;
    private static List<Player> m_scanners;

    public static void Init()
    {
        if (m_jobSkills == null)
        {
            m_jobSkills = new List<Job>();
            m_scanners = new List<Player>();
        }
    }

    public static void DeadPlayer(int profileId)
    {
        foreach (var p in CommonData.Players)
        {
            if (p.ProfileID == profileId)
            {
                m_murdered = p;
                p.SetDead();
                break;
            }
        }
    }

    public static void UseSkill(JobType pJob, string pScript)
    {
        m_jobSkills.Add(JobManager.GetJob(pJob));
    }

    public static void UseScanner(int profileId)
    {
        foreach (var p in CommonData.Players)
        {
            if (p.ProfileID == profileId)
            {
                m_scanners.Add(p);
                break;
            }
        }
    }

    public static void StartBriefing()
    {

    }
}
