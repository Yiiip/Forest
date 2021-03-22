using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioManager : SingletonMonoEntire<AudioManager>
{
    // private static AudioManager instance;
    // public static AudioManager Instance
    // {
    //     get
    //     {
    //         return instance;
    //     }
    // }

    private Dictionary<int, string> audioPathDict; // 存放音频文件路径

    private AudioSource musicAudioSource;

    private List<AudioSource> unusedSoundAudioSourceList; // 存放可以使用的音频组件

    private List<AudioSource> usedSoundAudioSourceList; // 存放正在使用的音频组件

    private Dictionary<int, AudioClip> audioClipDict; // 缓存音频文件

    private float musicVolume = 1;

    private float soundVolume = 1;

    private string musicVolumePrefs = "MusicVolume";

    private string soundVolumePrefs = "SoundVolume";

    private int poolCount = 3; // 对象池数量

    protected override void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);
        // instance = this;

        base.Awake();

        audioPathDict = new Dictionary<int, string>() // 这里设置音频文件路径。需要修改
        {
            // {1000, "AudioClip/Music/MainMenuScene"},
            // {1001, "AudioClip/Music/BattleScene"},
            // {2000, "AudioClip/Sound/Sound1"},
            // {2001, "AudioClip/Sound/Sound2"},
        };

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        unusedSoundAudioSourceList = new List<AudioSource>();
        usedSoundAudioSourceList = new List<AudioSource>();
        audioClipDict = new Dictionary<int, AudioClip>();
    }

    void Start()
    {
        // 从本地缓存读取声音音量
        if (PlayerPrefs.HasKey(musicVolumePrefs))
        {
            musicVolume = PlayerPrefs.GetFloat(musicVolumePrefs);
        }
        if (PlayerPrefs.HasKey(soundVolumePrefs))
        {
            musicVolume = PlayerPrefs.GetFloat(soundVolumePrefs);
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="id"></param>
    /// <param name="loop"></param>
    public void PlayMusic(int id, bool loop = true)
    {
        // 通过Tween将声音淡入淡出
        DOTween.To(() => musicAudioSource.volume, value => musicAudioSource.volume = value, 0, 0.5f).OnComplete(() =>
        {
            musicAudioSource.clip = GetAudioClip(id);
            musicAudioSource.clip.LoadAudioData();
            musicAudioSource.loop = loop;
            musicAudioSource.volume = musicVolume;
            musicAudioSource.Play();
            DOTween.To(() => musicAudioSource.volume, value => musicAudioSource.volume = value, musicVolume, 0.5f);
        });
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="id"></param>
    public void PlaySound(int id, Action action = null)
    {
        if (unusedSoundAudioSourceList.Count != 0)
        {
            AudioSource audioSource = UnusedToUsed();
            audioSource.clip = GetAudioClip(id);
            audioSource.clip.LoadAudioData();
            audioSource.Play();

            StartCoroutine(WaitPlayEnd(audioSource, action));
        }
        else
        {
            AddAudioSource();

            AudioSource audioSource = UnusedToUsed();
            audioSource.clip = GetAudioClip(id);
            audioSource.clip.LoadAudioData();
            audioSource.volume = soundVolume;
            audioSource.loop = false;
            audioSource.Play();

            StartCoroutine(WaitPlayEnd(audioSource, action));
        }
    }

    /// <summary>
    /// 播放3d音效
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    public void Play3dSound(int id, Vector3 position)
    {
        AudioClip ac = GetAudioClip(id);
        AudioSource.PlayClipAtPoint(ac, position);
    }

    /// <summary>
    /// 当播放音效结束后，将其移至未使用集合
    /// </summary>
    /// <param name="audioSource"></param>
    /// <returns></returns>
    IEnumerator WaitPlayEnd(AudioSource audioSource, Action action)
    {
        yield return new WaitUntil(() =>
        {
            return !audioSource.isPlaying;
        });
        UsedToUnused(audioSource);
        if (action != null)
        {
            action();
        }
    }

    /// <summary>
    /// 获取音频文件，获取后会缓存一份
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private AudioClip GetAudioClip(int id)
    {
        if (!audioClipDict.ContainsKey(id))
        {
            if (!audioPathDict.ContainsKey(id))
                return null;
            AudioClip ac = Resources.Load(audioPathDict[id]) as AudioClip;
            audioClipDict.Add(id, ac);
        }
        return audioClipDict[id];
    }

    /// <summary>
    /// 添加音频组件
    /// </summary>
    /// <returns></returns>
    private AudioSource AddAudioSource()
    {
        if (unusedSoundAudioSourceList.Count != 0)
        {
            return UnusedToUsed();
        }
        else
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            unusedSoundAudioSourceList.Add(audioSource);
            return audioSource;
        }
    }

    /// <summary>
    /// 将未使用的音频组件移至已使用集合里
    /// </summary>
    /// <returns></returns>
    private AudioSource UnusedToUsed()
    {
        AudioSource audioSource = unusedSoundAudioSourceList[0];
        unusedSoundAudioSourceList.RemoveAt(0);
        usedSoundAudioSourceList.Add(audioSource);
        return audioSource;
    }

    /// <summary>
    /// 将使用完的音频组件移至未使用集合里
    /// </summary>
    /// <param name="audioSource"></param>
    private void UsedToUnused(AudioSource audioSource)
    {
        if (usedSoundAudioSourceList.Contains(audioSource))
        {
            usedSoundAudioSourceList.Remove(audioSource);
        }
        if (unusedSoundAudioSourceList.Count >= poolCount)
        {
            Destroy(audioSource);
        }
        else if (audioSource != null && !unusedSoundAudioSourceList.Contains(audioSource))
        {
            unusedSoundAudioSourceList.Add(audioSource);
        }
    }

    /// <summary>
    /// 修改背景音乐音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        musicAudioSource.volume = volume;

        PlayerPrefs.SetFloat(musicVolumePrefs, volume);
    }

    /// <summary>
    /// 修改音效音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeSoundVolume(float volume)
    {
        soundVolume = volume;
        for (int i = 0; i < unusedSoundAudioSourceList.Count; i++)
        {
            unusedSoundAudioSourceList[i].volume = volume;
        }
        for (int i = 0; i < usedSoundAudioSourceList.Count; i++)
        {
            usedSoundAudioSourceList[i].volume = volume;
        }

        PlayerPrefs.SetFloat(soundVolumePrefs, volume);
    }
}

// [Usage]
// void Start () {
//     AudioManager.Instance.PlayMusic( 1 );
//     AudioManager.Instance.PlaySound( 11, OnSoundPlayEnd );
//     AudioManager.Instance.ChangeMusicVolume( 0.5f );
//     AudioManager.Instance.ChangeSoundVolume( 0.5f );
// }
// void OnSoundPlayEnd() {
//     Debug.Log( "音效播放完了" );
// }