using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx // : MonoBehaviour
{
    public List<Player> m_players;
    public List<Player> m_spys;
    public Player m_infected;

    public void StartGame(int pPlayer)
    {
        Job tempJob = new Job();

        m_players = new List<Player>();
        m_players.Add(new Player(IdGenerator.GenerateID(true), tempJob));

        for (int i = 1; i < pPlayer; i++)
        {
            m_players.Add(new Player(IdGenerator.GenerateID(), tempJob));
        }

        m_spys = new List<Player>(m_players);
        int spycount = DataManager.Data.SpyPerPlayer[pPlayer - 4];

        for (int i = pPlayer - 1; i > spycount; i--)
        {
            m_spys.RemoveAt(Random.Range(0, m_spys.Count));
        }

        m_infected = m_spys[Random.Range(0, m_spys.Count)];
        m_spys.Remove(m_infected);
    }
}
