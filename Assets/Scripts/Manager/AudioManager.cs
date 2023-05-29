using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private float bgmFadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_HIGH;

    private string nextBGMName;

    private bool isFadeOut = false;

    public AudioSource AttachBGMSource;
    public AudioSource AttachSESource;

    private Dictionary<string, AudioClip> bgmDic, seDic;


    protected override void Awake()
    {
        base.Awake();

        bgmDic = new Dictionary<string, AudioClip>();
        seDic = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll("Audio/BGM");
        object[] seList = Resources.LoadAll("Audio/SE");

        foreach (AudioClip bgm in bgmList)
        {
            bgmDic[bgm.name] = bgm;
        }

        foreach (AudioClip se in seList)
        {
            seDic[se.name] = se;
        }
    }

    private void Start()
    {
        AttachBGMSource.volume = PlayerPrefs.GetFloat(CONST.BGM_VOLUME_KEY, CONST.BGM_VOLUME_DEFAULT);
        AttachSESource.volume = PlayerPrefs.GetFloat(CONST.SE_VOLUME_KEY, CONST.SE_VOLUME_DEFAULT);
    }

    public void PlaySE(string seName, float delay = 0f)
    {
        if (!seDic.ContainsKey(seName))
        {
            Debug.LogError(seName + " There is no SE named");
            return;
        }

        //nextSEName = seName;
        StartCoroutine(DelayPlaySE(seName, delay));
    }

    private IEnumerator DelayPlaySE(string seName, float delay)
    {
        yield return new WaitForSeconds(delay);
        AttachSESource.PlayOneShot(seDic[seName] as AudioClip);
    }

    public void PlayBGM(string bgmName, float fadeSpeedRare = CONST.BGM_FADE_SPEED_RATE_HIGH)
    {
        if (!bgmDic.ContainsKey(bgmName))
        {
            if (!seDic.ContainsKey(bgmName))
            {
                Debug.LogError(bgmName + "There is no BGM named");
                return;
            }
        }

        if (!AttachBGMSource.isPlaying)
        {
            nextBGMName = "";
            AttachBGMSource.clip = bgmDic[bgmName] as AudioClip;
            AttachBGMSource.Play();
        }
        else if (AttachBGMSource.clip.name != bgmName)
        {
            nextBGMName = bgmName;
            FadeOutBGM(fadeSpeedRare);
        }
    }

    public void FadeOutBGM(float fadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_LOW)
    {
        bgmFadeSpeedRate = fadeSpeedRate;
        isFadeOut = true;
    }

    private void Update()
    {
        if (!isFadeOut) return;
        AttachBGMSource.volume -= Time.deltaTime * bgmFadeSpeedRate;
        if (AttachBGMSource.volume <= 0)
        {
            AttachBGMSource.Stop();
            AttachBGMSource.volume = PlayerPrefs.GetFloat(CONST.BGM_VOLUME_KEY, CONST.BGM_VOLUME_DEFAULT);
            isFadeOut = false;
            if (!string.IsNullOrEmpty(nextBGMName))
            {
                PlayBGM(nextBGMName);
            }
        }

    }

    public void ChangeBGMVolume(float BGMVolume)
    {
        AttachBGMSource.volume = BGMVolume;
        PlayerPrefs.SetFloat(CONST.BGM_VOLUME_KEY, BGMVolume);
    }
    public void ChangeSEVolume(float SEVolume)
    {
        AttachSESource.volume = SEVolume;
        PlayerPrefs.SetFloat(CONST.SE_VOLUME_KEY, SEVolume);
    }
}
public class CONST
{
    //key and default value for saving volume
    public const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    public const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
    public const float BGM_VOLUME_DEFAULT = .2f;
    public const float SE_VOLUME_DEFAULT = 1f;

    //Time it take for the BGM to fade
    public const float BGM_FADE_SPEED_RATE_HIGH = .9f;
    public const float BGM_FADE_SPEED_RATE_LOW = .3f;

}