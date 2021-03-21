//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the image fill amount
/// </summary>
[AddComponentMenu("Tween/Tween Fill Amount")]
public class TweenFillAmount : UITweener
{
	public float from = 1;
    public float to = 0;

    Image m_image = null;

    public float value
	{
		get
		{
            if (Init())
            {
                return m_image.fillAmount;
            }
            else
            {
                return 0;
            }
        }

        set
        {
            if (Init())
            {
                m_image.fillAmount = value;
            }
        }
	}

    private bool Init()
    {
        if (m_image == null)
            m_image = GetComponent<Image>();
        else
            return true;

        if (m_image == null)
        {
            Debug.LogError("TweenFillAmount needs a [Image] to work with", this);
            enabled = false;
            return false;
        }
        else if (m_image != null && m_image.type != Image.Type.Filled)
        {
            Debug.LogError("TweenFillAmount needs a [Image]'s type set to [Filled] to work with", this);
            enabled = false;
            return false;
        }

        return true;
    }

    public TweenFillAmount From(float a)
    {
        from = a;
        return this;
    }

	protected override void OnUpdate (float factor, bool isFinished)
	{
        value = UnityEngine.Mathf.Lerp(from, to, factor);
	}

    /// <summary>
    /// Start the tweening operation.
    /// </summary>
    static public TweenFillAmount Begin(GameObject go, float duration, float targetAlpha)
    {
        TweenFillAmount comp = UITweener.Begin<TweenFillAmount>(go, duration);
        comp.from = comp.value;
        comp.to = targetAlpha;

        return comp;
    }
}
