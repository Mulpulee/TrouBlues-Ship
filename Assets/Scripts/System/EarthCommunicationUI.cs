using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthCommunicationUI : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup m_layout;
    [SerializeField] private Text m_text;
    [SerializeField] private Image m_ID;

    private void Update()
    {
        m_layout.SetLayoutHorizontal();
    }

    public void PrintResult(bool pSucceed, int[] pResult)
    {
        RoomDisplayer.Ins.SetRoom(RoomType.Cockpit);
        m_layout.gameObject.SetActive(true);
        m_ID.gameObject.SetActive(true);
        SoundManager.Ins.PlaySfx("Communicate");
        StartCoroutine(PrintRoutine(pSucceed, pResult));
    }

    private IEnumerator PrintRoutine(bool pSucceed, int[] pResult)
    {
        bool isEnd = false;
        if (pSucceed)
        {
            string result;
            if (pResult[0] == -1) result = pResult.Length > 2 ?
                $"�������� ID {pResult[1]}�� ĭ ���ڰ� ������ ������ ������" :
                $"�������� ID���� ���� ���ڰ� ���Ե��� ������";
            else result = pResult.Length > 2 ?
                $"{pResult[0]} �������� ID {pResult[1]}�� ĭ ���ڰ� ������ ������ ������" :
                $"{pResult[0]} �������� ID���� ���� ���ڰ� ���Ե��� ������";
            m_ID.sprite = EarthCommunication.CharacterID[pResult[pResult.Length > 2 ? 2 : 1]];
            StartCoroutine(Printer(result, () => isEnd = true));
        }
        else
        {
            m_ID.gameObject.SetActive(false);
            StartCoroutine(Printer("(ġ����)....&#$....$..@$...#.>>$>....", () => isEnd = true));
            yield return new WaitUntil(() => isEnd);
            isEnd = false;
            StartCoroutine(Printer("������ �Ҿ����մϴ�.", () => isEnd = true));
        }
        yield return new WaitUntil(() => isEnd);

        m_layout.gameObject.SetActive(false);
        m_ID.gameObject.SetActive(false);
        PVHandler.pv.RPC("TaskEnded", Photon.Pun.RpcTarget.MasterClient);
    }

    private IEnumerator Printer(string pText, Action pNext = null)
    {
        m_text.text = "";
        foreach (var t in pText)
        {
            m_text.text += t;
            if (t != ' ') yield return new WaitForSeconds(0.1f);
        }
        m_text.text += ' ';
        m_layout.SetLayoutHorizontal();

        yield return new WaitUntil(() => Input.anyKeyDown || Input.GetMouseButtonDown(0));
        if (pNext != null) pNext.Invoke();
    }
}
