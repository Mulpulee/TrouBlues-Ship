using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    [SerializeField] private GameObject m_crueCanvas;
    [SerializeField] private GameObject m_spyCanvas;
    [SerializeField] private GameObject m_jobPanel;

    [SerializeField] private Image[] m_spyProfiles;
    [SerializeField] private Text m_spyText;

    public void Show()
    {
        Debug.Log("player");
        Player p = Player.This;

        if (p.IsSpy)
        {
            string t = "";
            m_spyCanvas.SetActive(true);
            Debug.Log("spy");

            for (int i = 0; i < CommonData.Spys.Count; i++)
            {
                m_spyProfiles[i].gameObject.SetActive(true);
                m_spyProfiles[i].sprite = CommonData.Spys[i].PlayerProfile;

                t += $"{CommonData.Spys[i].Name}, ";
            }

            m_spyText.text = t.Substring(0, t.Length - 2);
        }
        else
        {
            m_crueCanvas.SetActive(true);
            Debug.Log("crue ");

            Image[] players = m_crueCanvas.transform.GetChild(1).GetComponentsInChildren<Image>(true);

            for (int i = 0; i < CommonData.Players.Count; i++)
            {
                players[CommonData.Players[i].ProfileID].gameObject.SetActive(true);
            }
        }

        Invoke("ShowJob", 3f);
    }

    public void ShowJob()
    {
        m_jobPanel.SetActive(true);

        m_jobPanel.transform.GetChild(0).GetComponent<Image>().sprite = Player.This.PlayerJob.Icon;
        m_jobPanel.transform.GetChild(1).GetComponent<Text>().text = $"당신은 {Player.This.PlayerJob.Name}입니다.";
    }

    public void Hide()
    {
        m_crueCanvas.SetActive(false);
        m_spyCanvas.SetActive(false);
        m_jobPanel.SetActive(false);
    }
}
