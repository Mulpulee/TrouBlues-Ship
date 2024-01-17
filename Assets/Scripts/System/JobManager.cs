using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobManager
{
    private Dictionary<string, Job> m_jobs;

    public void SetJobs()
    {
        Job[] objects = Resources.LoadAll<Job>("ScriptableObject/Job");

        m_jobs = new Dictionary<string, Job>();
        foreach (Job obj in objects)
        {
            m_jobs.Add(obj.Name, obj);
        }
    }
}

public class CaptainSkill : Skill
{
    public bool CanUseSkill = true;

    public CaptainSkill()
    {
        CoolDown = 0;
    }

    public override void DoSkill()
    {
        // 지목한 플레이어의 직업 열람, 특수능력을 사용
        // 개백수 고르면 안타까움
    }
}

public class EngineerSkill : Skill
{
    public EngineerSkill()
    {
        CoolDown = 2;
        CoolCount = 0;
    }

    public override void DoSkill()
    {
        // 우주선 수리 진척도 전체의 10%를 올림
    }
}

public class MedicSkill : Skill
{
    public MedicSkill()
    {
        CoolDown = 2;
        CoolCount = 0;
    }

    public override void DoSkill()
    {
        // 스캐너사용
        // 아이템사용이랑 별개라서 하루에 두번도 ㄱㄴ
    }
}

public class JanitorSkill : Skill
{
    public JanitorSkill()
    {
        CoolDown = 1;
        CoolCount = 0;
    }

    public override void DoSkill()
    {
        // 원하는 자원 지급받음
        // 각각 선택 시 고철/나사/전선 4개, 초전도체 2개, 스캐너 1개 지급
    }
}

public class ControllerSkill : Skill
{
    public ControllerSkill()
    {
        CoolDown = 2;
        CoolCount = 0;
    }

    public override void DoSkill()
    {
        // 2인이상 지구통신 결과를 받음
    }
}
