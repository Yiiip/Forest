using UnityEngine;
using System.Collections;

public class TweenCameraSize : UITweener 
{
    public float from;
    public float to;

    protected Camera m_camera;

    public Camera cachedCamera { get { if (m_camera == null) m_camera = GetComponent<Camera>(); return m_camera; } }

    /// <summary>
    /// set the value 
    /// </summary>
    public float value
    {
        get
        {
            return cachedCamera.orthographicSize;
        }
        set
        {
            cachedCamera.orthographicSize = value;
        }
    }

    /// <summary>
    /// update 
    /// </summary>
    /// <param name="factor"></param>
    /// <param name="isFinished"></param>
    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = from * (1f - factor) + to * factor;
    }

    /// <summary>
    /// begin the tween (cubic bezier curve)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="duration"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    static public TweenCameraSize Begin(GameObject go, float duration, float destSize)
    {
        TweenCameraSize comp = UITweener.Begin<TweenCameraSize>(go, duration);

        comp.from = comp.value;
        comp.to = destSize;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
}
