using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : MonoBehaviour
{
    private static GameManagerEx m_instance;
    public static GameManagerEx Ins
    {
        get
        {
            if (m_instance == null) m_instance = GameObject.FindObjectOfType<GameManagerEx>();
            return m_instance;
        }
    }

    private int m_endCount = 8;
    public int EndCount { get { return m_endCount; } set { m_endCount = value; } }

    private void Start()
    {
        PVHandler.pv.RPC("TaskEnded", RpcTarget.MasterClient);
        if (!PVHandler.pv.IsMine) Destroy(gameObject);
        else m_instance = this;

        EarthCommunication.Ins.Init();
        StartCoroutine(InGame());
    }

    public void TaskEnded()
    {
        m_endCount--;
    }

    private IEnumerator InGame()
    {
        yield return new WaitUntil(() => m_endCount == 8 - CommonData.Players.Count);

        bool isTaskEnd = false;

        PVHandler.pv.RPC("Intro", RpcTarget.All, 0);
        yield return new WaitForSeconds(4);
        PVHandler.pv.RPC("Intro", RpcTarget.All, 1);
        yield return new WaitForSeconds(4);
        PVHandler.pv.RPC("Intro", RpcTarget.All, 2);

        PVHandler.pv.RPC("Hud", RpcTarget.All, 1, false);
        PVHandler.pv.RPC("Hud", RpcTarget.All, 0, true);

        PVHandler.pv.RPC("Brief", RpcTarget.All, 0);
        Timer.SetTimer(60);
        StartCoroutine(ReduceTime(0, () => isTaskEnd = true));
        PVHandler.pv.RPC("Sleep", RpcTarget.All, 0);
        yield return new WaitUntil(() => isTaskEnd == true);

        m_endCount = CommonData.Players.Count;
        PVHandler.pv.RPC("Sleep", RpcTarget.All, 1);
        yield return new WaitForSeconds(1);
        PVHandler.pv.RPC("Brief", RpcTarget.All, 1);
        yield return new WaitUntil(() => m_endCount == 0);

        m_endCount = 1;
        PVHandler.pv.RPC("Activity", RpcTarget.All, 0);
        yield return new WaitUntil(() => m_endCount == 0);
        yield return new WaitForSeconds(3);
        PVHandler.pv.RPC("Activity", RpcTarget.All, 3);

        m_endCount = CommonData.Players.Count;
        ItemSearching.Ins.Search(CommonData.Players.Count);
        EarthCommunication.Ins.StartCommunication(ChooseActivity.Ins.GetCommunicationCount());

        int[] searchResult = ItemSearching.Ins.GetDatas(false);
        PVHandler.pv.RPC("SetSearchDatas", RpcTarget.All,
            searchResult[0], searchResult[1], searchResult[2], ItemSearching.Ins.GetDatas(true));
        PVHandler.pv.RPC("Activity", RpcTarget.All, 1);
        yield return new WaitUntil(() => m_endCount == 0);

        m_endCount = CommonData.Players.Count;
        PVHandler.pv.RPC("Activity", RpcTarget.All, 2);
        yield return new WaitUntil(() => m_endCount == 0);

        m_endCount = 1;
        List<int> list = new List<int>();
        foreach (var p in CommonData.Players) if (!p.IsDead) list.Add(p.ProfileID);
        VoteManager.Ins.StartVote(VoteType.Normal, "누구를 가둘까?", list.Count, list.Count, list.ToArray());
        yield return new WaitUntil(() => m_endCount == 0);

        int locked = VoteManager.Ins.GetResult(false);
        if (locked != -1)
        {
            foreach (var p in CommonData.Players) if (p.ProfileID == locked) p.IsLocked = true;
            list.Remove(locked);
        }
    }
    
    private IEnumerator ReduceTime(int pPoint = 0, Action pAction = null)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Timer.ReduceTimer();

            if (pAction != null && Timer.Time == pPoint)
            {
                pAction.Invoke();
                break;
            }
        }
    }
}
