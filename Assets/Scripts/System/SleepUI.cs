using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class SleepUI : MonoBehaviour
{
    [SerializeField] private GameObject m_background;
    [SerializeField] private GameObject m_mainChoosing;
    [SerializeField] private GameObject m_listCanvas;
    [SerializeField] private GameObject m_useornotCanvas;

    [SerializeField] private Image m_jobIcon;
    [SerializeField] private Text m_skillState;
    [SerializeField] private Text m_scannerCount;

    [SerializeField] private Text m_remainTime;

    private List<Player> players;
    private bool m_useSkill = false;
    private bool m_useScanner = false;
    private bool m_usingScanner = false;
    private int m_scannerTargetId;

    private void Update()
    {
        m_remainTime.text = Timer.Time.ToString("000");
    }

    public void Show()
    {
        RoomDisplayer.Ins.SetRoom(RoomType.Individual);
        if (Player.This.IsLocked) RoomDisplayer.Ins.Announce(Announcement.Banned);
        if (Player.This.IsDead || Player.This.IsLocked) return;

        m_background.SetActive(true);
        m_mainChoosing.transform.parent.gameObject.SetActive(true);
        m_skillState.GetComponentInParent<Button>().interactable = true;

        m_scannerCount.text = $"현재 {Player.This.GetItem(ItemIndex.Scanner)}개";
        if (Player.This.GetItem(ItemIndex.Scanner) == 0)
            m_scannerCount.GetComponentInParent<Button>().interactable = false;
        else
            m_scannerCount.GetComponentInParent<Button>().interactable = true;

        if (Player.This.PlayerJob.Type == JobType.None)
        {
            m_skillState.text = $"(직업 없음)";
            m_skillState.GetComponentInParent<Button>().interactable = false;
            return;
        }

        if (Player.This.PlayerJob.SpecialSkill.CoolCount != 0) Player.This.PlayerJob.SpecialSkill.CoolCount--;
        Job job = Player.This.PlayerJob;

        m_jobIcon.sprite = job.Icon;

        if (job.SpecialSkill.CoolCount == 0)
        {
            m_skillState.text = "(사용 가능)";
        }
        else
        {
            if (job.Type == JobType.Captain)
                m_skillState.text = $"(추방투표 이후 가능)";
            else
                m_skillState.text = $"({job.SpecialSkill.CoolCount}턴 남음)";
            m_skillState.GetComponentInParent<Button>().interactable = false;
        }
    }

    public void UseJobSkill()
    {
        SoundManager.Ins.PlaySfx("Game_button");
        Job job = Player.This.PlayerJob;

        switch (job.Type)
        {
            case JobType.Captain:
            case JobType.Medic:
                {
                    m_listCanvas.SetActive(true);
                    m_listCanvas.transform.GetChild(2).GetComponent<Text>().text = "직업 능력 사용";
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
                        Transform p = m_listCanvas.transform.GetChild(4).GetChild(i);
                        p.gameObject.SetActive(true);
                        p.GetChild(0).GetComponent<Image>().sprite = list[i].PlayerProfile;
                        p.GetChild(1).GetComponent<Text>().text = list[i].Name;
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
        if (m_usingScanner)
        {
            if (index == -1) m_useScanner = false;
            else
            {
                m_scannerTargetId = CommonData.Players[index].ProfileID;
                m_useScanner = true;
            }
        }
        else
        {
            if (index == -1) m_useSkill = false;
            else
            {
                Player.This.PlayerJob.SpecialSkill.Target = players[index];
                m_useSkill = true;
            }
        }
        SoundManager.Ins.PlaySfx("Game_button");
    }

    public void EndUsingScanner()
    {
        m_usingScanner = false;
        SoundManager.Ins.PlaySfx("Game_button");
    }

    public void UseSkill(bool value)
    {
        m_useSkill = value;
        SoundManager.Ins.PlaySfx("Game_button");
    }

    public void UseScanner()
    {
        m_listCanvas.SetActive(true);
        m_listCanvas.transform.GetChild(2).GetComponent<Text>().text = "스캐너 사용";
        m_listCanvas.transform.GetChild(3).GetComponent<Text>().text = "지목한 플레이어의 감염 여부를 확인합니다.";

        for (int i = 0; i < CommonData.Players.Count; i++)
        {
            Transform p = m_listCanvas.transform.GetChild(1).GetChild(i);
            p.gameObject.SetActive(true);
            p.GetChild(0).GetComponent<Image>().sprite = CommonData.Players[i].PlayerProfile;
        }

        m_usingScanner = true;
        SoundManager.Ins.PlaySfx("Game_button");
    }

    public void Skip()
    {
        SoundManager.Ins.PlaySfx("Game_button");
        m_mainChoosing.transform.parent.gameObject.SetActive(false);
        m_listCanvas.SetActive(false);
        m_useornotCanvas.SetActive(false);
        PVHandler.pv.RPC("TaskEnded", Photon.Pun.RpcTarget.MasterClient);
    }

    public void EndSleep()
    {
        if (Player.This.IsDead || Player.This.IsLocked) return;
        Job job = Player.This.PlayerJob;

        if (m_useSkill)
        {
            PVHandler.pv.RPC("UseSkill", Photon.Pun.RpcTarget.All, job.Type, job.SpecialSkill.GetResult());
            Player.This.PlayerJob.SpecialSkill.CoolCount = job.SpecialSkill.CoolDown + 1;
        }
        if (m_useScanner)
        {
            PVHandler.pv.RPC("UseScanner", Photon.Pun.RpcTarget.All, m_scannerTargetId);
            Player.This.AddItem(ItemIndex.Scanner, -1);
        }

        m_useSkill = false;
        m_useScanner = false;

        Hide();
    }

    public void Hide()
    {
        m_background.SetActive(false);
        m_mainChoosing.transform.parent.gameObject.SetActive(false);
        m_listCanvas.SetActive(false);
        m_useornotCanvas.SetActive(false);
    }
}
