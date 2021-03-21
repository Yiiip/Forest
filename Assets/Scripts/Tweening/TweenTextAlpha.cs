//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the alpha of Text.
/// </summary>
[AddComponentMenu("Tween/Tween Text Alpha")]
public class TweenTextAlpha : UITweener
{
    Text m_text;

    string m_orgString;
    int m_count;
    int m_index;

    int Value
    {
        get
        {
            return m_index;
        }

        set
        {
            m_index = value;

            if (m_index < 0)
                m_index = 0;
            else if (m_index > m_count)
                m_index = m_count;

            var start = m_orgString.Substring(0, m_index);
            var end = m_orgString.Substring(m_index);
            m_text.text = string.Concat(start + "<color=#ffffff00>" + end + "</color>");
        }
    }

    public void Init()
    {
        if (m_text == null)
            m_text = GetComponent<Text>();

        m_orgString = m_text.text;
        m_count = m_orgString.Length;
        m_index = 0;
    }

	protected override void OnUpdate (float factor, bool isFinished)
	{
        Value = UnityEngine.Mathf.RoundToInt(UnityEngine.Mathf.Lerp(0f, (float)(m_count + 1), factor));
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>
    static public TweenTextAlpha Begin(GameObject go, float duration)
	{
        TweenTextAlpha comp = UITweener.Begin<TweenTextAlpha>(go, duration);
        comp.Init();
        return comp;
	}
}
