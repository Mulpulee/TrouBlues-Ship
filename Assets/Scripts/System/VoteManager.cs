using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VoteType
{
    Normal,
    ProsAndCons,
    Expel
}

public class VoteManager
{
    private static VoteManager m_instance;
    public static VoteManager Ins
    {
        get
        {
            if (m_instance == null) m_instance = new VoteManager();
            return m_instance;
        }
    }

    private int[] m_vote;
    private int m_voteCount;
    private VoteType m_type;
    private int[] m_list;

    public void StartVote(VoteType type, string subject, int index, int count, int[] list = null)
    {
        m_type = type;
        m_vote = new int[index];
        m_voteCount = count;
        m_list = list;

        PVHandler.pv.RPC("StartVote", Photon.Pun.RpcTarget.All, type, subject, list);
    }

    public void Vote(int index)
    {
        m_vote[index]++;
        m_voteCount--;

        if (m_voteCount == 0)
        {
            PVHandler.pv.RPC("EndVote", Photon.Pun.RpcTarget.All, m_vote);
            GameManagerEx.Ins.TaskEnded();
        }
    }

    public int[] VoteResult { set { m_vote = value; } get { return m_vote; } }

    public int GetResult(bool index)
    {
        int result = 0;
        int highest = 0;
        for (int i = 0; i < m_vote.Length; i++)
        {
            if (m_vote[i] > highest)
            {
                result = i;
                highest = m_vote[i];
            }
            else if (m_vote[i] == highest)
            {
                result = -1;
            }
        }

        if (!index && result == 0) return -1;
        return index ? result : m_list[m_type == VoteType.Normal ? result - 1 : result];
    }
}
