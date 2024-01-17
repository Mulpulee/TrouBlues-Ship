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
        GameManagerEx gm = new GameManagerEx();
        
        gm.StartGame(m_players);
        if (m_isSpy) GameManagerEx.Player.SetSpy();

        CommonData.RepairProgress[0] = 2;
        CommonData.RepairProgress[1] = 6;
        CommonData.RepairProgress[2] = 3;

        GameManagerEx.Player.AddItem((ItemIndex)0, 5);
        GameManagerEx.Player.AddItem((ItemIndex)1, 1);
        GameManagerEx.Player.AddItem((ItemIndex)2, 4);

        repairing.ShowUI();
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
