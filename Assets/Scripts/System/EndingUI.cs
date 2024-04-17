using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private GameObject m_canvas;
    [SerializeField] private Text m_ending;
    [SerializeField] private Text m_subscript;

    public void ShowEnding(string pEnding, string pSubScript)
    {
        SoundManager.Ins.StopBgm();
        RoomDisplayer.Ins.Announce(Announcement.None);
        m_ending.text = pEnding;
        m_subscript.text = pSubScript;

        m_canvas.SetActive(true);
    }

    public void Hide()
    {
        m_canvas.SetActive(false);
    }
}
