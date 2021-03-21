//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the audio source's volume.
/// </summary>
[AddComponentMenu("Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
	public float from = 1;
    public float to = 0;

    Graphic m_graphic = null;
    CanvasGroup m_group = null;
    SpriteRenderer m_spriteRenderer = null;

    public float value
	{
		get
		{
            Init();

            if (m_group != null)
                return m_group.alpha;
            else if(m_graphic != null)
                return m_graphic.color.a;
            else if (m_spriteRenderer != null)
                return m_spriteRenderer.color.a;
            else
                return 1;
		}

        set
        {
            Init();

            if (m_group != null)
            {
                m_group.alpha = value;
            }
            else if(m_graphic != null)
            {
                Color c = m_graphic.color;
                c.a = value;
                m_graphic.color = c;
            }
            else if (m_spriteRenderer != null)
            {
                Color c = m_spriteRenderer.color;
                c.a = value;
                m_spriteRenderer.color = c;
            }
        }
	}

    private void Init()
    {
        if (m_group == null)
            m_group = GetComponent<CanvasGroup>();

        if (m_graphic == null)
            m_graphic = GetComponent<Graphic>();

        if (m_spriteRenderer == null)
            m_spriteRenderer = GetComponent<SpriteRenderer>();


        if (m_graphic == null && m_group == null && m_spriteRenderer == null)
        {
            Debug.LogError("TweenAlpha needs a [Graphic / CanvasGroup / SpriteRenderer / SpriteText] to work with", this);
            enabled = false;
        }
    }

    public TweenAlpha From(float a)
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
    static public TweenAlpha Begin(GameObject go, float duration, float targetAlpha)
	{
        TweenAlpha comp = UITweener.Begin<TweenAlpha>(go, duration);
		comp.from = comp.value;
        comp.to = targetAlpha;

		return comp;
	}

	public override void SetStartToCurrentValue () { from = value; }
	public override void SetEndToCurrentValue () { to = value; }
}
