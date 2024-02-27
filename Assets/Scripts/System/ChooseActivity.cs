using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseActivity
{
    private static ChooseActivity m_instance;
    public static ChooseActivity Ins
    {
        get
        {
            if (m_instance == null) m_instance = new ChooseActivity();
            return m_instance;
        }
    }

    private List<int>[] m_selectedMemberLists;
    private int m_memberCount;
    public int SelectedActivity;

    public void Init()
    {
        m_selectedMemberLists = new List<int>[2] { new List<int>(), new List<int>() };
        m_memberCount = 0;

        foreach (var p in CommonData.Players)
        {
            if (!p.IsDead) m_memberCount++;
        }

        PVHandler.pv.RPC("StartChoosing", Photon.Pun.RpcTarget.All);
    }

    public void AddMember(int pIndex, int pProfileId)
    {
        m_selectedMemberLists[pIndex].Add(pProfileId);
        m_memberCount--;

        if (m_memberCount == 0)
        {
            PVHandler.pv.RPC("EndChoose", Photon.Pun.RpcTarget.All,
                new int[][] { m_selectedMemberLists[0].ToArray(), m_selectedMemberLists[1].ToArray() });

            GameManagerEx.Ins.TaskEnded();
        }
    }

    public int GetCommunicationCount()
    {
        return m_selectedMemberLists[1].Count;
    }
}
