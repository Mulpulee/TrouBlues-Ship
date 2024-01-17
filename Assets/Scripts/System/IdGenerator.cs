using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdGenerator : MonoBehaviour
{
    private List<int[]> m_previousIds;

    public int[] GenerateID(bool isNew = false)
    {
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