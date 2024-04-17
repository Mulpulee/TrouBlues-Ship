using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BriefingUI : MonoBehaviour
{
    [SerializeField] private GameObject m_nothing;
    [SerializeField] private GameObject m_death;
    [SerializeField] private GameObject m_skill;
    [SerializeField] private GameObject m_scanner;
    [SerializeField] private GameObject m_vaccine;

    private List<News> m_news;

    public void StartBriefing(List<News> pNews)
    {
        m_news = pNews;
        RoomDisplayer.Ins.SetRoom(RoomType.Meeting);
        RoomDisplayer.Ins.Announce(Announcement.None);
        StartCoroutine(BriefingCoroutine());
    }

    private IEnumerator BriefingCoroutine()
    {
        foreach (News news in m_news)
        {
            m_nothing.SetActive(false);
            m_death.SetActive(false);
            m_skill.SetActive(false);
            m_scanner.SetActive(false);
            m_vaccine.SetActive(false);

            yield return new WaitForSeconds(2f);

            switch (news.Type)
            {
                case NewsType.Nothing:
                    m_nothing.SetActive(true);
                    SoundManager.Ins.PlaySfx("Brief_positive");
                    break;
                case NewsType.Death:
                    m_death.SetActive(true);
                    m_death.transform.GetChild(0).GetComponent<Image>().sprite = news.Icon;
                    m_death.transform.GetChild(1).GetComponent<Text>().text = news.Target;
                    m_death.transform.GetChild(2).GetComponent<Text>().text = news.Script;
                    SoundManager.Ins.PlaySfx("Brief_negative");
                    break;
                case NewsType.JobSkill:
                    m_skill.SetActive(true);
                    m_skill.transform.GetChild(0).GetComponent<Image>().sprite = news.Icon;
                    m_skill.transform.GetChild(1).GetComponent<Text>().text = news.Target;
                    m_skill.transform.GetChild(2).gameObject.SetActive(false);
                    yield return new WaitForSeconds(2f);
                    SoundManager.Ins.PlaySfx("Brief_positive");

                    string[] s = news.Script.Split('`');
                    if (s.Length > 1)
                    {
                        m_skill.transform.GetChild(1).GetComponent<Text>().text = s[0];
                        m_skill.transform.GetChild(2).gameObject.SetActive(true);
                        m_skill.transform.GetChild(2).GetComponent<Image>().sprite = EarthCommunication.CharacterID[Int32.Parse(s[1])];
                    }
                    else m_skill.transform.GetChild(1).GetComponent<Text>().text = news.Script;
                    SoundManager.Ins.PlaySfx("Game_button");
                    break;
                case NewsType.Scanner:
                    m_scanner.SetActive(true);
                    m_scanner.transform.GetChild(1).GetComponent<Text>().text = news.Target;
                    m_scanner.transform.GetChild(2).GetComponent<Text>().text = news.Script;
                    SoundManager.Ins.PlaySfx("Game_button");
                    break;
                case NewsType.Vaccine:
                    m_vaccine.SetActive(true);
                    m_vaccine.transform.GetChild(1).GetComponent<Image>().sprite = news.Icon;
                    m_vaccine.transform.GetChild(1).GetComponent<Text>().text = news.Target;
                    m_vaccine.transform.GetChild(2).GetComponent<Text>().text = news.Script;
                    SoundManager.Ins.PlaySfx("Game_button");
                    break;
            }

            yield return new WaitForSeconds(3f);
        }

        PVHandler.pv.RPC("TaskEnded", Photon.Pun.RpcTarget.MasterClient);
        m_nothing.SetActive(false);
        m_death.SetActive(false);
        m_skill.SetActive(false);
        m_scanner.SetActive(false);
        m_vaccine.SetActive(false);
    }
}
