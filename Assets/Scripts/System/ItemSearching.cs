using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSearching
{
    private int m_provided;
    private int m_selectable;
    private int m_currentMap;
    private int[] m_percentage;

    public void Search(int pPlayer)
    {
        m_provided = DataManager.Data.ProvidedItems[pPlayer - 4];
        m_selectable = DataManager.Data.SelectableItems[pPlayer - 4];
        m_currentMap = Random.Range(0, 3);

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

    public int[] GetItems()
    {
        int[] items = new int[6];
        
        for (int i = 0; i < m_provided; i++)
        {
            int result = Random.Range(0, 100);
            if (result < m_percentage[0]) items[0]++;
            else if (result < m_percentage[0] + m_percentage[1]) items[1]++;
            else if (result < m_percentage[0] + m_percentage[1] + m_percentage[2]) items[2]++;
            else if (result < m_percentage[0] + m_percentage[1] + m_percentage[2] + m_percentage[3]) items[3]++;
            else if (result < m_percentage[0] + m_percentage[1] + m_percentage[2] + m_percentage[3] + m_percentage[4]) items[4]++;
            else items[5]++;
        }

        return items;
    }    
}
