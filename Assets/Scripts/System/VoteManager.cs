using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoteManager
{
    private static int[] m_vote;
    private static int m_voteCount;

    public static void StartVote(int index)
    {
        m_vote = new int[index];
        m_voteCount = 0;
    }

    public static void Vote(int index)
    {
        m_vote[index]++;
        m_voteCount++;
    }

    public static int[] VoteResult { set { m_vote = value; } get { return m_vote; } }
}
