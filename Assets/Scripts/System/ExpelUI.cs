using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpelUI : MonoBehaviour
{
    [SerializeField] private GameObject m_bg;
    [SerializeField] private Transform m_sprite;
    [SerializeField] private Image m_profile;
    [SerializeField] private Text m_text;
    [SerializeField] private Transform m_mask;

    public void Expel(int pProfileId, string pText)
    {
        RoomDisplayer.Ins.Announce(Announcement.None);
        m_bg.SetActive(true);
        m_sprite.gameObject.SetActive(true);

        m_profile.sprite = CommonData.ProfileObjects[pProfileId].profile;
        m_text.text = pText;

        StartCoroutine(ExpelRoutine());
    }

    private IEnumerator ExpelRoutine()
    {
        Vector3 textpos = m_text.transform.position;
        m_sprite.localPosition = new Vector3(-1200, 0);
        m_text.transform.position = textpos;
        m_mask.rotation = Quaternion.identity;

        while (m_sprite.localPosition.x < 1200)
        {
            m_sprite.position = m_sprite.position + (new Vector3(10, 0));
            m_sprite.Rotate(new Vector3(0, 0, -2));
            m_mask.rotation = Quaternion.identity;
            m_text.transform.position = textpos;
            yield return new WaitForSeconds(0.025f);
        }

        PVHandler.pv.RPC("TaskEnded", Photon.Pun.RpcTarget.MasterClient);
    }

    public void Hide()
    {
        m_bg.SetActive(false);
        m_sprite.gameObject.SetActive(false);
    }
}
