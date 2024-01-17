using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Services.Vivox;
using UnityEngine;
using VivoxUnity;

[SerializeField]
public class Vivox
{
    public Client client;

    public Uri server = new Uri("https://unity.vivox.com/appconfig/14569-troub-39310-udash");
    public string issuer = "14569-troub-39310-udash";
    public string domain = "mtu1xp.vivox.com";
    public string tokenKey = "zfyvW4V08xA8ua59QTOAW8g79rwQPV0A";
    public TimeSpan timeSpan = TimeSpan.FromSeconds(90);

    public ILoginSession LoginSession;
    public IChannelSession ChannelSession;
}

public class VivoxManager : MonoBehaviour
{
    public Vivox vivox = new Vivox();

    private void Awake()
    {
        vivox.client = new Client();
        vivox.client.Uninitialize();
        vivox.client.Initialize();
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnApplicationQuit()
    {
        vivox.client.Uninitialize();
    }

    public void Login()
    {
<<<<<<< HEAD
        string userName = "Tester";
        AccountId accountId = new AccountId(vivox.issuer, userName, vivox.domain);
        vivox.LoginSession = vivox.client.GetLoginSession(accountId);
        vivox.LoginSession.BeginLogin(vivox.server, vivox.LoginSession.GetLoginToken(vivox.tokenKey, vivox.timeSpan),
            callback =>
            {
                try
                {
                    vivox.LoginSession.EndLogin(callback);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
=======
        //string userName = "Tester";
        //AccountId accountId = new AccountId(vivox.issuer, userName, vivox.domain);
        //vivox.LoginSession = vivox.client.GetLoginSession(accountId);
        //vivox.LoginSession.BeginLogin(vivox.server, vivox.LoginSession.GetLoginToken(vivox.tokenKey, vivox.timeSpan),
        //    callback: AsyncResult =>
        //    {
        //        try
        //        {
        //            vivox.LoginSession.EndLogin(callback);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }
        //    });
>>>>>>> 485a3daa15e5138722096f77da935e232c7447fb
    }
}
