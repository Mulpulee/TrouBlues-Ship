using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInventory : MonoBehaviour
{
    [SerializeField] private GameObject[] m_itemObject;

    public void Show()
    {
        SoundManager.Ins.PlaySfx("Game_button");
        Show(Player.This.GetInventory());
    }

    public void Show(Dictionary<ItemIndex, int> pInventory)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < m_itemObject.Length; i++)
        {
            m_itemObject[i].transform.GetChild(1).GetComponent<Text>().text = pInventory[(ItemIndex)i].ToString();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
