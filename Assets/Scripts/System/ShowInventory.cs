using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInventory : MonoBehaviour
{
    [SerializeField] private GameObject m_inventoryObject;
    [SerializeField] private GameObject[] m_itemObject;

    public void Show(Dictionary<ItemIndex, int> pInventory)
    {
        m_inventoryObject.SetActive(true);

        for (int i = 0; i < m_itemObject.Length; i++)
        {
            if (pInventory[(ItemIndex)i] == 0)
            {
                m_itemObject[i].SetActive(false);
            }
            else
            {
                m_itemObject[i].SetActive(true);
                m_itemObject[i].transform.GetChild(1).GetComponent<Text>().text = pInventory[(ItemIndex)i].ToString();
            }
        }
    }

    public void Hide()
    {
        m_inventoryObject.SetActive(false);
    }
}
