using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EarthCommunication
{
    public static char[] CharacterID = { '¢À', '¡Ú', '¢¾', '¢¼', '¡ß', '¡Ü' };

    private static List<List<int>> m_openedAloneHint;
    private static List<List<int>> m_openedTogetherHint;

    private static List<int[]> m_spys;
    private static int m_spyCount;

    private static int m_alonePercent = 30;

    public static void SetInfo(List<Player> pSpylist)
    {
        m_spys = new List<int[]>();
        foreach (var item in pSpylist) m_spys.Add(item.ID);

        m_spyCount = m_spys.Count;

        m_openedAloneHint = new List<List<int>>();
        for (int i = 0; i < m_spyCount; i++) m_openedAloneHint.Add(new List<int> { 0, 1, 2, 3, 4 });
        m_openedTogetherHint = new List<List<int>>(m_openedAloneHint);
    }

    public static void StartCommunication(int pPlayer)
    {
        int[] percent = DataManager.Data.FailureProbability;

        if (Random.Range(0, 100) < (pPlayer == 1 ? m_alonePercent : percent[CommonData.MultipleSuccessStack]))
        {
            CommonData.AddSuccessStack(true);
            PVHandler.pv.RPC("Communicated", Photon.Pun.RpcTarget.All, false);
        }
        else
        {
            CommonData.AddSuccessStack();
            PVHandler.pv.RPC("Communicated", Photon.Pun.RpcTarget.All, true, pPlayer == 1 ? Alone() : Together());
        }
    }

    public static string Alone()
    {
        int index = Random.Range(0, m_spyCount);
        int[] id = m_spys[index];

        string indexing = m_spyCount == 1 ? "" : $"{index + 1}¹ø";
        int code;

        if (m_openedAloneHint[index].Count == 0) code = Random.Range(0, 6);
        else code = m_openedAloneHint[index][Random.Range(0, m_openedAloneHint[index].Count)];
        m_openedAloneHint[index].Remove(code);

        return $"{indexing} ½ºÆÄÀÌÀÇ ID¿¡´Â {CharacterID[id[code]]}°¡ Æ÷ÇÔµÊÀÌ ¹àÇôÁü";
    }

    public static string Together()
    {
        int index = Random.Range(0, m_spyCount);
        int[] id = m_spys[index];

        string indexing = m_spyCount == 1 ? "" : $"{index + 1}¹ø";
        int code;

        if (m_openedTogetherHint[index].Count == 0) code = Random.Range(0, 6);
        else code = m_openedTogetherHint[index][Random.Range(0, m_openedTogetherHint[index].Count)];
        m_openedTogetherHint[index].Remove(code);

        return $"{indexing} ½ºÆÄÀÌÀÇ ID {code + 1}¹ø Ä­ ¹®ÀÚ´Â {CharacterID[id[code]]}ÀÓÀÌ ¹àÇôÁü";
    }
}
