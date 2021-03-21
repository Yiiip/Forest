//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// Tween the object's Slider.
/// </summary>
[RequireComponent(typeof(Slider))]
[AddComponentMenu("Tween/Tween Slider")]
public class TweenSlider : UITweener
{
	public float from = 0;
	public float to = 1;

	Slider m_slider = null;

    Slider MySlider
    {
        get
        {
            if (m_slider == null)
                m_slider = gameObject.GetComponent<Slider>();

            return m_slider;
        }
    }

    public float value 
	{ 
		get
		{ 
			return MySlider.value;
		} 
		set 
		{
            MySlider.value = value; 
		}
	}

	/// <summary>
	/// Tween the value.
	/// </summary>
	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = from * (1f - factor) + to * factor;
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>
	static public TweenSlider Begin (GameObject go, float duration, float value)
	{
		TweenSlider comp = UITweener.Begin<TweenSlider>(go, duration);
		comp.from = comp.value;
		comp.to = value;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
