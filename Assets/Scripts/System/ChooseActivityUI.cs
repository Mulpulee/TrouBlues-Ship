using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseActivityUI : MonoBehaviour
{
    [SerializeField] private Button[] m_butttons;

    [SerializeField] private GameObject m_resultCanvas;
    [SerializeField] private Transform[] m_items;

    public void StartChoosing()
    {
        if (Player.This.IsLocked)
        {
            RoomDisplayer.Ins.Announce(Announcement.Banned);
        }
        if (Player.This.IsDead || Player.This.IsLocked)
        {
            RoomDisplayer.Ins.SetRoom(RoomType.Individual);
            ChooseActivity.Ins.SelectedActivity = -1;
            return;
        }

        GetComponent<Canvas>().enabled = true;
        m_resultCanvas.SetActive(false);
        foreach (var item in m_butttons)
        {
            item.interactable = true;
            item.transform.GetChild(2).gameObject.SetActive(false);
        }
        foreach (var item in m_items)
        {
            for (int i = 0; i < 8; i++)
            {
                item.GetChild(1 + (i / 4)).GetChild(i % 4).GetComponent<Image>().color = Color.clear;
            }
        }
    }

    public void Choose(int index)
    {
        foreach (var item in m_butttons) item.interactable = false;
        m_butttons[index].transform.GetChild(2).gameObject.SetActive(true);

        SoundManager.Ins.PlaySfx("Game_button");
        PVHandler.pv.RPC("Choose", Photon.Pun.RpcTarget.MasterClient, index, Player.This.ProfileID);
        ChooseActivity.Ins.SelectedActivity = index;
    }

    public void EndChoosing(int[][] result)
    {
        m_resultCanvas.SetActive(true);

        for (int i = 0; i < result.Length; i++)
        {
            for (int j = 0; j < result[i].Length; j++)
            {
                Image img = m_items[i].GetChild(1 + (j / 4)).GetChild(j % 4).GetComponent<Image>();
                img.color = Color.white;
                img.sprite = CommonData.ProfileObjects[result[i][j]].profile;
            }
        }
    }

    public void HideCanvas()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
