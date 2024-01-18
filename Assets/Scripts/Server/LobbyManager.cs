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
    List<Profile> m_profiles;

    public void SetProfile()
    {
        Profile[] profiles = Resources.LoadAll<Profile>("ScriptableObject/Profile");
        m_profiles = new List<Profile>();
        foreach (Profile profile in profiles)
        {
            m_profiles.Add(profile);
        }
    }

    [PunRPC]
    public void SetPlayerData(int[] pList)
    {
        m_players = pList.ToList();

        MakePlayer();
    }

    public void NewPlayer()
    {
        int random = Random.Range(0, m_profiles.Count);
        m_players.Add(random);
        m_profiles.RemoveAt(random);
    }

    public void MakePlayer()
    {
        foreach (var item in m_players)
        {
            LobbyPlayer lobbyPlayer = Instantiate<LobbyPlayer>(prefab, gridLayoutGroup.transform);
            lobbyPlayer.Setup(m_profiles[item].nickName, m_profiles[item].profile);
        }

        Debug.Log("������ ���� ����");
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
