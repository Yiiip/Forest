using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 简单的计时器
/// </summary>
[AddComponentMenu("Tween/Tween Timer")]
public class TweenTimer : UITweener
{
	protected override void OnUpdate (float factor, bool isFinished)
	{
        // do nothing
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>
    static public TweenTimer Begin(GameObject go, float duration, System.Action callback)
	{
        TweenTimer comp = UITweener.Begin<TweenTimer>(go, duration);
        comp.onFinished = callback;

        return comp;
	}
}
