using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHudInfo : MonoBehaviour
{
    [SerializeField] private Text m_tabCount;
    [SerializeField] private Image m_profile;

    public void UpdateInfo(int pMedecine, Sprite pProfile = null)
    {
        m_tabCount.text = pMedecine.ToString();
        if (pProfile != null) m_profile.sprite = pProfile;
    }
}
