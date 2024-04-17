using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    public InputField nameInput;
    private string m_name;

    private void Awake()
    {
        if (PlayerPrefs.GetString("PlayerName") == null)
        {
            PlayerPrefs.SetString("PlayerName", "");
        }
    }

    public void SetUpInputField()
    {
        m_name = PlayerPrefs.GetString("PlayerName");
        nameInput.text = m_name;
        nameInput.textComponent.text = m_name;
    }

    public void SetNickName()
    {
        PlayerPrefs.SetString("PlayerName", nameInput.textComponent.text);
    }

}
