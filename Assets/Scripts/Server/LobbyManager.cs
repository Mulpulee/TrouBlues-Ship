using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LobbyPlayer
{
    public Sprite profile;
    public Text nickName;
}

public class LobbyManager : MonoBehaviour
{
    [SerializeField] public LobbyPlayer[] lobbyPlayers;
    public GameObject l_Player;
    private List<GameObject> players = new List<GameObject>();

    public void NewPlayer()
    {
        int random = Random.Range(0, 8);
    }

}
