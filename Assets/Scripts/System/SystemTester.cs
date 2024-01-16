using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTester : MonoBehaviour
{
    [SerializeField] private ItemSearchingUI script;

    private void Start()
    {
        ItemSearching searching = new ItemSearching();

        searching.Search(4);

        script.SetValue(searching.GetSelectable(), searching.GetItems());
        script.ShowUI();
    }
}
