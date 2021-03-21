//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the audio source's volume.
/// </summary>
[RequireComponent(typeof(UnityEngine.Light))]
[AddComponentMenu("Tween/Tween Light")]
public class TweenLight : UITweener
{
	public Color colorFrom = new Color(1, 1, 1, 1);
    public Color colorTo = new Color(1, 1, 1, 1);

    public float intensityFrom = 1;
    public float intensityTo = 1;

    Light m_light;

	public Color color
	{
		get
		{
            Init();

            if (m_light != null)
            {
                return m_light.color;
            }
            else
            {
                return Color.white;
            }
		}

		set
		{
            Init();

            if (m_light != null)
            {
                m_light.color = value;
            }
		}
	}

    public float intensity
    {
        get
        {
            Init();

            if (m_light != null)
            {
                return m_light.intensity;
            }
            else
            {
                return 1;
            }
        }

        set
        {
            Init();

            if (m_light != null)
            {
                m_light.intensity = value;
            }
        }
    }

    private void Init()
    {
        if (m_light == null)
            m_light = GetComponent<Light>();

        if (m_light == null)
        {
            Debug.LogError("TweenLight needs a Light to work with", this);
            enabled = false;
        }
    }

	protected override void OnUpdate (float factor, bool isFinished)
	{
        color = Color.Lerp(colorFrom, colorTo, factor);
        intensity = Mathf.Lerp(intensityFrom, intensityTo, factor);
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>
    static public TweenLight Begin(GameObject go, float duration, Color targetColor, float targetIntensity)
	{
        TweenLight comp = UITweener.Begin<TweenLight>(go, duration);
        comp.colorFrom = comp.color;
        comp.colorTo = targetColor;
        comp.intensityFrom = comp.intensity;
        comp.intensityTo = targetIntensity;

		return comp;
	}
}
