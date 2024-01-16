using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int[] m_id;
    private Dictionary<ItemIndex, int> m_inventory;
    private bool m_isSpy;
    private Job m_job;
    private Dictionary<ItemIndex, int> m_usedItem;

    public int[] GetID() { return m_id; }
    public int GetID(int index) { return m_id[index]; }

    public int GetItem(ItemIndex index) { return m_inventory.ContainsKey(index) ? m_inventory[index] : 0; }
    public int AddItem(ItemIndex index, int value) { m_inventory[index] += value; return m_inventory[index]; }

    public int GetUsedItem(ItemIndex index) { return m_usedItem.ContainsKey(index) ? m_usedItem[index] : 0; }
    public void SetUsedItem(Dictionary<ItemIndex, int> usedItem) { m_usedItem = usedItem; }

    public Job GetJob() { return m_job; }
    public bool GetPosition() { return m_isSpy; }

    public Player(int[] pId, Job pJob, bool pIsSpy = false)
    {
        m_id = pId;
        m_job = pJob;
        m_isSpy = pIsSpy;

        m_inventory = new Dictionary<ItemIndex, int>();
    }
}