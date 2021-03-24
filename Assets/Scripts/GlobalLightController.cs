using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[System.Serializable]
public class StageColor
{
    [SerializeField] public float percent;
    [SerializeField] public Color color;
}

[RequireComponent(typeof(Light2D))]
public class GlobalLightController : MonoBehaviour
{
    private Light2D light2D;

    [SerializeField] public List<StageColor> colors;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        if (colors == null || colors.Count <= 1)
        {
            return;
        }

        float cur = GameManager.Instance.World.GetTodayPercent();
        StageColor from = null;
        StageColor to = null;
        for (int i = 0; i < colors.Count; i++)
        {
            if (cur >= colors[i].percent)
            {
                from = colors[i];
                break;
            }
        }
        for (int i = 0; i < colors.Count; i++)
        {
            if (cur < colors[i].percent)
            {
                to = colors[i];
                break;
            }
        }
        if (from != null && to != null)
        {
            var p = (cur - from.percent) / (to.percent - from.percent);
            // Debug.Log(p);
            light2D.color = Color.Lerp(from.color, to.color, p);
        }
    }
}
