using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PVHandler : MonoBehaviour
{
    private static PhotonView m_pv;
    public static PhotonView pv
    {
        get
        {
            if (m_pv == null)
            {
                m_pv = FindObjectOfType<PhotonView>();
                if (m_pv == null)
                {
                    m_pv = Instance.AddComponent<PhotonView>();
                }
            }

            return m_pv;
        }
    }
    private static PVHandler m_instance;
    public static PVHandler Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PVHandler>();
                if (m_instance == null)
                {
                    GameObject instance = new GameObject("PhotonView");
                    m_instance = instance.AddComponent<PVHandler>();
                    m_pv = instance.AddComponent<PhotonView>();
                    DontDestroyOnLoad(instance);
                }
            }
            return m_instance;
        }
    }
}
