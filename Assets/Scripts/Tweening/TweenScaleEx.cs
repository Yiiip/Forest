using UnityEngine;

/// <summary>
/// Tween the object's local scale.
/// </summary>

[AddComponentMenu("Tween/Tween ScaleEx")]
public class TweenScaleEx : UITweener
{
	public Vector3 from;
	public Vector3 mid;
	public Vector3 to;
	public bool updateTable = false;

    [Range(0f, 1f)]
    public float midPosition = 0.5f;
	
	Transform mTrans;

    public Vector3 scale { get { return Trans.localScale; } set { Trans.localScale = value; } }

    public Transform Trans 
    { 
        get 
        {
            if (mTrans == null)
            {
                mTrans = transform;
            }
            return mTrans;
        }
    }
	
	override protected void OnUpdate (float factor, bool isFinished)
	{
		Vector3 old = factor <= midPosition ? from : mid;
		Vector3 target = factor <= midPosition ? mid : to;
		float realfactor = factor <= midPosition ? factor / midPosition : (factor - midPosition) / (1 - midPosition);

        Trans.localScale = old * (1f - realfactor) + target * realfactor;
	}

    public TweenScaleEx From(Vector3 f)
    {
        from = f;
        return this;
    }

    public TweenScaleEx TweemMethod(Method m)
    {
        method = m;
        return this;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenScaleEx Begin(GameObject go, float duration, Vector3 scaleMid, Vector3 scaleEnd, float mid)
	{
		TweenScaleEx comp = UITweener.Begin<TweenScaleEx>(go, duration);
		comp.from = comp.scale;
		comp.mid = scaleMid;
		comp.to = scaleEnd;
		comp.midPosition = mid;
		
		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		
		return comp;
	}
}