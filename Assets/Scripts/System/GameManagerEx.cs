using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonData
{
    public static List<Player> Players;
    public static List<Player> Spys;
    public static Player Infected;

    public static int Medecines;
}

public class GameManagerEx // : MonoBehaviour
{
    private static Player m_this;
    public static Player Player
    {
        get { return m_this; }
    }

    public void StartGame(int pPlayer)
    {        
        Job tempJob = new Job();

        CommonData.Players = new List<Player>();
        CommonData.Players.Add(new Player(IdGenerator.GenerateID(true), tempJob));

        for (int i = 1; i < pPlayer; i++)
        {
            CommonData.Players.Add(new Player(IdGenerator.GenerateID(), tempJob));
        }

        CommonData.Spys = new List<Player>(CommonData.Players);
        int spycount = DataManager.Data.SpyPerPlayer[pPlayer - 4];

        for (int i = pPlayer - 1; i > spycount; i--)
        {
            CommonData.Spys.RemoveAt(Random.Range(0, CommonData.Spys.Count));
        }

        CommonData.Infected = CommonData.Spys[Random.Range(0, CommonData.Spys.Count)];
        CommonData.Spys.Remove(CommonData.Infected);

        m_this = CommonData.Players[0];

        CommonData.Medecines = 0;
    }
}
