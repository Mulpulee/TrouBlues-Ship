using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHudInfo : MonoBehaviour
{
    [SerializeField] private GameObject m_hudCanvas;

    [SerializeField] private Text m_tabCount;
    [SerializeField] private Image m_profile;

    public void OnOff(bool onoff)
    {
        m_hudCanvas.SetActive(onoff);
    }

    public void UpdateInfo()
    {
        m_tabCount.text = CommonData.Medecines.ToString();
        m_profile.sprite = Player.This.PlayerProfile;

        if (Player.This.IsDead) m_profile.color = Color.gray;
        else m_profile.color = Color.white;
    }
}
