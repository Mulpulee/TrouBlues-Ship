using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChooseActivity
{
    private static List<int>[] m_selectedMemberLists;
    private static int m_memberCount;

    public static void Init()
    {
        m_selectedMemberLists = new List<int>[2] { new List<int>(), new List<int>() };
        m_memberCount = 0;

        foreach (var p in CommonData.Players)
        {
            if (!p.IsDead) m_memberCount++;
        }

        PVHandler.pv.RPC("StartChoosing", Photon.Pun.RpcTarget.All);
    }

    public static void AddMember(int pIndex, int pProfileId)
    {
        m_selectedMemberLists[pIndex].Add(pProfileId);
        m_memberCount--;

        if (m_memberCount == 0)
        {
            PVHandler.pv.RPC("EndChoose", Photon.Pun.RpcTarget.All,
                new int[][] { m_selectedMemberLists[0].ToArray(), m_selectedMemberLists[1].ToArray() });
        }
    }
}
