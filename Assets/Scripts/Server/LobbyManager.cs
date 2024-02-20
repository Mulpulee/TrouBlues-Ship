using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviour
{
    [SerializeField] GameObject gridLayoutGroup;
    public LobbyPlayer prefab;
    private List<int> m_players = new List<int>();
    public List<int> Players
    {
        get { return m_players; }
    }
    List<Profile> m_profilesAsset;
    List<int> m_profiles;

    public int m_currentProfile;

    public LobbyPlayer mylobby; //
    public int playerID; //

    public void SetProfile()
    {
        Profile[] profiles = Resources.LoadAll<Profile>("ScriptableObject/Profile");
        m_profilesAsset = new List<Profile>();
        foreach (Profile profile in profiles)
        {
            m_profilesAsset.Add(profile);
        }

        m_profiles = new List<int>();
        for (int i = 0; i < 8; i++) m_profiles.Add(i);
    }

    public void AddPlayer(int item)
    {
        m_players.Add(item);
        LobbyPlayer lobbyPlayer = Instantiate<LobbyPlayer>(prefab, gridLayoutGroup.transform);
        lobbyPlayer.Setup(m_profilesAsset[item].nickName, m_profilesAsset[item].profile);
    }

    public void AddMyPlayer(int item)
    {
        m_players.Add(item);
        mylobby = Instantiate<LobbyPlayer>(prefab, gridLayoutGroup.transform);
        mylobby.Setup(m_profilesAsset[item].nickName, m_profilesAsset[item].profile);
    }

    public void NewPlayer()
    {
        foreach (int i in m_players) m_profiles.Remove(i);
        int random = Random.Range(0, 8 - m_players.Count);
        m_currentProfile = m_profiles[random];

        PVHandler.pv.RPC("AddPlayer", RpcTarget.OthersBuffered, m_currentProfile);

        playerID = m_currentProfile;
        AddMyPlayer(m_currentProfile);
    }

    public void RemovePlayer(int item)
    {
        m_players.Remove(item);
        Destroy(mylobby.gameObject);
    }

    public void RemoveMyPlayer()
    {
        m_players.Remove(playerID);
        Destroy(mylobby.gameObject);
    }
}

//public class LobbyPlayer : MonoBehaviour
//{
//    public Text nickName;
//    public Sprite profile;

//    public void Setup(string name, Sprite sprite)
//    {
//        nickName.text = name;
//        profile = sprite;
//    }
//}
