using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTester : MonoBehaviour
{
    private IntroUI intro;

    private void Start()
    {
        intro = FindObjectOfType<IntroUI>();

        intro.Show();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    if (!PVHandler.pv.IsMine) return;
        //    IdGenerator.ClearID();
        //    VoteManager.StartVote(VoteType.Normal, "테스트 투표입니다.", 5, 2, new int[4] { 3, 6, 9, 1 });
        //    Timer.SetTimer(20);
        //    StartCoroutine(ReduceTime());
        //}
    }

    private IEnumerator ReduceTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Timer.ReduceTimer();
        }
    }
}
