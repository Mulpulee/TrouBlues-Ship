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
        LobbyManager lobby = FindObjectOfType<LobbyManager>();
        lobby.AddPlayer(pItem);
    }

    [PunRPC]
    public void RemovePlayer(int pItem)
    {
        LobbyManager lobby = FindObjectOfType<LobbyManager>();
        lobby.RemovePlayer(pItem);
    }

    [PunRPC]
    public void Ready(int pItem)
    {
        LobbyManager lobby = FindObjectOfType<LobbyManager>();
        lobby.Ready(pItem);
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

    #endregion

    #region Vote

    [PunRPC]
    public void StartVote(VoteType pType, string pSubject, int[] pList = null)
    {
        GameObject.FindObjectOfType<VoteUI>().StartVote(pType, pSubject, pList);
    }

    [PunRPC]
    public void Vote(int pIndex)
    {
        VoteManager.Vote(pIndex);
    }

    [PunRPC]
    public void EndVote(int[] pResult)
    {
        GameObject.FindObjectOfType<VoteUI>().EndVote(pResult);
    }

    #endregion

    #region Sleep/Briefing

    [PunRPC]
    public void SendDeadPlayer(int pProfileId)
    {
        BriefingManager.Init();
        BriefingManager.DeadPlayer(pProfileId);
    }

    [PunRPC]
    public void UseSkill(JobType pType, string pScript)
    {
        BriefingManager.Init();
        BriefingManager.UseSkill(pType, pScript);
    }

    [PunRPC]
    public void UseScanner(int pProfileId)
    {
        BriefingManager.Init();
        BriefingManager.UseScanner(pProfileId);
    }

    [PunRPC]
    public void StartBriefing()
    {
        BriefingManager.StartBriefing();
    }

    #endregion

    #region ChooseActivity

    [PunRPC]
    public void StartChoosing()
    {
        GameObject.FindObjectOfType<ChooseActivityUI>().StartChoosing();
    }

    [PunRPC]
    public void Choose(int pIndex, int pProfileId)
    {
        ChooseActivity.AddMember(pIndex, pProfileId);
    }

    [PunRPC]
    public void EndChoose(int[][] pResults)
    {
        GameObject.FindObjectOfType<ChooseActivityUI>().EndChoosing(pResults);
    }

    #endregion

    #region Activity

    [PunRPC]
    public void Communicated(bool pSucceed, string pResult = null)
    {
        FindObjectOfType<EarthCommunicationUI>().PrintResult(pSucceed, pResult);
    }

    [PunRPC]
    public void TaskEnded()
    {
        FindObjectOfType<SystemTester>().TaskEnded();
    }

    #endregion
}
