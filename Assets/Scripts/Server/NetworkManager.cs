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
        btn_makeRoom.interactable = false;
        btn_joinRoom.interactable = false;

        lobby = GetComponent<LobbyManager>();

        GameObject go = GameObject.Find("NetworkManager");

        if (go == null)
        {
            gameObject.name = "NetworkManager";
            DontDestroyOnLoad(gameObject);
        }
        if (go != null && go != gameObject)
        {
            Destroy(gameObject);
        }
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료");

        btn_makeRoom.interactable = true;
        btn_joinRoom.interactable = true;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("연결 끊김");

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
        Debug.Log("방 만들기 성공");
        text_code.text = code.ToString();
        panel_Server.SetActive(false);
        panel_Lobby.SetActive(true);

        if (PVHandler.pv == null) PVHandler.pv = gameObject.GetComponent<PhotonView>();
        if (gameObject.GetComponent<PVHandler>() == null) gameObject.AddComponent<PVHandler>();

        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        PVHandler.pv.TransferOwnership(PhotonNetwork.LocalPlayer);
    }

    public void JoinRoom() => PhotonNetwork.JoinRoom(codeInput.text);
    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가 성공");
        panel_Server.SetActive(false);
        panel_Lobby.SetActive(true);
        panel_Code.SetActive(false);

        lobby.SetProfile();
        if (PVHandler.pv == null)
        {
            PVHandler.pv = gameObject.GetComponent<PhotonView>();
            gameObject.AddComponent<PVHandler>();
        }
        if (PhotonNetwork.PlayerList[0] == PhotonNetwork.LocalPlayer)
        {
            lobby.NewPlayer();
            GameObject.Find("JoinButton").SetActive(false);
        }
        else
        {
            text_code.text = codeInput.text;
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message) => Debug.Log("방 참가 실패");

    public void LeaveRoom()
    {
        panel_Lobby.SetActive(false);
        PVHandler.pv.RPC("RemovePlayer", RpcTarget.AllBuffered, lobby.playerID);
        foreach (Transform player in lobby.gridLayoutGroup.transform)
        {
            Destroy(player.gameObject);
        }
        lobby.ResetList();
        if (PVHandler.pv.IsMine && PhotonNetwork.PlayerList.Length > 1)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);
            PVHandler.pv.TransferOwnership(PhotonNetwork.PlayerListOthers[0]);
        }
        else if (!PVHandler.pv.IsMine)
            PVHandler.pv.TransferOwnership(PVHandler.pv.ViewID);
        PhotonNetwork.LeaveRoom();
    }
}
