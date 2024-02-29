using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    LobbyManager lobby;
    int count;

    private void Start()
    {
        lobby = GameObject.FindObjectOfType<LobbyManager>();
    }

    public void StartBtn()
    {
        GameStart();
    }

    public void ClearReady()
    {
        foreach (int item in lobby.m_players)
        {
            lobby.LobbyPlayers[item].isReady = false;
        }
    }

    public void GameStart()
    {
        Debug.Log("Invoke Success");
        count = 0;
        if (lobby.m_players != null && lobby.LobbyPlayers.Count > 3)
        {
            foreach (int item in lobby.m_players)
            {
                if (lobby.LobbyPlayers[item].isReady) count++;
            }
            if (count == lobby.LobbyPlayers.Count)
            {
                PVHandler.pv.RPC("ClearReady", RpcTarget.All);
                CommonData.MakePlayerInfo(lobby.Players.ToArray());
            }
        }
    }
}
