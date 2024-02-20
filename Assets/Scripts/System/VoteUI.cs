using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteUI : MonoBehaviour
{
    [SerializeField] private GameObject m_buttonParent;
    [SerializeField] private Button m_skip;
    [SerializeField] private GameObject m_skipFrame;

    [SerializeField] private Text m_subject;
    [SerializeField] private Text m_time;

    private Button[] m_buttons;

    private bool m_isVoting = false;
    private VoteType m_type;
    private int m_count;

    private void Start()
    {
        m_buttons = m_buttonParent.GetComponentsInChildren<Button>();
        foreach (var button in m_buttons) button.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!m_isVoting) return;

        m_time.text = Timer.Time.ToString("000");

        if (PVHandler.pv.IsMine && Timer.Time < 1)
        {
            m_time.text = "000";
            PVHandler.pv.RPC("EndVote", Photon.Pun.RpcTarget.All, VoteManager.VoteResult);
        }
    }

    public void StartVote(VoteType type, string subject, int[] list = null)
    {
        m_time.text = Timer.Time.ToString("000");
        m_subject.text = subject;
        m_type = type;
        m_count = list.Length;
        m_isVoting = true;
        switch (type)
        {
            case VoteType.Normal:
                for (int i = 0; i < m_count; i++)
                {
                    m_buttons[i].gameObject.SetActive(true);
                    //m_buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = list[i];
                }
                break;
        }
    }

    public void Vote(int index)
    {
        PVHandler.pv.RPC("Vote", Photon.Pun.RpcTarget.MasterClient, m_type == VoteType.Normal ? index : index - 1);
        
        foreach (var button in m_buttons) button.interactable = false;
        m_skip.interactable = false;

        if (index == 0) m_skipFrame.SetActive(true);
        else m_buttons[index - 1].transform.GetChild(2).gameObject.SetActive(true);
    }

    public void EndVote(int[] result)
    {
        m_isVoting = false;
        foreach (var button in m_buttons) button.interactable = false;
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
        }
    }
}
