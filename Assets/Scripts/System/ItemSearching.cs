using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSearching
{
    private int m_provided;
    private int m_selectable;
    private int m_currentMap;
    private int[] m_percentage;

    public void Search(int pPlayer) // 호스트만 굴림
    {
        m_provided = DataManager.Data.ProvidedItems[pPlayer - 4];
        m_selectable = DataManager.Data.SelectableItems[pPlayer - 4];
        m_currentMap = Random.Range(0, 2);

        m_percentage = DataManager.Data.SearchedItemPercentage[m_currentMap];
    }

    public void SetDatas(int pP, int pS, int pC, int[] pPercent)
    {
        m_provided = pP;
        m_selectable = pS;
        m_currentMap = pC;
        m_percentage = pPercent;
    }

    public int GetSelectable()
    {
        return m_selectable;
    }

    public ItemIndex[] GetItems()
    {
        ItemIndex[] items = new ItemIndex[m_provided];
        
        for (int i = 0; i < m_provided; i++)
        {
            int result = Random.Range(0, 99);
            if (result < m_percentage[0]) items[i] = ItemIndex.Scrap;
            else if (result < m_percentage[1]) items[i] = ItemIndex.Bolt;
            else if (result < m_percentage[2]) items[i] = ItemIndex.Wire;
            else if (result < m_percentage[3]) items[i] = ItemIndex.SuperConductor;
            else items[i] = ItemIndex.Scanner;
        }

        return items;
    }
}
