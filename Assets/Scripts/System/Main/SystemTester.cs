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

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChooseActivity.Init();
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
