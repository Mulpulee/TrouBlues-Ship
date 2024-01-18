using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Repairing : MonoBehaviour
{
    [SerializeField] private Text[] m_usingItemsText;
    [SerializeField] private Text[] m_progressText;
    [SerializeField] private Text[] m_inventoryText;
    [SerializeField] private Image[] m_changeUse;

    private int[] m_usingItems;
    private int[] m_inventory;
    private bool[] m_minus;

    public void ShowUI()
    {
        gameObject.SetActive(true);

        m_usingItems = new int[3];
        m_inventory = new int[3];
        m_minus = new bool[3] { false, false, false };

        foreach (var t in m_usingItemsText) t.text = "0";
        for (int i = 0; i < 3; i++) m_progressText[i].text
                = $"{CommonData.RepairProgress[i]}/{DataManager.Data.ShipRequirements[CommonData.Players.Count - 4][i]}";
        for (int i = 0; i < 3; i++)
        {
            m_inventory[i] = GameManagerEx.Player.GetInventory()[(ItemIndex)i];
            m_inventoryText[i].text = m_inventory[i].ToString();
        }

        if (!GameManagerEx.Player.IsSpy()) m_changeUse[0].transform.parent.gameObject.SetActive(false);
    }

    public void SetText(int index)
    {
        m_usingItemsText[index].text = $"{(m_minus[index] ? "-" : "")}{m_usingItems[index]}";
        m_inventoryText[index].text = m_inventory[index].ToString();
    }

    public void UseItem(int index)
    {
        if (m_inventory[index] > 0)
        {
            m_usingItems[index]++;
            m_inventory[index]--;

            SetText(index);
        }
    }

    public void ReturnItem(int index)
    {
        if (m_usingItems[index] > 0)
        {
            m_usingItems[index]--;
            m_inventory[index]++;

            SetText(index);
        }
    }

    public void ChangeUse(int index)
    {
        m_minus[index] = !m_minus[index];

        m_changeUse[index].color = m_minus[index] ? Color.red : Color.white;

        SetText(index);
    }

    public void EndSelection()
    {
        for (int i = 0; i < 3; i++)
        {
            GameManagerEx.Player.AddItem((ItemIndex)i, -m_usingItems[i]);
            if (!GameManagerEx.Player.IsInfected()) CommonData.RepairProgress[i] += m_usingItems[i];
        }
    }
}