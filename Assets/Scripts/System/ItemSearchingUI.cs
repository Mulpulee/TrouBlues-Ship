using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSearchingUI : MonoBehaviour
{
    [SerializeField] private Text m_remainCountText;

    [SerializeField] private Text[] m_searchedItemsText;
    [SerializeField] private Text[] m_collectedItemsText;

    private int m_collectableCount;
    private int[] m_searchedItems;
    private int[] m_collectedItems;

    public void ShowUI()
    {
        gameObject.SetActive(true);

        m_remainCountText.text = m_collectableCount.ToString();

        for (int i = 0; i < m_searchedItemsText.Length; i++)
            m_searchedItemsText[i].text = m_searchedItems[i].ToString();
        foreach (var t in m_collectedItemsText) t.transform.parent.gameObject.SetActive(false);
    }

    public void SetValue(int pCount, int[] pItems)
    {
        m_collectableCount = pCount;
        m_searchedItems = pItems;
        m_collectedItems = new int[6];
    }

    public void SetText(int index)
    {
        m_remainCountText.text = m_collectableCount.ToString();
        m_searchedItemsText[index].text = m_searchedItems[index].ToString();

        if (m_collectedItems[index] == 0)
            m_collectedItemsText[index].transform.parent.gameObject.SetActive(false);
        else
        {
            m_collectedItemsText[index].transform.parent.gameObject.SetActive(true);
            m_collectedItemsText[index].text = m_collectedItems[index].ToString();
        }
    }

    public void CollectItem(int index)
    {
        if (m_collectableCount > 0 && m_searchedItems[index] > 0)
        {
            m_collectableCount--;
            m_collectedItems[index]++;
            m_searchedItems[index]--;

            SetText(index);
        }
    }

    public void ReturnItem(int index)
    {
        if (m_collectedItems[index] > 0)
        {
            m_collectableCount++;
            m_searchedItems[index]++;
            m_collectedItems[index]--;

            SetText(index);
        }
    }

    public void EndSelection(Player pPlayer)
    {
        for (int i = 0; i < 5; i++)
        {
            pPlayer.AddItem((ItemIndex)i, m_collectedItems[i]);
        }

        PVHandler.pv.RPC("AddMedicines", Photon.Pun.RpcTarget.MasterClient, m_collectedItems[(int)ItemIndex.Medicine]);
    }
}
