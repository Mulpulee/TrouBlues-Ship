using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdGenerator
{
    private static List<int[]> m_previousIds;

    public static int[] GenerateID(bool isNew = false)
    {
        if (isNew)
        {
            if (m_previousIds == null) m_previousIds = new List<int[]>();
            m_previousIds.Clear();
        }

        int[] id = new int[5];
        for (int i = 0; i < 5; i++)
        {
            id[i] = Random.Range(0, 5);
        }

        if (m_previousIds.Contains(id))
        {
            return GenerateID();
        }
        else return id;
    }
}
