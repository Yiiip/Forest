//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the audio source's volume.
/// </summary>
[AddComponentMenu("Tween/Tween Size")]
public class TweenSize : UITweener
{
    public Vector2 from;
    public Vector2 to;

    RectTransform mTrans;

    public RectTransform cachedTransform
    {
        get
        {
            if (mTrans == null)
                mTrans = gameObject.GetComponent<RectTransform>();
            return mTrans;
        }
    }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Vector3 value
    {
        get
        {
            return cachedTransform.sizeDelta;
        }
        set
        {
            cachedTransform.sizeDelta = value;
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = Vector2.Lerp(from, to, factor);
    }

    public TweenSize From(Vector2 f)
    {
        from = f;
        return this;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenSize Begin(GameObject go, float duration, Vector2 size)
    {
        TweenSize comp = UITweener.Begin<TweenSize>(go, duration);
        comp.from = comp.value;
        comp.to = size;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue() { to = value; }
}
