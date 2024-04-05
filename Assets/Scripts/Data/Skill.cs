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
        Explanation = "지목한 플레이어의 직업을 확인하고, 그 능력을 사용합니다.";
        //Script = $"{Searched.Name}의 직업은  {Searched.PlayerJob.name}으로 밝혀졌습니다!";
    }

    public override string GetResult()
    {
        return $"{Target.Name}의 직업은  {Target.PlayerJob.Name}으로 밝혀졌습니다!";
    }
}

public class EngineerSkill : Skill
{
    public EngineerSkill()
    {
        CoolDown = 2;
        CoolCount = 0;
        Explanation = "우주선 수리 진척도가 일정 수준 올라갑니다.";
    }

    public override string GetResult()
    {
        PVHandler.pv.RPC("AddProgress", Photon.Pun.RpcTarget.MasterClient, DataManager.Data.EngineerSkillLevel[CommonData.Players.Count - 4]);
        // 우주선 수리 진척도 올림

        return "우주선 수리 진척도가 일정 수준 올라갔습니다.";
    }
}

public class MedicSkill : Skill
{
    public MedicSkill()
    {
        CoolDown = 2;
        CoolCount = 0;
        Explanation = "지목한 플레이어의 기생충 감염 여부를 확인합니다.";
    }

    public override string GetResult()
    {
        // 스캐너사용
        // 아이템사용이랑 별개라서 하루에 두번도 ㄱㄴ

        return $"{Target.Name}, {(Target.IsInfected ? "" : "비")}감염자로 확인되었습니다.";
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
        Explanation = "수리에 필요한 자원을 획득합니다.";
    }

    public override string GetResult()
    {
        Player.This.AddItem(ItemIndex.Scrap, 2);
        Player.This.AddItem(ItemIndex.Bolt, 2);
        Player.This.AddItem(ItemIndex.Wire, 2);
        // 원하는 자원 지급받음
        // 각각 선택 시 고철/나사/전선 4개, 초전도체 2개, 스캐너 1개 지급

        return $"관리인이 고철, 나사, 전선 각 2개를 획득했습니다.";
    }
}

public class ControllerSkill : Skill
{
    public ControllerSkill()
    {
        CoolDown = 2;
        CoolCount = 0;
        Explanation = "스파이의 ID에 포함된 문자 하나가 공개됩니다.";
    }

    public override string GetResult()
    {
        int[] results = EarthCommunication.Ins.Together();
        return results[0] == -1 ?
            $"스파이의 ID {results[1]}번 칸 문자가 다음과 같음이 밝혀짐`{results[2]}" :
            $"{results[0]} 스파이의 ID {results[1]}번 칸 문자가 다음과 같음이 밝혀짐`{results[2]}";
    }
}
