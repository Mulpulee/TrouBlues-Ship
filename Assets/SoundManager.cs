using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Slider _bgmSlider;
    public Slider _effectSlider;

    public AudioSource Bgm;
    public AudioSource Effect;

    public Sound[] BgmSnds;
    public Sound[] EffectSnds;

    public void PlayBgm(string name)
    {
        Sound s = Array.Find(BgmSnds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            Bgm.clip = s.audioClip;
            Bgm.Play();
        }
    }

    public void PlayEffect(string name)
    {
        Sound s = Array.Find(EffectSnds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            Effect.PlayOneShot(s.audioClip);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBgm("Bgm");
    }

    public void BgmVolume(float volume)
    {
        Bgm.volume = volume;
    }

    public void EffectVolume(float volume)
    {
        Bgm.volume = volume;
    }


    public void BgmVolume()
    {
        instance.BgmVolume(_bgmSlider.value);
    }

    public void EffectVolume()
    {
        instance.EffectVolume(_bgmSlider.value);
    }

    public void SetVolume()
    {
        _bgmSlider.value = instance.Bgm.volume;
        _effectSlider.value = instance.Effect.volume;
    }
}