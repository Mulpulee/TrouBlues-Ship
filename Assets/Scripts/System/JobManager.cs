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
    public Player Searched;

    public CaptainSkill()
    {
        CoolDown = 0;
        Script = $"{Searched.Name}�� ������  {Searched.Job.name}���� ���������ϴ�!";
    }

    public override void DoSkill()
    {
        if (Searched.Job.Icon != null)
        {
            Searched.Job.SpecialSkill.DoSkill();
        }
        // ������ �÷��̾��� ���� ����, Ư���ɷ��� ���
        // ����� ���� ��Ÿ���
    }
}

public class EngineerSkill : Skill
{
    public EngineerSkill()
    {
        CoolDown = 2;
        CoolCount = 0;
        Script = "���ּ� ���� ��ô���� ���� ���� �ö󰩴ϴ�.";
    }

    public override void DoSkill()
    {
        for (int i = 0; i < 3; i++)
            CommonData.RepairProgress[i] += DataManager.Data.EngineerSkillLevel[CommonData.Players.Count - 4][i];
        // ���ּ� ���� ��ô�� �ø�(��ġ�޾ƿ;��Ԥ̤�)
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
        // ��ĳ�ʻ��
        // �����ۻ���̶� ������ �Ϸ翡 �ι��� ����
    }
}

public class JanitorSkill : Skill
{
    public Item Component;

    public JanitorSkill()
    {
        CoolDown = 1;
        CoolCount = 0;
        //Script = $"{Component.name}��(��) �������� ȹ���߽��ϴ�.";
        Script = $"�������� ��ö, ����, ���� �� 2���� ȹ���߽��ϴ�.";
    }

    public override void DoSkill()
    {
        Player.This.AddItem(ItemIndex.Scrap, 2);
        Player.This.AddItem(ItemIndex.Bolt, 2);
        Player.This.AddItem(ItemIndex.Wire, 2);
        // ���ϴ� �ڿ� ���޹���
        // ���� ���� �� ��ö/����/���� 4��, ������ü 2��, ��ĳ�� 1�� ����
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
