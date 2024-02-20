using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void MakePlayerList(List<Player> pList)
    {
        CommonData.MakePlayerInfo(pList);
    }

    [PunRPC]
    public void SetPlayerList(List<Player> pPlayers, List<Player> pSpys, Player pInfected)
    {
        CommonData.SetPlayerInfo(pPlayers, pSpys, pInfected);
    }

    [PunRPC]
    public void Vote(int index)
    {
        VoteManager.Vote(index);
    }

    [PunRPC]
    public void GetVoteResult(int[] result)
    {
        VoteManager.VoteResult = result;
    }
}
