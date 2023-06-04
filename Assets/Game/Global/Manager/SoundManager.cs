using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 원하는 오디오 클립을 재생하는 클래스. 
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }
    public AudioSource SFXSource;

    public void PlaySound(AudioClip clip)
    {
        SFXSource.clip = clip;
        SFXSource.Play();
    }
}