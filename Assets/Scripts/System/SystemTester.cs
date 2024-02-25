using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemTester : MonoBehaviour
{
    private IntroUI intro;

    private void Awake()
    {
        CommonData.ProfileID = 0;
        CommonData.MakePlayerInfo(new int[5] { 0, 1, 2, 3, 4 });
    }

    private void Start()
    {
        Timer.SetTimer(60);
        StartCoroutine(ReduceTime());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (int i = 0; i < 10; i++) Timer.ReduceTimer();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SleepScene");
            BriefingManager.Init(true);
        }
    }

    private IEnumerator ReduceTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Timer.ReduceTimer();

            if (Timer.Time == 59)
            {
                Player.This.AddItem(ItemIndex.Scanner, 1);
                FindObjectOfType<SleepUI>().Show();
            }
        }
    }
}
