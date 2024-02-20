using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Invoke("GameStart", 2f);
    }

    public void GameStart()
    {
        Debug.Log("Invoke Success");
        count = 0;
        if (lobby.m_players != null)
        {
            foreach (int item in lobby.m_players)
            {
                if (lobby.LobbyPlayers[item].isReady) count++;
            }
            if (count == lobby.LobbyPlayers.Count)
            {
                SceneManager.LoadScene("SampleScene");
                PVHandler.pv.RPC("GameStart", RpcTarget.Others);
            }
        }
    }
}
