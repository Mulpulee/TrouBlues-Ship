using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum NewsType
{
    Nothing,
    Death,
    JobSkill,
    Scanner,
    Vaccine
}

public struct News
{
    public Sprite Icon;
    public string Target;
    public string Script;
    public NewsType Type;

    public News(Sprite icon, string target, string script, NewsType type)
    {
        Icon = icon;
        Target = target;
        Script = script;
        Type = type;
    }
}

public class BriefingManager
{
    private static BriefingManager m_instance;
    public static BriefingManager Ins
    {
        get
        {
            if (m_instance == null) m_instance = new BriefingManager();
            return m_instance;
        }
    }

    private Dictionary<JobType, News> m_newsList;
    private List<News> m_result;

    private int m_scannerCount;

    public void Init()
    {
        m_newsList = new Dictionary<JobType, News>();
        m_result = new List<News>();
        m_scannerCount = -1;
    }

    public void DeadPlayer(int profileId)
    {
        foreach (var p in CommonData.Players)
        {
            if (p.ProfileID == profileId)
            {
                p.SetDead();
                m_result.Add(new News(p.PlayerProfile, p.Name, "이 살해당했습니다.", NewsType.Death));
                break;
            }
        }
    }

    public void UseSkill(JobType pJob, string pScript)
    {
        m_newsList.Add(pJob, new News(JobManager.GetJob(pJob).Icon,
            $"{JobManager.GetJob(pJob).Name}이(가) 능력을 사용했습니다.", pScript, NewsType.JobSkill));
    }

    public void UseScanner(int profileId)
    {
        foreach (var p in CommonData.Players)
        {
            if (p.ProfileID == profileId)
            {
                m_newsList.Add((JobType)m_scannerCount--,
                    new News(null, p.Name, $"{(p.IsInfected ? "" : "비")}감염자로 확인되었습니다.", NewsType.Scanner));
                break;
            }
        }
    }

    public void UseVaccine(int profileId, bool used)
    {
        foreach (var p in CommonData.Players)
        {
            if (p.ProfileID == profileId)
            {
                m_newsList.Add((JobType)300,
                    new News(p.PlayerProfile, p.Name, used ? "기생충에서 해방되었습니다." : "헛수고였습니다...", NewsType.Vaccine));
                break;
            }
        }
    }

    public void StartBriefing()
    {
        if (m_result.Count == 0) m_result.Add(new News(null, null, null, NewsType.Nothing));

        var newlist = m_newsList.OrderBy(n => n.Key);
        var scanners = new List<News>();

        foreach (var p in newlist)
        {
            if (p.Key < 0) scanners.Add(p.Value);
            else if (p.Key == (JobType)300) m_result.Add(p.Value);
            else m_result.Add(p.Value);
        }

        GameObject.FindObjectOfType<BriefingUI>().StartBriefing(m_result.Concat(scanners).ToList());
    }
}
