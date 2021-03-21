//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright ? 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's position.
/// </summary>

[AddComponentMenu("Tween/Tween PositionEx")]
public class TweenPositionEx : UITweener
{
	public Vector3 from;
    public Vector3 mid;
	public Vector3 to;

    [Range(0f, 1f)]
    public float midPosition = 0.5f;

	Transform mTrans;

	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }
	public Vector3 position { get { return cachedTransform.localPosition; } set { cachedTransform.localPosition = value; } }

	override protected void OnUpdate (float factor, bool isFinished)
    {
        Vector3 old = factor <= midPosition ? from : mid;
        Vector3 target = factor <= midPosition ? mid : to;
        float realfactor = factor <= midPosition ? factor / midPosition : (factor - midPosition) / (1 - midPosition);

        cachedTransform.localPosition = old * (1f - realfactor) + target * realfactor;
    }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public TweenPositionEx Begin(GameObject go, float duration, Vector3 mid, Vector3 pos, float midPosition)
	{
        TweenPositionEx comp = UITweener.Begin<TweenPositionEx>(go, duration);
		comp.from = comp.position;
        comp.mid = mid;
		comp.to = pos;
        comp.midPosition = midPosition;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }

		return comp;
	}
}