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
        int from = 0;
        int to = 1;
        int i = 0;
        for (i = 0; i < colors.Count; i++)
        {
            if (cur - colors[i].percent < 0)
            {
                from = i - 1;
                to = i;
                break;
            }
        }
        // Debug.Log($"from {from} to {to}");
        var p = (cur - colors[from].percent) / (colors[to].percent - colors[from].percent);
        light2D.color = Color.Lerp(colors[from].color, colors[to].color, p);
    }
}
