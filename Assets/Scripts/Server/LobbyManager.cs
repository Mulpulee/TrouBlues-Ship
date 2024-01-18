using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

    public void SetProfile()
    {
        Profile[] profiles = Resources.LoadAll<Profile>("ScriptableObject/Profile");
        m_profilesAsset = new List<Profile>();
        foreach (Profile profile in profiles)
        {
            m_profilesAsset.Add(profile);
        }

        m_profiles = new List<int>();
    }

    [PunRPC]
    public void SetPlayerData(int[] pList)
    {
        m_players = pList.ToList();
    }

    public void NewPlayer()
    {
        foreach (int i in m_players) m_profiles.RemoveAt(i);
        int random = Random.Range(0, m_profiles.Count);
        m_players.Add(m_profiles[random]);
        m_profiles.RemoveAt(random);
        MakePlayer();
    }

    public void MakePlayer()
    {
        foreach (var item in m_players)
        {
            LobbyPlayer lobbyPlayer = Instantiate<LobbyPlayer>(prefab, gridLayoutGroup.transform);
            lobbyPlayer.Setup(m_profilesAsset[item].nickName, m_profilesAsset[item].profile);
        }

        Debug.Log("橇府普 积己 己傍");
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
