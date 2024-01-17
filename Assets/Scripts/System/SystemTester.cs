using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTester : MonoBehaviour
{
    [SerializeField] private int m_players;
    [SerializeField] private int m_participants;

    private void Start()
    {
        GameManagerEx gm = new GameManagerEx();

        gm.StartGame(m_players);

        Debug.Log("Players");
        foreach (var item in gm.m_players) PrintArrayInLine(item.GetID());
        Debug.Log("Spys");
        foreach (var item in gm.m_spys) PrintArrayInLine(item.GetID());
        Debug.Log("Infected");
        PrintArrayInLine(gm.m_infected.GetID());

        EarthCommunication ec = new EarthCommunication();

        ec.SetInfo(gm.m_spys);
        ec.StartCommunication(m_participants);
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
