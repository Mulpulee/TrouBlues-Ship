using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PVHandler : MonoBehaviour
{
    public static PhotonView pv;

    #region Lobby

    [PunRPC]
    public void AddPlayer(int pItem)
    {
        FindObjectOfType<LobbyManager>().AddPlayer(pItem);
    }

    [PunRPC]
    public void RemovePlayer(int pItem)
    {
        FindObjectOfType<LobbyManager>().RemovePlayer(pItem);
    }

    [PunRPC]
    public void Ready(int pItem)
    {
        FindObjectOfType<LobbyManager>().Ready(pItem);
    }

    [PunRPC]
    public void ClearReady()
    {
        FindObjectOfType<TitleSceneManager>().ClearReady();
    }

    [PunRPC]
    public void SetGameScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
    }

    [PunRPC]
    public void ReturnToLobby()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TitleScene"));
        SceneManager.UnloadSceneAsync("GameScene");
    }

    #endregion

    #region CommonData

    [PunRPC]
    public void SetPlayerList(int[] pPlayers, int[] pSpys, int pInfected, int[] pIDs, int[] pJobs)
    {
        CommonData.SetPlayerInfo(pPlayers, pSpys, pInfected, pIDs, pJobs);
    }

    [PunRPC]
    public void SetTimer(int pTime)
    {
        Timer.Time = pTime;
    }

    [PunRPC]
    public void AddMedicines(int pValue)
    {
        CommonData.AddMedicines(pValue);
    }

    [PunRPC]
    public void SetMedicineCount(int pValue)
    {
        CommonData.Medecines = pValue;
    }

    [PunRPC]
    public void AddProgress(int[] pValue)
    {
        CommonData.AddProgress(pValue);
    }

    [PunRPC]
    public void SetProgress(int[] pValue)
    {
        CommonData.RepairProgress = pValue;
    }

    [PunRPC]
    public void CutProgress()
    {
        CommonData.CutProgress();
    }

    [PunRPC]
    public void UpdatePlayerInfo(int pProfileId, bool pInfected, bool pLocked)
    {
        CommonData.UpdatePlayerInfo(pProfileId, pInfected, pLocked);
    }

    #endregion

    #region Vote

    [PunRPC]
    public void StartVote(VoteType pType, string pSubject, int[] pList)
    {
        FindObjectOfType<VoteUI>().StartVote(pType, pSubject, pList);
    }

    [PunRPC]
    public void Vote(int pIndex)
    {
        VoteManager.Ins.Vote(pIndex);
    }

    [PunRPC]
    public void EndVote(int[] pResult)
    {
        FindObjectOfType<VoteUI>().EndVote(pResult);
    }

    [PunRPC]
    public void HideVote()
    {
        FindObjectOfType<VoteUI>().Hide();
    }

    [PunRPC]
    public void SetSpyMode(bool pSpyMode)
    {
        FindObjectOfType<VoteUI>().SpyMode = pSpyMode;
    }

    #endregion

    #region Sleep/Briefing

    [PunRPC]
    public void SendDeadPlayer(int pProfileId)
    {
        BriefingManager.Ins.DeadPlayer(pProfileId);
    }

    [PunRPC]
    public void UseSkill(JobType pType, string pScript)
    {
        BriefingManager.Ins.UseSkill(pType, pScript);
    }

    [PunRPC]
    public void UseScanner(int pProfileId)
    {
        BriefingManager.Ins.UseScanner(pProfileId);
    }

    #endregion

    #region ChooseActivity

    [PunRPC]
    public void StartChoosing()
    {
        FindObjectOfType<ChooseActivityUI>().StartChoosing();
    }

    [PunRPC]
    public void Choose(int pIndex, int pProfileId)
    {
        ChooseActivity.Ins.AddMember(pIndex, pProfileId);
    }

    [PunRPC]
    public void EndChoose(int[][] pResults)
    {
        FindObjectOfType<ChooseActivityUI>().EndChoosing(pResults);
    }

    #endregion

    #region Activity

    [PunRPC]
    public void SetSearchDatas(int pP, int pS, int pC, int[] pPercent)
    {
        ItemSearching.Ins.SetDatas(pP, pS, pC, pPercent);
    }

    [PunRPC]
    public void Communicated(bool pSucceed, string pResult)
    {
        if (ChooseActivity.Ins.SelectedActivity == 1)
            FindObjectOfType<EarthCommunicationUI>().PrintResult(pSucceed, pResult);
    }

    [PunRPC]
    public void TaskEnded()
    {
        GameManagerEx.Ins.TaskEnded();
    }

    #endregion

    #region Game

    [PunRPC]
    public void Intro(int pIndex)
    {
        IntroUI intro = FindObjectOfType<IntroUI>();
        switch (pIndex)
        {
            case 0: intro.Show(); break;
            case 1: intro.ShowJob(); break;
            case 2: intro.Hide(); break;
        }
    }

    [PunRPC]
    public void Hud(int pIndex, bool pPar)
    {
        SetHudInfo hud = FindObjectOfType<SetHudInfo>();
        switch (pIndex)
        {
            case 0: hud.OnOff(pPar); break;
            case 1: hud.UpdateInfo(); break;
        }
    }

    [PunRPC]
    public void Sleep(int pIndex)
    {
        SleepUI sleep = FindObjectOfType<SleepUI>();
        switch (pIndex)
        {
            case 0: sleep.Show(); break;
            case 1: sleep.EndSleep(); break;
        }
    }

    [PunRPC]
    public void Brief(int pIndex)
    {
        switch (pIndex)
        {
            case 0: BriefingManager.Ins.Init(); break;
            case 1: BriefingManager.Ins.StartBriefing(); break;
        }
    }

    [PunRPC]
    public void Activity(int pIndex)
    {
        switch (pIndex)
        {
            case 0: ChooseActivity.Ins.Init(); break;
            case 1:
                if (ChooseActivity.Ins.SelectedActivity == 0)
                {
                    ItemSearchingUI ui = FindObjectOfType<ItemSearchingUI>();
                    ui.SetValue(ItemSearching.Ins.GetSelectable(), ItemSearching.Ins.GetItems());
                    ui.ShowUI();
                }
                break;
            case 2: FindObjectOfType<Repairing>().ShowUI(); break;
            case 3: FindObjectOfType<ChooseActivityUI>().HideCanvas(); break;
        }
    }

    [PunRPC]
    public void Expel(int pIndex, int pProfileId, string pText)
    {
        ExpelUI ui = FindObjectOfType<ExpelUI>();
        switch (pIndex)
        {
            case 0: ui.Expel(pProfileId, pText); break;
            case 1: ui.Hide(); break;
        }
    }

    [PunRPC]
    public void Ending(int pIndex, string pEnding, string pSubScript)
    {
        EndingUI ending = FindObjectOfType<EndingUI>();
        switch (pIndex)
        {
            case 0: ending.ShowEnding(pEnding, pSubScript); break;
            case 1: ending.Hide(); break;
        }
    }

    #endregion
}
