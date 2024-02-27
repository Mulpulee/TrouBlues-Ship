using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Timer
{
    public static int Time;

    public static void SetTimer(int value)
    {
        Time = value;
    }

    public static int ReduceTimer()
    {
        PVHandler.pv.RPC("SetTimer", Photon.Pun.RpcTarget.All, Time - 1);
        return Time;
    }
}
