using UnityEngine;
using System.Collections;

/// <summary>
/// bezier tween 
/// </summary>
[AddComponentMenu("Tween/Tween Bezier")]
public class TweenBezier : UITweener 
{
    public enum eCurveType
    {
        QUADRATIC,
        CUBIC,
    }

    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;

    protected Transform mTrans;
    public eCurveType m_type;

    public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

    /// <summary>
    /// set the value 
    /// </summary>
    public Vector3 value
    {
        get
        {
            return cachedTransform.localPosition;
        }
        set
        {
            cachedTransform.localPosition = value;
        }
    }

    /// <summary>
    /// update 
    /// </summary>
    /// <param name="factor"></param>
    /// <param name="isFinished"></param>
    protected override void OnUpdate(float factor, bool isFinished)
    {
        float t = factor;
        float rt = 1f - factor;

        if (m_type == eCurveType.CUBIC)
            value = p0 * rt * rt * rt + 3 * p1 * t * rt * rt + 3 * p2 * t * t * rt + p3 * t * t * t;
        else if (m_type == eCurveType.QUADRATIC)
            value = rt * rt * p0 + 2 * t * rt * p1 + t * t * p2;
    }

    /// <summary>
    /// begin the tween (cubic bezier curve)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="duration"></param>
    /// <param name="targetPoint"></param>
    /// <returns></returns>
    static public TweenBezier Begin(GameObject go, float duration, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 targetPoint)
    {
        TweenBezier comp = UITweener.Begin<TweenBezier>(go, duration);

        comp.m_type = eCurveType.CUBIC;
        comp.p0 = comp.value;
        comp.p1 = controlPoint1;
        comp.p2 = controlPoint2;
        comp.p3 = targetPoint;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    /// <summary>
    /// begin the tween (quadratic bezier curve)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="duration"></param>
    /// <param name="targetPoint"></param>
    /// <returns></returns>
    static public TweenBezier Begin(GameObject go, float duration, Vector3 controlPoint, Vector3 targetPoint)
    {
        TweenBezier comp = UITweener.Begin<TweenBezier>(go, duration);

        comp.m_type = eCurveType.QUADRATIC;
        comp.p0 = comp.value;
        comp.p1 = controlPoint;
        comp.p2 = targetPoint;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
}
