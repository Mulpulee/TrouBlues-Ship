using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthCommunicationUI : MonoBehaviour
{
    [SerializeField] private GameObject m_room;

    [SerializeField] private HorizontalLayoutGroup m_layout;
    [SerializeField] private Text m_text;

    public void PrintResult(bool pSucceed, string pResult)
    {
        m_room.SetActive(true);
        m_layout.gameObject.SetActive(true);
        StartCoroutine(PrintRoutine(pSucceed, pResult));
    }

    private IEnumerator PrintRoutine(bool pSucceed, string pResult)
    {
        bool isEnd = false;
        if (pSucceed)
        {
            StartCoroutine(Printer(pResult, () => isEnd = true));
        }
        else
        {
            StartCoroutine(Printer("(치지직)....&#$....$..@$...#.>>$>....", () => isEnd = true));
            yield return new WaitUntil(() => isEnd);
            isEnd = false;
            StartCoroutine(Printer("연결이 불안정합니다.", () => isEnd = true));
        }
        yield return new WaitUntil(() => isEnd);

        m_room.SetActive(false);
        m_layout.gameObject.SetActive(false);
        PVHandler.pv.RPC("TaskEnded", Photon.Pun.RpcTarget.MasterClient);
    }

    private IEnumerator Printer(string pText, Action pNext = null)
    {
        m_text.text = "";
        foreach (var t in pText)
        {
            m_text.text += t;
            m_layout.SetLayoutHorizontal();
            if (t != ' ') yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitUntil(() => Input.anyKeyDown || Input.GetMouseButtonDown(0));
        if (pNext != null) pNext.Invoke();
    }
}
