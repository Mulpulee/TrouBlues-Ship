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
}
