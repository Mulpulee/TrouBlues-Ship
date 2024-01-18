using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ProfileObject", menuName = "ScriptableObject/ProfileObject", order = 1)]
public class Profile : ScriptableObject
{
    public Image profile;
    public string nickName;
}
