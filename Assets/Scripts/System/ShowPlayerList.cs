using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerList : MonoBehaviour
{
    [SerializeField] private Transform m_listParent;

    public void Show(bool SetID = true)
    {
        Show(CommonData.Players, SetID);
    }

    public void Show(List<Player> pPlayers, bool pSetID)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < m_listParent.childCount; i++) m_listParent.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < pPlayers.Count; i++)
        {
            Transform p = m_listParent.GetChild(i);
            p.gameObject.SetActive(true);
            p.GetChild(0).GetComponent<Image>().sprite = pPlayers[i].Profile;

            if (!pSetID) continue;

            int[] id = pPlayers[i].ID;
            string temp = "";
            foreach (int item in id) temp = $"{temp}{EarthCommunication.CharacterID[item]}";
            p.GetChild(1).GetComponent<Text>().text = temp;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
