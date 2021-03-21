//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the object's alpha.
/// </summary>

[AddComponentMenu("Tween/Tween Number")]
public class TweenNumber : UITweener
{
	public int from = 0;
    public int to = 1;
    public string stringFormatter = "{0:D}";
    public string numberFormatter = "";
    public string replaceFrom = null;
    public string replaceTo = null;

    Text m_uiText;

    int m_number = 1;

	/// <summary>
	/// Current alpha.
	/// </summary>

	public int CurrentNumber
	{
		get
		{
            return m_number;
		}
		set
		{
            m_number = value;
            if (m_uiText != null && !string.IsNullOrEmpty(m_uiText.text))
            {
                if (!string.IsNullOrEmpty(replaceFrom))
                {
                    m_uiText.text = string.Format(stringFormatter, m_number.ToString(numberFormatter)).Replace(replaceFrom, replaceTo);
                }
                else
                {
                    m_uiText.text = string.Format(stringFormatter, m_number.ToString(numberFormatter));
                }
            }
        }
	}

	/// <summary>
	/// Find all needed components.
	/// </summary>

	void Awake ()
	{
        m_uiText = GetComponentInChildren<Text>();

        if (m_uiText != null && !string.IsNullOrEmpty(m_uiText.text))
        {
            string str = m_uiText.text;
            int num;

            if (int.TryParse(str.Replace(",", ""), out num))
                from = num;
            else
                from = 0;

            m_number = from;
        }
    }

    public TweenNumber From(int f)
    {
        from = f;
        return this;
    }

    public TweenNumber Formatter(string f)
    {
        stringFormatter = f;
        return this;
    }

    /// <summary>
    /// Interpolate and update the alpha.
    /// </summary>

    override protected void OnUpdate(float factor, bool isFinished)
    {
        CurrentNumber = Mathf.RoundToInt(Mathf.Lerp(from, to, factor));
    }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public TweenNumber Begin(GameObject go, float duration, int number)
	{
        TweenNumber comp = Begin<TweenNumber>(go, duration);
		comp.from = comp.CurrentNumber;
        comp.to = number;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
