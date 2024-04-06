using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthCommunication
{
    private static EarthCommunication m_instance;
    public static EarthCommunication Ins
    {
        get
        {
            if (m_instance == null) m_instance = new EarthCommunication();
            return m_instance;
        }
    }

    public static Sprite[] CharacterID; // = { '¢À', '¡Ú', '¢¾', '¢¼' }; // , '¡ß', '¡Ü' };

    private List<List<int>> m_openedAloneHint;
    private List<List<int>> m_openedTogetherHint;

    private List<int[]> m_spys;
    private int m_spyCount;

    private int m_alonePercent = 30;

    public void Init()
    {
        CharacterID = Resources.LoadAll<Sprite>("ID");

        m_spys = new List<int[]>();
        foreach (var item in CommonData.Players)
        {
            if (item.IsSpy) m_spys.Add(item.ID);
        }

        m_spyCount = m_spys.Count;

        m_openedAloneHint = new List<List<int>>();
        for (int i = 0; i < m_spyCount; i++) m_openedAloneHint.Add(new List<int> { 0, 1, 2, 3, 4 });
        m_openedTogetherHint = new List<List<int>>(m_openedAloneHint);
    }

    public void StartCommunication(int pPlayer)
    {
        if (pPlayer == 0) return;

        int[] percent = DataManager.Data.FailureProbability;

        if (Random.Range(0, 100) < (pPlayer == 1 ? m_alonePercent : percent[CommonData.MultipleSuccessStack]))
        {
            CommonData.AddSuccessStack(true);
            PVHandler.pv.RPC("Communicated", Photon.Pun.RpcTarget.All, false, null);
        }
        else
        {
            if (pPlayer > 1) CommonData.AddSuccessStack();
            PVHandler.pv.RPC("Communicated", Photon.Pun.RpcTarget.All, true, pPlayer == 1 ? Alone() : Together());
        }
    }

    public int[] Alone()
    {
        int index = Random.Range(0, m_spyCount);
        int[] id = m_spys[index];

        int code;

        if (m_openedAloneHint[index].Count == 0) code = Random.Range(0, 4);
        else code = m_openedAloneHint[index][Random.Range(0, m_openedAloneHint[index].Count)];
        m_openedAloneHint[index].Remove(code);

        return new int[] { m_spyCount == 1 ? -1 : index + 1, id[code] };
    }

    public int[] Together()
    {
        int index = Random.Range(0, m_spyCount);
        int[] id = m_spys[index];

        int code;

        if (m_openedTogetherHint[index].Count == 0) code = Random.Range(0, 4);
        else code = m_openedTogetherHint[index][Random.Range(0, m_openedTogetherHint[index].Count)];
        m_openedTogetherHint[index].Remove(code);

        return new int[] { m_spyCount == 1 ? -1 : index + 1, code + 1, id[code] };
    }
}
