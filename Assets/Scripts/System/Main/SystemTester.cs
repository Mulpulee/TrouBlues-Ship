using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemTester : MonoBehaviour
{
    [SerializeField] private ExpelUI expel;

    private void Awake()
    {

    }

    private void Start()
    {
        expel.Expel(0, "8명이 비행을 계속합니다...");
    }

    private void Update()
    {

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
                break;
            }
        }
    }
}
