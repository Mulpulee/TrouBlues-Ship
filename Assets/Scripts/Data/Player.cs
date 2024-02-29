using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private static Player m_this;
    public static Player This
    {
        get { return m_this; }
        set { m_this = value; }
    }

    private Sprite m_profile;
    private int m_profileID;
    private string m_name;
    private int[] m_id;
    private Dictionary<ItemIndex, int> m_inventory;
    private bool m_isSpy = false;
    private bool m_isInfected = false;
    private bool m_isDead = false;
    private bool m_isLocked = false;
    private Job m_job;
    private Dictionary<ItemIndex, int> m_usedItem;

    public int ProfileID { set { m_profileID = value; } get { return m_profileID; } }
    public Sprite PlayerProfile { set { m_profile = value; } get { return m_profile; } }

    public string Name { set { m_name = value; } get { return m_name; } }

    public int[] ID { set { m_id = value; } get { return m_id; } }
    public int GetID(int index) { return m_id[index]; }

    public Dictionary<ItemIndex, int> GetInventory() { return m_inventory; }
    public int GetItem(ItemIndex index) { return m_inventory[index]; }
    public int AddItem(ItemIndex index, int value) { m_inventory[index] += value; return m_inventory[index]; }

    public int GetUsedItem(ItemIndex index) { return m_usedItem.ContainsKey(index) ? m_usedItem[index] : 0; }
    public void SetUsedItem(Dictionary<ItemIndex, int> usedItem) { m_usedItem = usedItem; }

    public Job PlayerJob { set { m_job = value; } get { return m_job; } }
    public bool GetPosition() { return m_isSpy; }

    public bool IsSpy { get { return m_isSpy; } }
    public void SetSpy() { m_isSpy = true; }

    public bool IsInfected { get { return m_isInfected; } }
    public void SetInfected(bool infected = true) { m_isInfected = infected; }

    public bool IsDead { get { return m_isDead; } }
    public void SetDead() { m_isDead = true; }

    public bool IsLocked { get { return m_isLocked; } set { m_isLocked = value; } }

    public Player()
    {
        m_inventory = new Dictionary<ItemIndex, int>();
        for (int i = 0; i < 5; i++)
        {
            m_inventory.Add((ItemIndex)i, 0);
        }
    }
}
