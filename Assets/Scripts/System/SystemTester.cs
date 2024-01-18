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
        ItemSearching searching = new ItemSearching();
        searching.Search(4);
        script.SetValue(searching.GetSelectable(), searching.GetItems());
        script.ShowUI();
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
