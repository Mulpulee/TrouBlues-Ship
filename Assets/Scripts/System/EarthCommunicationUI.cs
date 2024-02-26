using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthCommunicationUI : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup m_layout;
    [SerializeField] private Text m_text;

    public void PrintResult(bool pSucceed, string pResult)
    {
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
            StartCoroutine(Printer("(ġ����)....&#$....$..@$...#.>>$>....", () => isEnd = true));
            yield return new WaitUntil(() => isEnd);
            isEnd = false;
            StartCoroutine(Printer("������ �Ҿ����մϴ�.", () => isEnd = true));
        }
        yield return new WaitUntil(() => isEnd);

        m_layout.gameObject.SetActive(false);
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
