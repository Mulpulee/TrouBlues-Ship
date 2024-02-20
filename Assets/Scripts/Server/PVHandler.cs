using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PVHandler : MonoBehaviour
{
    public static PhotonView pv;

    [PunRPC]
    public void AddPlayer(int item)
    {
        LobbyManager lobby = FindObjectOfType<LobbyManager>();
        lobby.AddPlayer(item);
    }

    [PunRPC]
    public void RemovePlayer(int item)
    {
        LobbyManager lobby = FindObjectOfType<LobbyManager>();
        lobby.RemovePlayer(item);
    }

    [PunRPC]
    public void Ready(int item)
    {
        LobbyManager lobby = FindObjectOfType<LobbyManager>();
        lobby.Ready(item);
    }

    [PunRPC]
    public void GameStart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    [PunRPC]
    public void SetPlayerList(int[] pPlayers, int[] pSpys, int pInfected, int[] pIDs)
    {
        CommonData.SetPlayerInfo(pPlayers, pSpys, pInfected, pIDs);
    }

    [PunRPC]
    public void StartVote(VoteType type, string subject, int[] list = null)
    {
        GameObject.FindObjectOfType<VoteUI>().StartVote(type, subject, list);
    }

    [PunRPC]
    public void Vote(int index)
    {
        VoteManager.Vote(index);
    }

    [PunRPC]
    public void EndVote(int[] result)
    {
        GameObject.FindObjectOfType<VoteUI>().EndVote(result);
    }

    [PunRPC]
    public void SetTimer(int time)
    {
        Timer.Time = time;
    }
}
