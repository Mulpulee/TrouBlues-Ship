using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class JobManager
{
    private static Dictionary<JobType, Job> m_jobs;
    public static Job GetJob(JobType type) { return m_jobs[type]; }

    public static void SetJobs()
    {
        Job[] objects = Resources.LoadAll<Job>("ScriptableObject/Job");

        m_jobs = new Dictionary<JobType, Job>();
        foreach (Job obj in objects)
        {
            m_jobs.Add(obj.Type, obj);
        }

        m_jobs[JobType.Captain].SpecialSkill = new CaptainSkill();
        m_jobs[JobType.Engineer].SpecialSkill = new EngineerSkill();
        m_jobs[JobType.Medic].SpecialSkill = new MedicSkill();
        m_jobs[JobType.Janitor].SpecialSkill = new JanitorSkill();
        m_jobs[JobType.Controller].SpecialSkill = new ControllerSkill();
    }
}

public class CaptainSkill : Skill
{
    public bool CanUseSkill = true;
    public Player Searched;

    public CaptainSkill()
    {
        CoolDown = 0;
        //Script = $"{Searched.Name}의 직업은  {Searched.PlayerJob.name}으로 밝혀졌습니다!";
    }

    public override void DoSkill()
    {
        if (Searched.PlayerJob.Icon != null)
        {
            Searched.PlayerJob.SpecialSkill.DoSkill();
        }
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
        Script = "우주선 수리 진척도가 일정 수준 올라갑니다.";
    }

    public override void DoSkill()
    {
        for (int i = 0; i < 3; i++)
            CommonData.RepairProgress[i] += DataManager.Data.EngineerSkillLevel[CommonData.Players.Count - 4][i];
        // 우주선 수리 진척도 올림(수치받아와야함ㅜㅜ)
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
    public Item Component;

    public JanitorSkill()
    {
        CoolDown = 1;
        CoolCount = 0;
        //Script = $"{Component.name}을(를) 관리인이 획득했습니다.";
        Script = $"관리인이 고철, 나사, 전선 각 2개를 획득했습니다.";
    }

    public override void DoSkill()
    {
        Player.This.AddItem(ItemIndex.Scrap, 2);
        Player.This.AddItem(ItemIndex.Bolt, 2);
        Player.This.AddItem(ItemIndex.Wire, 2);
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
        Script = EarthCommunication.ins.Together();
    }
}
