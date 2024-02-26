using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemTester : MonoBehaviour
{
    private int m_endCount;
    private IntroUI intro;

    private void Awake()
    {

    }

    private void Start()
    {
        Timer.SetTimer(60);
        StartCoroutine(ReduceTime());

        m_endCount = 0;

        foreach (var p in CommonData.Players)
        {
            if (!p.IsDead) m_endCount++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (int i = 0; i < 10; i++) Timer.ReduceTimer();
        }
    }

    private IEnumerator ReduceTime(int pPoint = 0, Action pAction = null)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Timer.ReduceTimer();

            if (pAction != null && Timer.Time == pPoint)
            {
                pAction.Invoke();
            }
        }
    }

    public void TaskEnded()
    {
        m_endCount--;
    }
}
