using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx // : MonoBehaviour
{
    private static Player m_this;
    public static Player Player
    {
        get { return m_this; }
    }

    public static EarthCommunication EarthCommu = new EarthCommunication();

    public void StartGame()
    {

    }
}
