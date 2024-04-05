using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerList : MonoBehaviour
{
    [SerializeField] private Transform m_listParent;

    public void Show()
    {
        Show(CommonData.Players);
    }

    public void Show(List<Player> pPlayers)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < m_listParent.childCount; i++) m_listParent.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < pPlayers.Count; i++)
        {
            Transform p = m_listParent.GetChild(i);
            p.gameObject.SetActive(true);
            p.GetChild(0).GetComponent<Image>().sprite = pPlayers[i].PlayerProfile;

            int[] id = pPlayers[i].ID;
            Transform idP = p.GetChild(1);

            for (int j = 0; j < id.Length; j++)
            {
                idP.GetChild(j).GetComponent<Image>().sprite = EarthCommunication.CharacterID[id[j]];
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
