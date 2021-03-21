//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright ? 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's rotation.
/// </summary>

[AddComponentMenu("Tween/Tween RotationEx")]
public class TweenRotationEx : UITweener
{
	public Vector3 from;
    public Vector3 mid;
	public Vector3 to;

    [Range(0f, 1f)]
    public float midPosition = 0.5f;

	Transform mTrans;

	public Quaternion rotation { get { return mTrans.localRotation; } set { mTrans.localRotation = value; } }

	void Awake () { mTrans = transform; }

	override protected void OnUpdate (float factor, bool isFinished)
	{
        Vector3 old = factor <= midPosition ? from : mid;
        Vector3 target = factor <= midPosition ? mid : to;
        float realfactor = factor <= midPosition ? factor / midPosition : (factor - midPosition) / (1 - midPosition);

        mTrans.localRotation = Quaternion.Slerp(Quaternion.Euler(old), Quaternion.Euler(target), realfactor);
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public TweenRotationEx Begin(GameObject go, float duration, Quaternion midRot, Quaternion rot, float mid)
	{
        TweenRotationEx comp = UITweener.Begin<TweenRotationEx>(go, duration);
		comp.from = comp.rotation.eulerAngles;
        comp.mid = midRot.eulerAngles;
		comp.to = rot.eulerAngles;
        comp.midPosition = mid;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }

		return comp;
	}

    static public TweenRotationEx Begin(GameObject go, float duration, Vector3 midRot, Vector3 rot, float mid)
    {
        TweenRotationEx comp = UITweener.Begin<TweenRotationEx>(go, duration);
        comp.from = comp.rotation.eulerAngles;
        comp.mid = midRot;
        comp.to = rot;
        comp.midPosition = mid;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }

        return comp;
    }
}