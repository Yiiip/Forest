using UnityEngine;


[AddComponentMenu("Tween/Tween Rect Position")]
public class TweenRectPosition : UITweener
{
    public Vector2 from;
    public Vector2 to;

    RectTransform m_trans;

    public RectTransform cachedTransform
    {
        get
        {
            if (m_trans == null)
                m_trans = (RectTransform)transform;
            return m_trans;
        }
    }

    // 刷新
    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = Vector2.Lerp(from, to, factor);
    }

    public Vector2 value
    {
        get
        {
            return cachedTransform.anchoredPosition;
        }
        set
        {
            cachedTransform.anchoredPosition = value;
        }
    }

    public TweenRectPosition From(Vector2 f)
    {
        from = f;
        return this;
    }

    static public TweenRectPosition Begin(GameObject go, float duration, Vector2 pos)
    {
        TweenRectPosition comp = UITweener.Begin<TweenRectPosition>(go, duration);
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }

        return comp;
    }


    [ContextMenu("TestPos")]
    public void TestPos()
    {
        Debug.LogWarning(gameObject.transform.localPosition);
    }
}
