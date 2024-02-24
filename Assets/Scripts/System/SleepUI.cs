using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepUI : MonoBehaviour
{
    [SerializeField] private GameObject m_mainChoosing;
    [SerializeField] private GameObject m_listCanvas;
    [SerializeField] private GameObject m_useornotCanvas;

    [SerializeField] private Image m_jobIcon;
    [SerializeField] private Text m_skillState;
    [SerializeField] private Text m_scannerCount;

    [SerializeField] private Text m_remainTime;

    private List<Player> players;
    private bool m_useSkill = false;

    private void Update()
    {
        m_remainTime.text = Timer.Time.ToString("00");
    }

    public void Show()
    {
        m_mainChoosing.transform.parent.gameObject.SetActive(true);

        Job job = Player.This.PlayerJob;

        m_jobIcon.sprite = job.Icon;

        if (job.SpecialSkill.CoolCount == 0)
        {
            m_skillState.text = "(��� ����)";
        }
        else
        {
            m_skillState.text = $"{job.SpecialSkill.CoolCount}�� ����";
            m_skillState.GetComponentInParent<Button>().enabled = false;
        }

        m_scannerCount.text = $"���� {Player.This.GetItem(ItemIndex.Scanner)}��";
        if (Player.This.GetItem(ItemIndex.Scanner) == 0) m_scannerCount.GetComponentInParent<Button>().enabled = false;
    }

    public void UseJobSkill()
    {
        Job job = Player.This.PlayerJob;

        switch (job.Type)
        {
            case JobType.Captain:
            case JobType.Medic:
                {
                    m_listCanvas.SetActive(true);
                    m_listCanvas.transform.GetChild(3).GetComponent<Text>().text = job.SpecialSkill.Explanation;

                    List<Player> list = CommonData.Players;
                    if (job.Type == JobType.Medic)
                    {
                        foreach (Player player in list)
                        {
                            if (player.IsDead) list.Remove(player);
                        }
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        Transform p = m_listCanvas.transform.GetChild(1).GetChild(i);
                        p.gameObject.SetActive(true);
                        p.GetChild(0).GetComponent<Image>().sprite = list[i].PlayerProfile;
                    }

                    players = list;

                    break;
                }
            case JobType.Engineer:
            case JobType.Janitor:
            case JobType.Controller:
                {
                    m_useornotCanvas.SetActive(true);
                    m_useornotCanvas.transform.GetChild(2).GetComponent<Text>().text = job.SpecialSkill.Explanation;
                }
                break;
        }
    }

    public void SelectTarget(int index)
    {
        if (index == -1) m_useSkill = false;
        else
        {
            Player.This.PlayerJob.SpecialSkill.Target = players[index];
            m_useSkill = true;
        }
    }

    public void UseSkill(bool value)
    {
        m_useSkill = value;
    }

    public void EndSleep()
    {
        Job job = Player.This.PlayerJob;

        if (m_useSkill)
        {
            PVHandler.pv.RPC("UseSkill", Photon.Pun.RpcTarget.All, job.Type, job.SpecialSkill.GetResult());
            if (job.Type == JobType.Medic)
            {
                PVHandler.pv.RPC("UseScanner", Photon.Pun.RpcTarget.All, job.SpecialSkill.Target.ProfileID);
            }
        }
    }
}
