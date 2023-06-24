using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource MusicSource;
    public AudioSource SFXSource;

    private Dictionary<string, AudioClip> bgmDic, sfxDic;

    protected override void Awake()
    {
        base.Awake();

        bgmDic = new Dictionary<string, AudioClip>();
        sfxDic = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll("Audio/BGM");
        object[] seList = Resources.LoadAll("Audio/SE");

        foreach (AudioClip bgm in bgmList)
        {
            bgmDic[bgm.name] = bgm;
        }

        foreach (AudioClip se in seList)
        {
            sfxDic[se.name] = se;
        }
    }

    private void Start()
    {
        MuteAll();
    }

    public void MuteAll()
    {
        MusicSource.volume = 0f;
        MuteSFX();
    }

    public void MuteSFX()
    {
        SFXSource.volume = 0f;
    }

    public void UnMuteSFX()
    {
        SFXSource.volume = 1f;
    }

    public void PlaySFX(string sfxName, float delay = 0f)
    {
        if (!sfxDic.ContainsKey(sfxName))
        {
            Debug.LogError(sfxName + " There is no SE named");
            return;
        }
        StartCoroutine(DelayPlaySFX(sfxName, delay));
    }

    private IEnumerator DelayPlaySFX(string sfxName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SFXSource.PlayOneShot(sfxDic[sfxName]);
    }


    public void PlayMusic(string bgmName, float fadeOutDuration = 5f, float fadeInDuration = 5f)
    {
        if (!bgmDic.ContainsKey(bgmName))
        {
            Debug.LogError(bgmName + "There is no BGM named");
            return;
        }

        if (!MusicSource.isPlaying)
        {
            MusicSource.volume = 0f;
            MusicSource.clip = bgmDic[bgmName];
            FadeMusicVolume(1, fadeInDuration);
            MusicSource.Play();
        }

        else if (MusicSource.clip.name != bgmName)
        {
            MusicSource.clip = bgmDic[bgmName];
            MusicSource.Play();
            FadeMusicVolume(1, fadeInDuration);
        }
    }

    public void FadeMusicVolume(float toValue = .5f, float duration = 4f, TweenCallback OnComplete = null)
    {
        MusicSource.DOFade(toValue, duration).SetUpdate(true).OnComplete(OnComplete);
    }
}

//public class CONST
//{
//    //key and default value for saving volume
//    public const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
//    public const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
//    public const float BGM_VOLUME_DEFAULT = 1f;
//    public const float SE_VOLUME_DEFAULT = 1f;

//    //Time it take for the BGM to fade
//    public const float BGM_FADE_SPEED_RATE_HIGH = .9f;
//    public const float BGM_FADE_SPEED_RATE_LOW = .3f;
//}