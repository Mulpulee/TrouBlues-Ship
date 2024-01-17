using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTester : MonoBehaviour
{
    [SerializeField] private int m_players;
    [SerializeField] private int m_participants;

    [SerializeField] private SetHudInfo m_hudInfo;

    private void Start()
    {
        GameManagerEx gm = new GameManagerEx();
        
        gm.StartGame(m_players);

        m_hudInfo.UpdateInfo(CommonData.Medecines, GameManagerEx.Player.GetProfile());
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
