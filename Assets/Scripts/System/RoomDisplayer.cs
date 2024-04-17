using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RoomType
{ 
    Individual,
    Meeting,
    Lounge,
    Machine,
    Storage,
    Cockpit,
    Orb
}

public enum Announcement
{
    None,
    Dead,
    Banned,
    SpyActing
}

public class RoomDisplayer : MonoBehaviour
{
    public static RoomDisplayer Ins { get; private set; }
    private void Awake() { Ins = this; }

    [SerializeField] private GameObject[] m_rooms;
    [SerializeField] private GameObject[] m_announcements;

    public void SetRoom(RoomType pRoom)
    {
        foreach (var room in m_rooms)
        {
            room.SetActive(false);
        }

        if (Player.This.IsDead && pRoom != RoomType.Meeting)
        {
            m_rooms[(int)RoomType.Individual].SetActive(true);
            m_rooms[(int)pRoom].transform.GetChild(1).GetComponent<Image>().sprite = Player.This.PlayerProfile;
            m_announcements[(int)Announcement.Dead].SetActive(true);
            return;
        }

        m_rooms[(int)pRoom].SetActive(true);

        if (pRoom == RoomType.Meeting)
        {
            Image[] players = m_rooms[(int)pRoom].transform.GetChild(1).GetComponentsInChildren<Image>(true);

            for (int i = 0; i < CommonData.Players.Count; i++)
            {
                players[CommonData.Players[i].ProfileID].gameObject.SetActive(true);
            }
        }
        else
        {
            m_rooms[(int)pRoom].transform.GetChild(1).GetComponent<Image>().sprite = Player.This.PlayerProfile;
        }
    }

    public void Announce(Announcement pType)
    {
        foreach (var item in m_announcements)
        {
            item.SetActive(false);
        }

        if (pType == Announcement.None) return;

        m_announcements[(int)pType].SetActive(true);
    }
}
