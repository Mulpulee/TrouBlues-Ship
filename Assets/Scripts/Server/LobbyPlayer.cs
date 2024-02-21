using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour
{
    public Text nickName;
    public Image profile;
    public bool isReady = false;
    public GameObject ReadyLine;

    private void Update()
    {
        if (isReady)
        {
            ReadyLine.SetActive(true);
        }
        else
        {
            ReadyLine.SetActive(false);
        }
    }

    public void Setup(string name, Sprite sprite)
    {
        nickName.text = name;
        profile.sprite = sprite;
    }
}
