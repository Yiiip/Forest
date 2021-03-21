//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the audio source's volume.
/// </summary>
[RequireComponent(typeof(UnityEngine.UI.Graphic))]
[AddComponentMenu("Tween/Tween Color")]
public class TweenColor : UITweener
{
	public Color from = new Color(1, 1, 1, 1);
    public Color to = new Color(1, 1, 1, 1);

    Graphic m_graphic = null;
    SpriteRenderer m_spriteRenderer = null;


	public Color value
	{
		get
		{
            Init();
            if (m_graphic != null)
                return m_graphic.color;
            else if (m_spriteRenderer != null)
                return m_spriteRenderer.color;
            else
                return Color.white;
		}

		set
		{
            Init();

            if (m_graphic != null)
                m_graphic.color = value;
            else if (m_spriteRenderer != null)
                m_spriteRenderer.color = value;
		}
	}

    private void Init()
    {
        if (m_graphic == null)
            m_graphic = GetComponent<Graphic>();
        
        if (m_spriteRenderer == null)
            m_spriteRenderer = GetComponent<SpriteRenderer>();

        if (m_graphic == null && m_spriteRenderer == null)
        {
            Debug.LogError("TweenColor needs an Graphic or CanvasGroup to work with", this);
            enabled = false;
        }
    }

	protected override void OnUpdate (float factor, bool isFinished)
	{
        value = Color.Lerp(from, to, factor);
	}

    public TweenColor From(Color c)
    {
        from = c;
        return this;
    }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>
    static public TweenColor Begin(GameObject go, float duration, Color targetColor)
	{
        TweenColor comp = UITweener.Begin<TweenColor>(go, duration);
		comp.from = comp.value;
        comp.to = targetColor;

		return comp;
	}

	public override void SetStartToCurrentValue () { from = value; }
	public override void SetEndToCurrentValue () { to = value; }
}
