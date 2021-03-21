using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicMask : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image image;
    private Material mat;

    private Transform target = null;
    private Action onFinish = null;

    void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        mat = image.material;
    }

    public MagicMask SetTarget(Transform target)
    {
        this.target = target;
        return this;
    }

    public void RemoveTarget()
    {
        this.target = null;
    }

    public void Enable()
    {
        image.enabled = true;
    }

    public void Disable()
    {
        image.enabled = false;
    }

    public void Focus(float fromRadius, float toRadius, float duration = 0.5f, Action onFinish = null)
    {
        this.onFinish = onFinish;
        Enable();
        StartCoroutine(FocusAnim(fromRadius, toRadius, duration));
    }

    public void Focus(float toRadius, float duration = 0.5f, Action onFinish = null)
    {
        Focus(fromRadius: GetRadius(), toRadius: 0, duration, onFinish);
    }

    public void Focus(float duration = 0.5f, Action onFinish = null)
    {
        Focus(toRadius: 0, duration, onFinish);
    }

    private IEnumerator FocusAnim(float fromRadius, float toRadius, float duration)
    {
        float p = 0;
        float spd = 1f / duration;
        while (p < 1)
        {
            p += Time.deltaTime * spd;
            float newVal = Mathf.Lerp(fromRadius, toRadius, p);
            SetRadius(newVal);
            yield return null;
        }

        onFinish?.Invoke();
        onFinish = null;
        if (target != null)
        {
            RemoveTarget();
        }
    }

    public void Disperse(Action onFinish = null)
    {

    }

    private void Update()
    {
        if (target != null)
        {
            if (cam != null)
            {
                Vector2 uiPos = UIUtility.WorldToUGUI(target.position, cam, canvas);
                SetCenter(uiPos);
            }
        }
    }

    public MagicMask SetCenter(Vector2 center)
    {
        if (mat != null)
        {
            mat.SetVector("_Center", center);
        }
        return this;
    }

    public Vector2 GetCenter()
    {
        if (mat != null)
        {
            return mat.GetVector("_Center");
        }
        return default;
    }

    public MagicMask SetRadius(float radius)
    {
        if (mat != null)
        {
            mat.SetFloat("_Radius", radius);
        }
        return this;
    }

    public float GetRadius()
    {
        if (mat != null)
        {
            mat.GetFloat("_Radius");
        }
        return 0f;
    }

    public MagicMask SetRadiusSmooth(float radiusSmooth)
    {
        if (mat != null)
        {
            mat.SetFloat("_RadiusSmooth", radiusSmooth);
        }
        return this;
    }

    public float GetRadiusSmooth()
    {
        if (mat != null)
        {
            mat.GetFloat("_RadiusSmooth");
        }
        return 0f;
    }

    public MagicMask SetColor(Color color)
    {
        if (mat != null)
        {
            mat.SetColor("_MainColor", color);
        }
        return this;
    }

    public Color GetColor()
    {
        if (mat != null)
        {
            return mat.GetColor("_MainColor");
        }
        return Color.black;
    }

    public MagicMask SetTexture(Texture2D tex2D)
    {
        if (mat != null)
        {
            mat.SetTexture("_MainTexture", tex2D);
        }
        return this;
    }

    public MagicMask SetTexture(Sprite sp)
    {
        return SetTexture(sp.texture);
    }

    public MagicMask SetTexture(Image img)
    {
        return SetTexture(img.sprite.texture);
    }
}