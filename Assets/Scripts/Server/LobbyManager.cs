using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviour
{
    [SerializeField] public GameObject gridLayoutGroup;
    public LobbyPlayer prefab;
    public List<int> m_players = new List<int>();
    public Dictionary<int, string> m_playerNames = new Dictionary<int, string>();
    public List<int> Players
    {
        get { return m_players; }
    }
    List<Profile> m_profilesAsset;
    List<int> m_profiles;
    public int m_currentProfile;
    public int playerID;
    public Dictionary<int, LobbyPlayer> LobbyPlayers = new Dictionary<int, LobbyPlayer>();

    public void SetProfile()
    {
        Profile[] profiles = Resources.LoadAll<Profile>("ScriptableObject/Profile");
        profiles = profiles.OrderBy(p => p.ID).ToArray();
        m_profilesAsset = new List<Profile>();
        foreach (Profile profile in profiles)
        {
            m_profilesAsset.Add(profile);
        }

        m_profiles = new List<int>();
        for (int i = 0; i < 8; i++) m_profiles.Add(i);
    }

    public void AddPlayer(int pItem, string pName)
    {
        m_players.Add(pItem);
        m_playerNames.Add(pItem, pName);
        LobbyPlayer lobbyPlayer = Instantiate<LobbyPlayer>(prefab, gridLayoutGroup.transform);
        if (pName == "") lobbyPlayer.Setup(m_profilesAsset[pItem].nickName, m_profilesAsset[pItem].profile);
        else lobbyPlayer.Setup(pName, m_profilesAsset[pItem].profile);
        LobbyPlayers.Add(pItem, lobbyPlayer);
    }

    public void NewPlayer()
    {
        foreach (int i in m_players) m_profiles.Remove(i);
        int random = Random.Range(0, 8 - m_players.Count);
        m_currentProfile = m_profiles[random];

        PVHandler.pv.RPC("AddPlayer", RpcTarget.OthersBuffered, m_currentProfile, PlayerPrefs.GetString("PlayerName"));

        playerID = m_currentProfile;
        CommonData.ProfileID = playerID;
        AddPlayer(m_currentProfile, PlayerPrefs.GetString("PlayerName"));
    }

    public void RemovePlayer(int item)
    {
        if (LobbyPlayers.ContainsKey(item))
        {
            Destroy(LobbyPlayers[item].gameObject);
            LobbyPlayers.Remove(item);
        }
        m_players.Remove(item);
        m_playerNames.Remove(item);
    }

    public void ResetList()
    {
        LobbyPlayers.Clear();
        m_players.Clear();
    }

    public void Ready(int item)
    {
        LobbyPlayers[item].isReady = !LobbyPlayers[item].isReady;
    }

    public void ReadyBnt()
    {
        PVHandler.pv.RPC("Ready", RpcTarget.AllBuffered, playerID);
    }
}
