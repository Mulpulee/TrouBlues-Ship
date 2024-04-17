using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteUI : MonoBehaviour
{
    [SerializeField] private GameObject m_ui;

    [SerializeField] private GameObject m_buttonParent;
    [SerializeField] private GameObject m_pacParent;
    [SerializeField] private Button m_skip;
    [SerializeField] private GameObject m_skipFrame;

    [SerializeField] private Text m_subject;
    [SerializeField] private Text m_time;

    private Button[] m_buttons;
    private Button[] m_pacButtons;

    private bool m_isVoting = false;
    private VoteType m_type;
    private int m_count;

    public bool SpyMode = false;

    private void Start()
    {
        m_buttons = m_buttonParent.GetComponentsInChildren<Button>();
        m_pacButtons = m_pacParent.GetComponentsInChildren<Button>();
        foreach (var button in m_buttons) button.gameObject.SetActive(false);
        m_pacParent.SetActive(false);
    }

    private void Update()
    {
        if (!m_isVoting) return;

        m_time.text = Timer.Time.ToString("000");
    }

    public void StartVote(VoteType type, string subject, int[] list = null)
    {
        RoomDisplayer.Ins.SetRoom(RoomType.Meeting);
        if (SpyMode && !Player.This.IsSpy)
        {
            RoomDisplayer.Ins.Announce(Announcement.SpyActing);
            return;
        }
        RoomDisplayer.Ins.Announce(Announcement.None);

        m_ui.SetActive(true);
        m_time.text = Timer.Time.ToString("000");
        m_subject.text = subject;
        m_type = type;
        m_isVoting = true;
        foreach (var button in m_buttons) button.gameObject.SetActive(false);
        m_pacParent.SetActive(false);

        foreach (var button in m_buttons)
        { button.interactable = true; button.transform.GetChild(1).gameObject.SetActive(false); button.transform.GetChild(2).gameObject.SetActive(false); }
        foreach (var button in m_pacButtons)
        { button.interactable = true; button.transform.GetChild(1).gameObject.SetActive(false); button.transform.GetChild(0).gameObject.SetActive(false); }
        m_skip.interactable = true;
        m_skipFrame.SetActive(false);
        m_skip.transform.GetChild(1).gameObject.SetActive(false);

        switch (type)
        {
            case VoteType.Normal:
                m_count = list.Length;
                for (int i = 0; i < m_count; i++)
                {
                    m_buttons[i].gameObject.SetActive(true);
                    m_buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = CommonData.ProfileObjects[list[i]].profile;
                    m_buttons[i].transform.GetChild(3).GetComponent<Text>().text = CommonData.GetPlayer(list[i]).Name;
                }
                m_skip.gameObject.SetActive(true);
                break;
            case VoteType.ProsAndCons:
                m_count = 2;
                m_skip.gameObject.SetActive(false);
                m_pacParent.SetActive(true);
                break;
            case VoteType.Expel:
                m_count = list.Length;
                m_skip.gameObject.SetActive(false);
                for (int i = 0; i < m_count; i++)
                {
                    m_buttons[i].gameObject.SetActive(true);
                    m_buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = CommonData.ProfileObjects[list[i]].profile;
                    m_buttons[i].transform.GetChild(3).GetComponent<Text>().text = CommonData.GetPlayer(list[i]).Name;
                }
                break;
        }

        if (Player.This.IsDead || Player.This.IsLocked)
        {
            foreach (var button in m_buttons) button.interactable = false;
            foreach (var button in m_pacButtons) button.interactable = false;
            m_skip.interactable = false;
        }
    }

    public void Vote(int index)
    {
        PVHandler.pv.RPC("Vote", Photon.Pun.RpcTarget.MasterClient, m_type == VoteType.Expel ? index - 1 : index);

        foreach (var button in m_buttons) button.interactable = false;
        foreach (var button in m_pacButtons) button.interactable = false;
        m_skip.interactable = false;

        switch (m_type)
        {
            case VoteType.Normal:
                if (index == 0) m_skipFrame.SetActive(true);
                else m_buttons[index - 1].transform.GetChild(2).gameObject.SetActive(true);
                break;
            case VoteType.ProsAndCons:
                m_pacButtons[index].transform.GetChild(0).gameObject.SetActive(true);
                break;
            case VoteType.Expel:
                m_buttons[index - 1].transform.GetChild(2).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void EndVote(int[] result)
    {
        if (SpyMode && !Player.This.IsSpy) return;

        m_isVoting = false;
        foreach (var button in m_buttons) button.interactable = false;
        foreach (var button in m_pacButtons) button.interactable = false;
        m_skip.interactable = false;

        switch (m_type)
        {
            case VoteType.Normal:
                for (int i = 0; i < m_count; i++)
                {
                    m_buttons[i].transform.GetChild(1).gameObject.SetActive(true);
                    m_buttons[i].transform.GetChild(1).GetComponent<Text>().text = result[i + 1].ToString();
                }
                m_skip.transform.GetChild(1).gameObject.SetActive(true);
                m_skip.transform.GetChild(1).GetComponent<Text>().text = result[0].ToString();
                break;
            case VoteType.ProsAndCons:
                for (int i = 0; i < 2; i++)
                {
                    m_pacButtons[i].transform.GetChild(1).gameObject.SetActive(true);
                    m_pacButtons[i].transform.GetChild(1).GetComponent<Text>().text = result[i].ToString();
                }
                break;
            case VoteType.Expel:
                for (int i = 0; i < m_count; i++)
                {
                    m_buttons[i].transform.GetChild(1).gameObject.SetActive(true);
                    m_buttons[i].transform.GetChild(1).GetComponent<Text>().text = result[i].ToString();
                }
                break;
        }
    }

    public void Hide()
    {
        foreach (var button in m_buttons) button.gameObject.SetActive(false);
        m_pacParent.SetActive(false);
        m_ui.SetActive(false);
    }
}
