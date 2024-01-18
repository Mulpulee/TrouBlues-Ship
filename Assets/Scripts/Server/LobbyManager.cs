using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviour
{
    [SerializeField] GameObject gridLayoutGroup;
    public LobbyPlayer prefab;
    private List<LobbyPlayer> players = new List<LobbyPlayer>();
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
    public void NewPlayer()
    {
        int random = Random.Range(0, m_profiles.Count);
        LobbyPlayer lobbyPlayer = Instantiate<LobbyPlayer>(prefab, gridLayoutGroup.transform);
        lobbyPlayer.Setup(m_profiles[random].name, m_profiles[random].profile);

        Debug.Log("프리팹 생성 성공");

        players.Add(lobbyPlayer);

        m_profiles.RemoveAt(random);
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
