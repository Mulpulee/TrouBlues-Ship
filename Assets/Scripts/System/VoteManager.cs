using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VoteType
{
    Normal,
    ProsAndCons,
    Expel
}

public static class VoteManager
{
    private static int[] m_vote;
    private static int m_voteCount;
    private static VoteType m_type;

    public static void StartVote(VoteType type, string subject, int index, Player[] list = null)
    {
        m_type = type;
        m_vote = new int[index];
        m_voteCount = 0;

        GameObject.FindObjectOfType<VoteUI>().StartVote(type, subject, list);
    }

    public static void Vote(int index)
    {
        m_vote[index]++;
        m_voteCount++;
    }

    public static int[] VoteResult { set { m_vote = value; } get { return m_vote; } }

    public static int GetResult()
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

        return result;
    }
}
