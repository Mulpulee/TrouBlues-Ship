using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTester : MonoBehaviour
{
    [SerializeField] private int m_players;
    [SerializeField] private int m_participants;
    [SerializeField] private bool m_isSpy;

    [SerializeField] private ItemSearchingUI script;

    [SerializeField] private Sprite m_sprite;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!PVHandler.pv.IsMine) return;
            IdGenerator.ClearID();
            VoteManager.StartVote(VoteType.Normal, "테스트 투표입니다.", 5, 2, new int[4] { 1, 2, 3, 4 });
            Timer.SetTimer(20);
            StartCoroutine(ReduceTime());
        }
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
