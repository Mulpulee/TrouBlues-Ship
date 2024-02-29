using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Timer
{
    public static int Time;

    public static void SetTimer(int value)
    {
        Time = value;
        PVHandler.pv.RPC("SetTimer", Photon.Pun.RpcTarget.All, Time);
    }

    public static int ReduceTimer(bool plus = false)
    {
        PVHandler.pv.RPC("SetTimer", Photon.Pun.RpcTarget.All, Time - (plus ? -1 : 1));
        return Time;
    }
}
