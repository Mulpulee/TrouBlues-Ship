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
        Script = $"{Searched.Name}�� ������  {Searched.GetJob().name}���� ���������ϴ�!";
    }

    public override void DoSkill()
    {
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
        GameManagerEx.Player.AddItem(ItemIndex.Scrap, 2);
        GameManagerEx.Player.AddItem(ItemIndex.Bolt, 2);
        GameManagerEx.Player.AddItem(ItemIndex.Wire, 2);
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
        Script = GameManagerEx.EarthCommu.Together();
    }
}
