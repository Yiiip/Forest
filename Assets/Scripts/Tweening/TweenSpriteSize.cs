//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the audio source's volume.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[AddComponentMenu("Tween/Tween Sprite Size")]
public class TweenSpriteSize : UITweener
{
	public Vector2 from = new Vector2(0, 0);
    public Vector2 to = new Vector2(1, 1);

    SpriteRenderer m_spriteRenderer = null;

	public Vector2 value
	{
		get
		{
            Init();

            if (m_spriteRenderer != null)
                return m_spriteRenderer.size;
            else
                return Vector2.zero;
		}

		set
		{
            Init();

            if (m_spriteRenderer != null)
                m_spriteRenderer.size = value;
        }
	}

    private void Init()
    {
        if (m_spriteRenderer == null)
            m_spriteRenderer = GetComponent<SpriteRenderer>();

        if (m_spriteRenderer == null)
        {
            Debug.LogError("TweenSpriteSize needs an SpriteRenderer to work with", this);
            enabled = false;
        }
    }

	protected override void OnUpdate (float factor, bool isFinished)
	{
        value = Vector2.Lerp(from, to, factor);
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>
    static public TweenSpriteSize Begin(GameObject go, float duration, Vector2 targetSize)
	{
        TweenSpriteSize comp = UITweener.Begin<TweenSpriteSize>(go, duration);
		comp.from = comp.value;
        comp.to = targetSize;

		return comp;
	}
}
