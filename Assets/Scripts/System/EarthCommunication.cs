using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthCommunication
{
    private static EarthCommunication instance;
    public static EarthCommunication ins
    {
        get
        {
            if (instance == null) instance = new EarthCommunication();
            return instance;
        }
    }
    public static char[] CharacterID = { '♣', '★', '♥', '♠', '◆', '●' };

    private List<List<int>> m_openedAloneHint;
    private List<List<int>> m_openedTogetherHint;

    private List<int[]> m_spys;
    private int m_spyCount;

    private int m_successStack = 0;
    private int m_alonePercent = 30;

    public void SetInfo(List<Player> pSpylist)
    {
        m_spys = new List<int[]>();
        foreach (var item in pSpylist) m_spys.Add(item.ID);

        m_spyCount = m_spys.Count;

        m_openedAloneHint = new List<List<int>>();
        for (int i = 0; i < m_spyCount; i++) m_openedAloneHint.Add(new List<int> { 0, 1, 2, 3, 4 });
        m_openedTogetherHint = new List<List<int>>(m_openedAloneHint);
    }

    public bool StartCommunication(int pPlayer)
    {
        int[] percent = DataManager.Data.FailureProbability;

        if (Random.Range(0, 100) < (pPlayer == 1 ? m_alonePercent : percent[m_successStack]))
        {
            m_successStack = 0;
            //Debug.Log("(치지직)....&#$....$..@$...#.>>$>...%>>..(치지직)..$%#..#..@%...");
            //Debug.Log("연결이 불안정합니다.");
            return false;
        }
        else
        {
            if (pPlayer == 1) Alone();
            else Together();

            return true;
        }
    }

    public string Alone()
    {
        int index = Random.Range(0, m_spyCount);
        int[] id = m_spys[index];

        string indexing = m_spyCount == 1 ? "" : $"{index + 1}번";
        int code;

        if (m_openedAloneHint[index].Count == 0) code = Random.Range(0, 6);
        else code = m_openedAloneHint[index][Random.Range(0, m_openedAloneHint[index].Count)];
        m_openedAloneHint[index].Remove(code);

        return $"{indexing} 스파이의 ID에는 {CharacterID[id[code]]}가 포함됨이 밝혀짐";
    }

    public string Together()
    {
        int index = Random.Range(0, m_spyCount);
        int[] id = m_spys[index];

        string indexing = m_spyCount == 1 ? "" : $"{index + 1}번";
        int code;

        if (m_openedTogetherHint[index].Count == 0) code = Random.Range(0, 6);
        else code = m_openedTogetherHint[index][Random.Range(0, m_openedTogetherHint[index].Count)];
        m_openedTogetherHint[index].Remove(code);

        return $"{indexing} 스파이의 ID {code + 1}번 칸 문자는 {CharacterID[id[code]]}임이 밝혀짐";
    }
}
