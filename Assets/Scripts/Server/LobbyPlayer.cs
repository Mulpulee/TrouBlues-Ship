using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour
{
    public Text nickName;
    public Image profile;

    public void Setup(string name, Image sprite)
    {
        nickName.text = name;
        profile = sprite;
    }
}
