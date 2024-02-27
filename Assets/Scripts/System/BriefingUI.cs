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

    private List<News> m_news;

    public void StartBriefing(List<News> pNews)
    {
        m_news = pNews;
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

            yield return new WaitForSeconds(2f);

            switch (news.Type)
            {
                case NewsType.Nothing:
                    m_nothing.SetActive(true);
                    break;
                case NewsType.Death:
                    m_death.SetActive(true);
                    m_death.transform.GetChild(0).GetComponent<Image>().sprite = news.Icon;
                    m_death.transform.GetChild(1).GetComponent<Text>().text = news.Target;
                    m_death.transform.GetChild(2).GetComponent<Text>().text = news.Script;
                    break;
                case NewsType.JobSkill:
                    m_skill.SetActive(true);
                    m_skill.transform.GetChild(0).GetComponent<Image>().sprite = news.Icon;
                    m_skill.transform.GetChild(1).GetComponent<Text>().text = news.Target;
                    yield return new WaitForSeconds(2f);
                    m_skill.transform.GetChild(1).GetComponent<Text>().text = news.Script;
                    break;
                case NewsType.Scanner:
                    m_scanner.SetActive(true);
                    m_scanner.transform.GetChild(1).GetComponent<Text>().text = news.Target;
                    m_scanner.transform.GetChild(2).GetComponent<Text>().text = news.Script;
                    break;
            }

            yield return new WaitForSeconds(3f);
        }

        PVHandler.pv.RPC("TaskEnded", Photon.Pun.RpcTarget.MasterClient);
        m_nothing.SetActive(false);
        m_death.SetActive(false);
        m_skill.SetActive(false);
        m_scanner.SetActive(false);
    }
}
