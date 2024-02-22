using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonData
{
    public static int ProfileID;

    public static List<Player> Players;
    public static List<Player> Spys;
    public static Player Infected;

    public static int Medecines;
    public static int[] RepairProgress;
    public static int MultipleSuccessStack;

    public static void MakePlayerInfo(int[] pList)
    {
        Profile[] profiles = Resources.LoadAll<Profile>("ScriptableObject/Profile");
        JobManager.SetJobs();

        Players = new List<Player>();
        IdGenerator.ClearID();

        for (int i = 0; i < pList.Length; i++)
        {
            Players.Add(new Player()
            {
                PlayerProfile = profiles[pList[i]].profile,
                ProfileID = pList[i],
                Name = profiles[pList[i]].nickName,
                ID = IdGenerator.GenerateID()
            });
        }

        Spys = new List<Player>(Players);
        for (int i = Players.Count - 1; i > DataManager.Data.SpyPerPlayer[Players.Count - 4]; i--)
            Spys.RemoveAt(Random.Range(0, Spys.Count));

        Infected = Spys[Random.Range(0, Spys.Count)];
        Spys.Remove(Infected);

        foreach (Player p in Spys) p.SetSpy();
        Infected.SetInfected();

        List<JobType> jobList = new List<JobType> { JobType.Engineer, JobType.Medic, JobType.Janitor, JobType.Controller };
        for (int i = jobList.Count - 1; i > DataManager.Data.JobPerPlayer[Players.Count - 4]; i--)
            jobList.RemoveAt(Random.Range(0, jobList.Count));

        Player captain = Players[Random.Range(0, Players.Count)];
        captain.PlayerJob = JobManager.GetJob(JobType.Captain);

        for (int i = 1; i < Players.Count; i++)
        {
            Player player = Players[Random.Range(0, Players.Count)];

            while (player.PlayerJob != null) player = Players[Random.Range(0, Players.Count)];

            if (jobList.Count == 0)
            {
                player.PlayerJob = JobManager.GetJob(JobType.None);
            }
            else
            {
                player.PlayerJob = JobManager.GetJob(jobList[Random.Range(0, jobList.Count)]);
                jobList.Remove(player.PlayerJob.Type);
            }

            if (player.ProfileID == ProfileID) Player.This = player;
        }

        Medecines = 0;
        RepairProgress = new int[3];
        MultipleSuccessStack = 0;

        int[] spyArr = new int[Spys.Count];
        int[] idArr = new int[Players.Count * 5];
        int[] jobArr = new int[Players.Count];

        for (int i = 0; i < Players.Count; i++)
        {
            for (int j = 0; j < 5; j++) idArr[i * 5 + j] = Players[i].GetID(j);
            jobArr[i] = (int)Players[i].PlayerJob.Type;
        }

        for (int i = 0; i < Spys.Count; i++) spyArr[i] = Spys[i].ProfileID;

        PVHandler.pv.RPC("SetPlayerList", Photon.Pun.RpcTarget.Others, pList, spyArr, Infected.ProfileID, idArr, jobArr);
    }

    public static void SetPlayerInfo(int[] pPlayers, int[] pSpys, int pInfected, int[] pIDs, int[] pJobs)
    {
        Profile[] profiles = Resources.LoadAll<Profile>("ScriptableObject/Profile");
        JobManager.SetJobs();

        Players = new List<Player>();
        Spys = new List<Player>();

        for (int i = 0; i < pPlayers.Length; i++)
        {
            Player player = new Player()
            {
                PlayerProfile = profiles[pPlayers[i]].profile,
                ProfileID = pPlayers[i],
                Name = profiles[pPlayers[i]].nickName,
                ID = new int[5] { pIDs[i * 5], pIDs[i * 5 + 1], pIDs[i * 5 + 2], pIDs[i * 5 + 3], pIDs[i * 5 + 4] },
                PlayerJob = JobManager.GetJob((JobType)pJobs[i])
            };
            foreach (int s in pSpys)
            {
                if (s == pPlayers[i])
                {
                    player.SetSpy();
                    Spys.Add(player);
                }
            }

            if (pInfected == pPlayers[i])
            {
                player.SetInfected();
                Infected = player;
            }

            Players.Add(player);

            if (player.ProfileID == ProfileID) Player.This = player;
        }

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
