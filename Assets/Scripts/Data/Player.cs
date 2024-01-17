using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private Sprite m_profile;
    private int[] m_id;
    private Dictionary<ItemIndex, int> m_inventory;
    private bool m_isSpy = false;
    private bool m_isInfected = false;
    private Job m_job;
    private Dictionary<ItemIndex, int> m_usedItem;

    public Sprite GetProfile() { return m_profile; }
    public void SetProfile(Sprite sprite) { m_profile = sprite; }

    public int[] GetID() { return m_id; }
    public int GetID(int index) { return m_id[index]; }

    public Dictionary<ItemIndex, int> GetInventory() { return m_inventory; }
    public int GetItem(ItemIndex index) { return m_inventory[index]; }
    public int AddItem(ItemIndex index, int value) { m_inventory[index] += value; return m_inventory[index]; }

    public int GetUsedItem(ItemIndex index) { return m_usedItem.ContainsKey(index) ? m_usedItem[index] : 0; }
    public void SetUsedItem(Dictionary<ItemIndex, int> usedItem) { m_usedItem = usedItem; }

    public Job GetJob() { return m_job; }
    public bool GetPosition() { return m_isSpy; }

    public bool IsSpy() { return m_isSpy; }
    public void SetSpy() { m_isSpy = true; }

    public bool IsInfected() { return m_isInfected; }
    public void SetInfected() { m_isInfected = true; }

    public Player(int[] pId, Job pJob)
    {
        m_id = pId;
        m_job = pJob;

        m_inventory = new Dictionary<ItemIndex, int>();
        for (int i = 0; i < 5; i++)
        {
            m_inventory.Add((ItemIndex)i, 0);
        }
    }
}
