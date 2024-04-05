using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public int CoolDown;
    public int CoolCount;
    public string Explanation;
    public Player Target;
    public abstract string GetResult();
}


public class CaptainSkill : Skill
{
    public CaptainSkill()
    {
        CoolDown = -10;
        CoolCount = 0;
        Explanation = "������ �÷��̾��� ������ Ȯ���ϰ�, �� �ɷ��� ����մϴ�.";
        //Script = $"{Searched.Name}�� ������  {Searched.PlayerJob.name}���� ���������ϴ�!";
    }

    public override string GetResult()
    {
        return $"{Target.Name}�� ������  {Target.PlayerJob.Name}���� ���������ϴ�!";
    }
}

public class EngineerSkill : Skill
{
    public EngineerSkill()
    {
        CoolDown = 2;
        CoolCount = 0;
        Explanation = "���ּ� ���� ��ô���� ���� ���� �ö󰩴ϴ�.";
    }

    public override string GetResult()
    {
        PVHandler.pv.RPC("AddProgress", Photon.Pun.RpcTarget.MasterClient, DataManager.Data.EngineerSkillLevel[CommonData.Players.Count - 4]);
        // ���ּ� ���� ��ô�� �ø�

        return "���ּ� ���� ��ô���� ���� ���� �ö󰬽��ϴ�.";
    }
}

public class MedicSkill : Skill
{
    public MedicSkill()
    {
        CoolDown = 2;
        CoolCount = 0;
        Explanation = "������ �÷��̾��� ����� ���� ���θ� Ȯ���մϴ�.";
    }

    public override string GetResult()
    {
        // ��ĳ�ʻ��
        // �����ۻ���̶� ������ �Ϸ翡 �ι��� ����

        return $"{Target.Name}, {(Target.IsInfected ? "" : "��")}�����ڷ� Ȯ�εǾ����ϴ�.";
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
        Explanation = "������ �ʿ��� �ڿ��� ȹ���մϴ�.";
    }

    public override string GetResult()
    {
        Player.This.AddItem(ItemIndex.Scrap, 2);
        Player.This.AddItem(ItemIndex.Bolt, 2);
        Player.This.AddItem(ItemIndex.Wire, 2);
        // ���ϴ� �ڿ� ���޹���
        // ���� ���� �� ��ö/����/���� 4��, ������ü 2��, ��ĳ�� 1�� ����

        return $"�������� ��ö, ����, ���� �� 2���� ȹ���߽��ϴ�.";
    }
}

public class ControllerSkill : Skill
{
    public ControllerSkill()
    {
        CoolDown = 2;
        CoolCount = 0;
        Explanation = "�������� ID�� ���Ե� ���� �ϳ��� �����˴ϴ�.";
    }

    public override string GetResult()
    {
        int[] results = EarthCommunication.Ins.Together();
        return results[0] == -1 ?
            $"�������� ID {results[1]}�� ĭ ���ڰ� ������ ������ ������`{results[2]}" :
            $"{results[0]} �������� ID {results[1]}�� ĭ ���ڰ� ������ ������ ������`{results[2]}";
    }
}
