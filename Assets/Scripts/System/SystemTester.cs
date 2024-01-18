using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTester : MonoBehaviour
{
    [SerializeField] private int m_players;
    [SerializeField] private int m_participants;
    [SerializeField] private bool m_isSpy;

    [SerializeField] private Repairing repairing;

    private void Start()
    {
        PVHandler.pv.RPC("dd", RpcTarget.All);

    }

    private void PrintArrayInLine<T>(T[] pArray)
    {
        string temp = "";

        foreach (T t in pArray)
        {
            temp = $"{temp} {t}";
        }

        Debug.Log(temp);
    }
}
