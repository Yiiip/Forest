using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Tween/Tween Sound")]
public class TweenSound : UITweener
{
    public AudioSource m_audio;
    public float from;
    public float to;

    public AudioSource cachedAudioSounrce
    {
        get
        {
            if (m_audio == null)
                m_audio = gameObject.GetComponent<AudioSource>();
            return m_audio;
        }
    }

    public float value
    {
        get
        {
            return cachedAudioSounrce.volume;
        }
        set
        {
            cachedAudioSounrce.volume = value;
        }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = Mathf.Lerp(from, to, factor);
    }

    static public TweenSound Begin(GameObject go, float duration, float volume)
    {
        TweenSound comp = UITweener.Begin<TweenSound>(go, duration);
        comp.from = comp.value;
        comp.to = volume;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
}
