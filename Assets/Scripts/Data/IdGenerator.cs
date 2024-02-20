using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdGenerator
{
    private static List<int[]> m_previousIds;

    public static void ClearID()
    {
        if (m_previousIds == null) m_previousIds = new List<int[]>();
        m_previousIds.Clear();
    }

    public static int[] GenerateID()
    {
        int[] id = new int[5];
        for (int i = 0; i < 5; i++)
        {
            id[i] = Random.Range(0, 6);
        }

        if (m_previousIds.Contains(id))
        {
            return GenerateID();
        }
        else return id;
    }
}
