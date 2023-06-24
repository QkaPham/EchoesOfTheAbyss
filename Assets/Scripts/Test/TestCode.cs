using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestCode : MonoBehaviour
{
    //public Button button;
    //public Image image;

    //public AudioClip audioClip;

    //public AudioSource audioSource1;
    //public float fadeOutTime;
    //public float fadeSpeed;
    //public float audioClipTime;

    //public AudioSource audioSource2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(ItemConfig.maxRarity);
        }

        //audioClipTime = audioSource1.time;

        //if (audioSource1.time > fadeOutTime)
        //{
        //    if (!audioSource2.isPlaying)
        //    {
        //        audioSource2.Play();
        //    }
        //    audioSource1.volume -= fadeSpeed * Time.deltaTime;
        //    audioSource2.volume += fadeSpeed * Time.deltaTime;
        //}

    }

    //private void Awake()
    //{
    //    Debug.Log(audioClip.length);
    //}

    //public void Func()
    //{
    //    Debug.Log("Click");
    //    image.color = Color.gray;
    //}


}
