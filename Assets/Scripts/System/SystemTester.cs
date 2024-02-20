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

    private void Start()
    {
        if (!PVHandler.pv.IsMine) return;

        VoteManager.StartVote(VoteType.Normal, "테스트 투표입니다.", 4, 2,
            new Player[4] {
                new Player(IdGenerator.GenerateID(), new Job()),
                new Player(IdGenerator.GenerateID(), new Job()),
                new Player(IdGenerator.GenerateID(), new Job()),
                new Player(IdGenerator.GenerateID(), new Job())
        });
        Timer.SetTimer(20);
        StartCoroutine(ReduceTime());
    }

    private IEnumerator ReduceTime()
    {
        yield return new WaitForSeconds(1);
        Timer.ReduceTimer();
    }
}
