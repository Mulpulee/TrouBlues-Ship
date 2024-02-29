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

        // ��Ʈ�� ���
        PVHandler.pv.RPC("Intro", RpcTarget.All, 0);
        yield return new WaitForSeconds(4);
        PVHandler.pv.RPC("Intro", RpcTarget.All, 1);
        yield return new WaitForSeconds(4);
        PVHandler.pv.RPC("Intro", RpcTarget.All, 2);

        // HUD ����
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

            // ������ ���
            List<int> alives = new List<int>();
            foreach (var p in CommonData.Players) if (!p.IsDead) alives.Add(p.ProfileID);

            // ����ð� ����
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
                VoteManager.Ins.StartVote(VoteType.Normal, "������ ���ϱ�?", alives.Count + 1, hasSpy, alives.ToArray());

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

            // ����ð� ����, �긮�� ����
            m_endCount = CommonData.Players.Count;
            PVHandler.pv.RPC("Sleep", RpcTarget.All, 1);
            yield return new WaitForSeconds(1);
            PVHandler.pv.RPC("Brief", RpcTarget.All, 1);
            yield return new WaitUntil(() => m_endCount == 0);
            
            PVHandler.pv.RPC("UnlockAll", RpcTarget.All);
            PVHandler.pv.RPC("CutProgress", RpcTarget.All);

            // ���� �ൿ����
            m_endCount = 1;
            PVHandler.pv.RPC("Activity", RpcTarget.All, 0);
            yield return new WaitUntil(() => m_endCount == 0);
            yield return new WaitForSeconds(3);
            PVHandler.pv.RPC("Activity", RpcTarget.All, 3);

            // �ڿ� Ž��, ���� ��� ���� ������
            m_endCount = alives.Count;
            ItemSearching.Ins.Search(CommonData.Players.Count);
            EarthCommunication.Ins.StartCommunication(ChooseActivity.Ins.GetCommunicationCount());

            // �Ϸ� ���
            int[] searchResult = ItemSearching.Ins.GetDatas(false);
            PVHandler.pv.RPC("SetSearchDatas", RpcTarget.All,
                searchResult[0], searchResult[1], searchResult[2], ItemSearching.Ins.GetDatas(true));
            PVHandler.pv.RPC("Activity", RpcTarget.All, 1);
            yield return new WaitUntil(() => m_endCount == 0);

            // ���ּ� ����
            m_endCount = alives.Count;
            PVHandler.pv.RPC("Activity", RpcTarget.All, 2);
            yield return new WaitUntil(() => m_endCount == 0);

            PVHandler.pv.RPC("CutProgress", RpcTarget.All);

            // ���� ��ǥ
            m_endCount = 1; isTaskEnd = false;
            Timer.SetTimer(90);
            VoteManager.Ins.StartVote(VoteType.Normal, "������ ���ѱ�?", alives.Count + 1, alives.Count, alives.ToArray());
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
                VoteManager.Ins.StartVote(VoteType.ProsAndCons, "���� �� ����� ���ѱ�?", 2, alives.Count);
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

            // ������ ��� ��ǥ
            if (CommonData.Medecines > 0)
            {
                m_endCount = 1; isTaskEnd = false;
                Timer.SetTimer(60);
                VoteManager.Ins.StartVote(VoteType.Normal, "�������� �������� ����ұ�?", alives.Count + 1, alives.Count - haslocked, alives.ToArray());
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

            // �߹� ��ǥ
            Debug.Log($"{progress}/{m_goal} = {((float)progress / m_goal) * 100}%");
            Debug.Log($"{DataManager.Data.ExpelMilestones[CommonData.Spys.Count - 1][m_mileStone]}");
            while ((((float)progress / m_goal) * 100) >= DataManager.Data.ExpelMilestones[CommonData.Spys.Count - 1][m_mileStone])
            {
                m_canSpyKill = true;

                m_endCount = 1; isTaskEnd = false;
                Timer.SetTimer(90);
                VoteManager.Ins.StartVote(VoteType.Expel, "������ ��������?", alives.Count, alives.Count - haslocked, alives.ToArray());
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
                    VoteManager.Ins.StartVote(VoteType.ProsAndCons, "���� �� ����� ��������?", 2, alives.Count - haslocked);
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
                            ((float)(progress / m_goal) * 100) >= 100 ? "\"�������ƾƾƾƾƾƾƾƾƾƾ�......\"" : $"{alives.Count}���� ������ ����մϴ�...");
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
            ending = "����� �¸�!";
            subending = "�����̵� ���� ����浵 ���� ��ȭ�ο� ����";
        }
        else if (hasInfected && !hasSpy)
        {
            ending = "����� �¸�...?";
            subending = "�ϴ�... �������� ���߿� �� ã���� �ɰž�...";
        }
        else if (hasInfected && hasSpy)
        {
            ending = "������ �¸�!";
            subending = "������ ���ư��⸸ �ϸ� ��Ȯõ���� ���տ�..!";
        }
        else
        {
            ending = "������ �¸�...?";
            subending = "�������... ������... ��Ƴ������ϱ� ������...";
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
