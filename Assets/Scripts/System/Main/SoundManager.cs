using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Ins;

    [SerializeField] private Slider BgmSlider;
    [SerializeField] private Slider SfxSlider;

    private AudioSource m_bgmSource;
    private List<AudioSource> m_sfxSources;

    private float m_sfxVol = 0.5f;

    [SerializeField] private Sound[] Bgms;
    [SerializeField] private Sound[] Sfxs;

    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        m_bgmSource = GetComponent<AudioSource>();
        m_sfxSources = new List<AudioSource>();

        SetVolume();
        m_sfxVol = PlayerPrefs.GetFloat("sfxVol");
    }
    
    private void Update()
    {
        foreach (var i in m_sfxSources)
        {
            if (!i.isPlaying)
            {
                m_sfxSources.Remove(i);
                Destroy(i);
                break;
            }
        }
    }

    public void PlayBgm(string name)
    {
        Sound s = Array.Find(Bgms, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            m_bgmSource.clip = s.clip;
            m_bgmSource.Play();
        }
    }

    public void StopBgm() { m_bgmSource.Stop(); }

    public void PlaySfx(string name)
    {
        Sound s = Array.Find(Sfxs, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            AudioSource sfx = gameObject.AddComponent<AudioSource>();
            sfx.volume = m_sfxVol;
            sfx.clip = s.clip;
            sfx.Play();
            m_sfxSources.Add(sfx);
        }
    }

    public void BgmVolume(float volume)
    {
        m_bgmSource.volume = volume;
        PlayerPrefs.SetFloat("bgmVol", volume);
    }

    public void SfxtVolume(float volume)
    {
        m_sfxVol = volume;
        PlayerPrefs.SetFloat("sfxVol", volume);
    }


    public void BgmVolume()
    {
        Ins.BgmVolume(BgmSlider.value);
    }
    
    public void SfxVolume()
    {
        Ins.SfxtVolume(SfxSlider.value);
    }
    
    public void SetVolume()
    {
        BgmSlider.value = PlayerPrefs.GetFloat("bgmVol");
        m_sfxVol = PlayerPrefs.GetFloat("sfxVol");
        SfxSlider.value = m_sfxVol;
    }
}