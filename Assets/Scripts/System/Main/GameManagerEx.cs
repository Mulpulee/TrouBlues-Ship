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
            //if (m_instance == null) m_instance = GameObject.FindObjectOfType<GameManagerEx>();
            return m_instance;
        }
    }

    private int m_endCount = 8;
    public int EndCount { get { return m_endCount; } set { m_endCount = value; } }

    private int m_mileStone = 0;
    private int m_goal;
    private bool m_canSpyKill = false;

    private Coroutine m_ingameRoutine;

    private void Start()
    {
        if (PVHandler.pv.IsMine) m_instance = this;
        PVHandler.pv.RPC("TaskEnded", RpcTarget.MasterClient);
        if (!PVHandler.pv.IsMine) Destroy(gameObject);
        else m_instance = this;

        m_goal = 0;
        for (int i = 0; i < DataManager.Data.ShipRequirements[0].Length - 1; i++)
            m_goal += DataManager.Data.ShipRequirements[CommonData.Players.Count - 4][i];

        EarthCommunication.Ins.Init();
        StartCoroutine(Intro());
    }

    public void TaskEnded()
    {
        m_endCount--;
    }

    private IEnumerator Intro()
    {
        PVHandler.pv.RPC("SetGameScene", RpcTarget.All);

        // 인트로 재생
        PVHandler.pv.RPC("Intro", RpcTarget.All, 0);
        yield return new WaitForSeconds(4);
        PVHandler.pv.RPC("Intro", RpcTarget.All, 1);
        yield return new WaitForSeconds(4);
        PVHandler.pv.RPC("Intro", RpcTarget.All, 2);

        // HUD 띄우기
        PVHandler.pv.RPC("Hud", RpcTarget.All, 1, false);
        PVHandler.pv.RPC("Hud", RpcTarget.All, 0, true);

        m_ingameRoutine = StartCoroutine(InGame());
    }

    private IEnumerator InGame()
    {
        while (true)
        {
            bool isTaskEnd = false;
            Coroutine timer;

            // 생존자 목록
            List<int> alives = new List<int>();
            foreach (var p in CommonData.Players) if (!p.IsDead) alives.Add(p.ProfileID);

            // 수면시간 시작
            PVHandler.pv.RPC("Brief", RpcTarget.All, 0);

            int hasSpy = 0;
            foreach (var p in CommonData.Spys) if (!p.IsDead && !p.IsLocked) hasSpy++;

            if (m_canSpyKill && hasSpy > 0)
            {
                m_canSpyKill = false;
                PVHandler.pv.RPC("SetSpyMode", RpcTarget.All, true);

                Timer.SetTimer(30);
                timer = StartCoroutine(ReduceTime(0, () => isTaskEnd = true));
                m_endCount = 1;
                VoteManager.Ins.StartVote(VoteType.Normal, "누구를 죽일까?", alives.Count + 1, hasSpy, alives.ToArray());

                while (m_endCount != 0)
                {
                    yield return null;
                    if (isTaskEnd)
                    {
                        PVHandler.pv.RPC("EndVote", RpcTarget.All, VoteManager.Ins.VoteResult);
                        break;
                    }
                }
                Timer.SetTimer(0); StopCoroutine(timer);
                yield return new WaitForSeconds(3);

                int killed = VoteManager.Ins.GetResult();
                if (killed != -1)
                {
                    foreach (var p in CommonData.Players) if (p.ProfileID == killed) p.SetDead();
                    alives.Remove(killed);
                    PVHandler.pv.RPC("SendDeadPlayer", RpcTarget.All, killed);
                }

                PVHandler.pv.RPC("SetSpyMode", RpcTarget.All, false);
                PVHandler.pv.RPC("HideVote", RpcTarget.All);
            }

            isTaskEnd = false;
            Timer.SetTimer(60);
            StartCoroutine(ReduceTime(0, () => isTaskEnd = true));
            PVHandler.pv.RPC("Sleep", RpcTarget.All, 0);
            yield return new WaitUntil(() => isTaskEnd == true);
            foreach (var p in CommonData.Players) if (p.IsLocked) p.IsLocked = false;

            // 수면시간 종료, 브리핑 시작
            m_endCount = CommonData.Players.Count;
            PVHandler.pv.RPC("Sleep", RpcTarget.All, 1);
            yield return new WaitForSeconds(1);
            PVHandler.pv.RPC("Brief", RpcTarget.All, 1);
            yield return new WaitUntil(() => m_endCount == 0);
            
            PVHandler.pv.RPC("UnlockAll", RpcTarget.All);
            PVHandler.pv.RPC("CutProgress", RpcTarget.All);

            // 오전 행동선택
            m_endCount = 1;
            PVHandler.pv.RPC("Activity", RpcTarget.All, 0);
            yield return new WaitUntil(() => m_endCount == 0);
            yield return new WaitForSeconds(3);
            PVHandler.pv.RPC("Activity", RpcTarget.All, 3);

            // 자원 탐색, 지구 통신 로직 돌리기
            m_endCount = alives.Count;
            ItemSearching.Ins.Search(CommonData.Players.Count);
            EarthCommunication.Ins.StartCommunication(ChooseActivity.Ins.GetCommunicationCount());

            // 완료 대기
            int[] searchResult = ItemSearching.Ins.GetDatas(false);
            PVHandler.pv.RPC("SetSearchDatas", RpcTarget.All,
                searchResult[0], searchResult[1], searchResult[2], ItemSearching.Ins.GetDatas(true));
            PVHandler.pv.RPC("Activity", RpcTarget.All, 1);
            yield return new WaitUntil(() => m_endCount == 0);

            // 우주선 수리
            m_endCount = alives.Count;
            PVHandler.pv.RPC("Activity", RpcTarget.All, 2);
            yield return new WaitUntil(() => m_endCount == 0);

            PVHandler.pv.RPC("CutProgress", RpcTarget.All);

            // 구금 투표
            m_endCount = 1; isTaskEnd = false;
            Timer.SetTimer(90);
            VoteManager.Ins.StartVote(VoteType.Normal, "누구를 가둘까?", alives.Count + 1, alives.Count, alives.ToArray());
            timer = StartCoroutine(ReduceTime(0, () => isTaskEnd = true));
            while (m_endCount != 0)
            {
                yield return null;
                if (isTaskEnd)
                {
                    PVHandler.pv.RPC("EndVote", RpcTarget.All, VoteManager.Ins.VoteResult);
                    break;
                }
            }
            Timer.SetTimer(0); StopCoroutine(timer);
            yield return new WaitForSeconds(3);

            int haslocked = 0;
            int locked = VoteManager.Ins.GetResult();
            if (locked != -1)
            {
                m_endCount = 1; isTaskEnd = false;
                Timer.SetTimer(30);
                VoteManager.Ins.StartVote(VoteType.ProsAndCons, "정말 이 사람을 가둘까?", 2, alives.Count);
                timer = StartCoroutine(ReduceTime(0, () => isTaskEnd = true));
                while (m_endCount != 0)
                {
                    yield return null;
                    if (isTaskEnd)
                    {
                        PVHandler.pv.RPC("EndVote", RpcTarget.All, VoteManager.Ins.VoteResult);
                        break;
                    }
                }
                Timer.SetTimer(0); StopCoroutine(timer);

                if (VoteManager.Ins.GetResult(true) == 0)
                {
                    foreach (var p in CommonData.Players)
                    {
                        if (p.ProfileID == locked)
                        {
                            p.IsLocked = true;
                            PVHandler.pv.RPC("UpdatePlayerInfo", RpcTarget.All, locked, p.IsInfected, true);
                        }
                    }
                    haslocked = 1;
                }
                yield return new WaitForSeconds(3);
            }

            // 구충제 사용 투표
            if (CommonData.Medecines > 0)
            {
                m_endCount = 1; isTaskEnd = false;
                Timer.SetTimer(60);
                VoteManager.Ins.StartVote(VoteType.Normal, "누구에게 구충제를 사용할까?", alives.Count + 1, alives.Count - haslocked, alives.ToArray());
                timer = StartCoroutine(ReduceTime(0, () => isTaskEnd = true));
                while (m_endCount != 0)
                {
                    yield return null;
                    if (isTaskEnd)
                    {
                        PVHandler.pv.RPC("EndVote", RpcTarget.All, VoteManager.Ins.VoteResult);
                        break;
                    }
                }
                Timer.SetTimer(0); StopCoroutine(timer);
                yield return new WaitForSeconds(3);

                int used = VoteManager.Ins.GetResult();
                if (used != -1)
                {
                    foreach (var p in CommonData.Players)
                    {
                        if (p.ProfileID == used)
                        {
                            p.SetInfected(false);
                            PVHandler.pv.RPC("UpdatePlayerInfo", RpcTarget.All, used, false, p.IsLocked);
                        }
                    }
                    CommonData.AddMedicines(-1);
                }
            }

            PVHandler.pv.RPC("HideVote", RpcTarget.All);

            int progress = 0;
            foreach (var i in CommonData.RepairProgress) progress += i;

            // 추방 투표
            Debug.Log($"{progress}/{m_goal} = {((float)progress / m_goal) * 100}%");
            Debug.Log($"{DataManager.Data.ExpelMilestones[CommonData.Spys.Count - 1][m_mileStone]}");
            while ((((float)progress / m_goal) * 100) >= DataManager.Data.ExpelMilestones[CommonData.Spys.Count - 1][m_mileStone])
            {
                m_canSpyKill = true;

                m_endCount = 1; isTaskEnd = false;
                Timer.SetTimer(90);
                VoteManager.Ins.StartVote(VoteType.Expel, "누구를 내보낼까?", alives.Count, alives.Count - haslocked, alives.ToArray());
                timer = StartCoroutine(ReduceTime(0, () => isTaskEnd = true));
                while (m_endCount != 0)
                {
                    yield return null;
                    if (isTaskEnd)
                    {
                        PVHandler.pv.RPC("EndVote", RpcTarget.All, VoteManager.Ins.VoteResult);
                        break;
                    }
                }
                Timer.SetTimer(0); StopCoroutine(timer);
                yield return new WaitForSeconds(3);
                PVHandler.pv.RPC("HideVote", RpcTarget.All);

                int expeled = VoteManager.Ins.GetResult(); Debug.Log(expeled);

                if (expeled != -1)
                {
                    m_endCount = 1; isTaskEnd = false;
                    Timer.SetTimer(30);
                    VoteManager.Ins.StartVote(VoteType.ProsAndCons, "정말 이 사람을 내보낼까?", 2, alives.Count - haslocked);
                    timer = StartCoroutine(ReduceTime(0, () => isTaskEnd = true));
                    while (m_endCount != 0)
                    {
                        yield return null;
                        if (isTaskEnd)
                        {
                            PVHandler.pv.RPC("EndVote", RpcTarget.All, VoteManager.Ins.VoteResult);
                            break;
                        }
                    }
                    Timer.SetTimer(0); StopCoroutine(timer);
                    yield return new WaitForSeconds(3);
                    PVHandler.pv.RPC("HideVote", RpcTarget.All);

                    if (VoteManager.Ins.GetResult(true) == 0)
                    {
                        foreach (var p in CommonData.Players) if (p.ProfileID == expeled) { p.SetDead(); alives.Remove(expeled); }

                        m_endCount = CommonData.Players.Count;
                        PVHandler.pv.RPC("Expel", RpcTarget.All, 0, expeled,
                            ((float)(progress / m_goal) * 100) >= 100 ? "\"으으으아아아아아아아아아아아......\"" : $"{alives.Count}명이 비행을 계속합니다...");
                        yield return new WaitUntil(() => m_endCount == 0);
                        PVHandler.pv.RPC("Expel", RpcTarget.All, 1, 0, "");
                    }
                }

                foreach (var p in CommonData.Players) if (p.PlayerJob.Type == JobType.Captain) p.PlayerJob.SpecialSkill.CoolCount = 0;
                
                m_mileStone++;
                if (m_mileStone == CommonData.Spys.Count)
                {
                    StartCoroutine(Ending());
                    StopCoroutine(m_ingameRoutine);
                    break;
                }
            }
            if (m_mileStone == CommonData.Spys.Count)
            {
                break;
            }
        }
    }

    private IEnumerator Ending()
    {
        PVHandler.pv.RPC("Expel", RpcTarget.All, 1, 0, "");
        yield return new WaitForSeconds(3);

        PVHandler.pv.RPC("Hud", RpcTarget.All, 0, false);

        bool hasInfected = false;
        bool hasSpy = false;
        foreach (var p in CommonData.Players)
        {
            if (p.IsInfected && !p.IsDead) hasInfected = true;
            if (p.IsSpy && !p.IsDead) hasSpy = true;
        }

        string ending;
        string subending;

        if (!hasInfected && !hasSpy)
        {
            ending = "비행사 승리!";
            subending = "스파이도 없고 기생충도 없는 평화로운 세상";
        }
        else if (hasInfected && !hasSpy)
        {
            ending = "비행사 승리...?";
            subending = "일단... 구충제는 나중에 또 찾으면 될거야...";
        }
        else if (hasInfected && hasSpy)
        {
            ending = "스파이 승리!";
            subending = "지구로 돌아가기만 하면 일확천금이 눈앞에..!";
        }
        else
        {
            ending = "스파이 승리...?";
            subending = "기생충은... 없지만... 살아남았으니까 괜찮아...";
        }

        PVHandler.pv.RPC("Ending", RpcTarget.All, 0, ending, subending);
        yield return new WaitForSeconds(5);
        PVHandler.pv.RPC("Ending", RpcTarget.All, 1, "", "");
        yield return new WaitForSeconds(2);

        PVHandler.pv.RPC("ReturnToLobby", RpcTarget.All);

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PVHandler.pv.RPC("AddProgress", RpcTarget.MasterClient, new int[] { 5, 5, 5 });
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (int i = 0; i < 5; i++)
                Timer.ReduceTimer();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (int i = 0; i < 5; i++)
                Timer.ReduceTimer(true);
        }
    }
}
