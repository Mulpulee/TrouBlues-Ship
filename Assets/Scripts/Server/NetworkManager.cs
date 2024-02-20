using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject panel_Server;
    public GameObject panel_Code;
    public GameObject panel_Lobby;

    public InputField codeInput;

    public Button btn_makeRoom;
    public Button btn_joinRoom;

    public Text text_code;
    private int code;

    private LobbyManager lobby;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);

        btn_makeRoom.interactable = false;
        btn_joinRoom.interactable = false;

        lobby = GetComponent<LobbyManager>();
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ���� �Ϸ�");

        btn_makeRoom.interactable = true;
        btn_joinRoom.interactable = true;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("���� ����");

        btn_makeRoom.interactable = false;
        btn_joinRoom.interactable = false;
    }

    public void CreateRoom()
    {
        code = UnityEngine.Random.Range(100000, 1000000);
        PhotonNetwork.CreateRoom(code.ToString(), new RoomOptions { MaxPlayers = 8});
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("�� ����� ����");
        text_code.text = code.ToString();
        panel_Server.SetActive(false);
        panel_Lobby.SetActive(true);

        if (PVHandler.pv == null) PVHandler.pv = gameObject.AddComponent<PhotonView>();
        PVHandler.pv.ViewID = PhotonNetwork.LocalPlayer.ActorNumber;
        if (gameObject.GetComponent<PVHandler>() == null) gameObject.AddComponent<PVHandler>();
    }

    public void JoinRoom() => PhotonNetwork.JoinRoom(codeInput.text);
    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� ����");
        panel_Server.SetActive(false);
        panel_Lobby.SetActive(true);
        panel_Code.SetActive(false);

        lobby.SetProfile();
        if (PVHandler.pv == null)
        {
            PVHandler.pv = gameObject.AddComponent<PhotonView>();
            PVHandler.pv.ViewID = PhotonNetwork.MasterClient.ActorNumber;
            gameObject.AddComponent<PVHandler>();
        }
        if (PVHandler.pv.IsMine)
        {
            lobby.NewPlayer();
            GameObject.Find("JoinButton").SetActive(false);
        }
        else
        {
            text_code.text = codeInput.text;
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message) => Debug.Log("�� ���� ����");

    public void LeaveRoom()
    {
        panel_Lobby.SetActive(false);
        lobby.RemovePlayer(lobby.playerID);
        PVHandler.pv.RPC("RemovePlayer", RpcTarget.OthersBuffered);
        PhotonNetwork.LeaveRoom();
    }
}
