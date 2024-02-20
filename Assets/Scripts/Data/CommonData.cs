using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonData
{
    public static List<Player> Players;
    public static List<Player> Spys;
    public static Player Infected;

    public static int Medecines;
    public static int[] RepairProgress;
    public static int MultipleSuccessStack;

    public static void MakePlayerInfo(List<Player> pList)
    {
        Players = pList;
        IdGenerator.ClearID();
        foreach (Player p in Players) p.ID = IdGenerator.GenerateID();

        Spys = new List<Player>(Players);
        for (int i = Players.Count - 1; i > DataManager.Data.SpyPerPlayer[Players.Count - 4]; i--)
            Spys.RemoveAt(Random.Range(0, Spys.Count));

        Infected = Spys[Random.Range(0, Spys.Count)];
        Spys.Remove(Infected);

        Medecines = 0;
        RepairProgress = new int[3];
        MultipleSuccessStack = 0;
    }

    public static void SetPlayerInfo(int[] pPlayers, int[] pSpys, int pInfected, int[] pIDs)
    {
        // 받아온 데이터 바탕으로 플레이어 리스트 재구성(프로필, 이름, 아이디, 스파이/감염, 직업(나중에추가))

        Medecines = 0;
        RepairProgress = new int[3];
        MultipleSuccessStack = 0;
    }

    public static void AddMedicines(int pValue)
    {
        Medecines += pValue;
    }

    public static void AddProgress(int[] pValue)
    {
        for (int i = 0; i < pValue.Length; i++)
        {
            RepairProgress[i] += pValue[i];
        }
    }

    public static void AddSuccessStack(bool pReset)
    {
        if (pReset) MultipleSuccessStack = 0;
        else MultipleSuccessStack++;
    }
}
