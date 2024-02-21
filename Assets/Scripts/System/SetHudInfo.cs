using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHudInfo : MonoBehaviour
{
    [SerializeField] private Text m_tabCount;
    [SerializeField] private Image m_profile;

    public void UpdateInfo()
    {
        m_tabCount.text = CommonData.Medecines.ToString();
        m_profile.sprite = Player.This.PlayerProfile;
    }
}
