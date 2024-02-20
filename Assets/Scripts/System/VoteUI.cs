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

    private Button[] m_buttons;

    private void Start()
    {
        m_buttons = m_buttonParent.GetComponentsInChildren<Button>();
    }

    public void StartVote(VoteType type, string subject, Player[] list = null)
    {
        m_subject.text = subject;
        switch (type)
        {
            case VoteType.Normal:
                for (int i = 0; i < list.Length; i++)
                {
                    m_buttons[i].gameObject.SetActive(true);
                    m_buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = list[i].Profile;
                }
                break;
        }
    }

    public void Vote(int index, bool hasNovote = true)
    {
        PVHandler.pv.RPC("Vote", Photon.Pun.RpcTarget.MasterClient, index);
        
        foreach (var button in m_buttons) button.interactable = false;
        m_skip.interactable = false;

        if (hasNovote && index == 0) m_skipFrame.SetActive(true);
        else m_buttons[hasNovote ? index - 1 : index].transform.GetChild(2).gameObject.SetActive(true);
    }
}
