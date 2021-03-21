using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIUtility
{
    public static T FindInParents<T>(this GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null) return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    public static bool HasComponent<T>(this GameObject go) where T : Component
    {
        return go.GetComponent<T>() != null;
    }

    public static GameObject InstantiatePrefab(GameObject prefab, Transform parent)
    {
        GameObject instantiation = GameObject.Instantiate(prefab);
        if (parent != null) instantiation.transform.SetParent(parent, false);
        return instantiation;
    }

    public static void SetActiveOptimize(this GameObject go, bool isActive)
    {
        if (go != null && go.activeSelf != isActive) go.SetActive(isActive);
    }

    public static void SetText(this Text textView, float floatText)
    {
        if (textView != null) textView.text = floatText.ToString();
    }
    public static void SetText(this Text textView, int IntText)
    {
        if (textView != null) textView.text = IntText.ToString();
    }
    public static void SetText(this Text textView, System.Text.StringBuilder stringBuilder)
    {
        if (textView != null) textView.text = stringBuilder.ToString();
    }

    public static void SetText(this InputField inputField, string text)
    {
        if (inputField != null) inputField.text = text;
    }

    public static void SetText(this Dropdown dropdown, string text)
    {
        if (dropdown != null) dropdown.captionText.text = text;
    }

    public static void SetImage(this Image imageView, Sprite sprite)
    {
        if (imageView != null) imageView.sprite = sprite;
    }

    public static void SetImage(this RawImage rawImage, Texture2D tex2d)
    {
        if (rawImage != null && tex2d != null) rawImage.texture = tex2d;
    }

    public static void SetImageFill(this Image imageView, float fillAmount)
    {
        if (imageView != null) imageView.fillAmount = fillAmount;
    }

    public static void SetColor(this Graphic ui, Color color)
    {
        if (ui != null) ui.color = color;
    }
    public static void SetAlpha(this Graphic ui, float a)
    {
        if (ui != null)
        {
            var color = ui.color;
            color.a = a;
            ui.color = color;
        }
    }
    public static bool IsTotalTransparent(this Graphic ui)
    {
        return (ui != null && ui.color.a == 0);
    }
    public static void SetColorToTransparent(this Graphic ui)
    {
        SetAlpha(ui, 0.0f);
    }
    public static void SetColorToOpaque(this Graphic ui)
    {
        SetAlpha(ui, 1.0f);
    }

    public static void SetSliderValues(this Slider slider, float min, float max, float value)
    {
        if (slider != null)
        {
            slider.minValue = min;
            slider.maxValue = max;
            slider.value = Mathf.Clamp(value, min, max);
        }
    }

    public static void SetInteractable(this Selectable ui, bool interactable)
    {
        if (ui != null && ui.interactable != interactable) ui.interactable = interactable;
    }

    public static string ColoredRechText(this string text, string colorHex)
    {
        return "<color=" + colorHex + ">" + text + "</color>";
    }
    public static string ColoredRechText(this int itext, string colorHex)
    {
        return "<color=" + colorHex + ">" + itext + "</color>";
    }
    public static string ColoredRechText(this float ftext, string colorHex)
    {
        return "<color=" + colorHex + ">" + ftext + "</color>";
    }

    public static void AnchoredPosToZero(this RectTransform rectTransform)
    {
        if (rectTransform != null) rectTransform.anchoredPosition = Vector2.zero;
    }

    [System.Obsolete("Not recommend.")]
    public static void ScrollToTop(GridLayoutGroup gridLayoutGroup)
    {
        if (gridLayoutGroup != null) gridLayoutGroup.GetComponent<RectTransform>().AnchoredPosToZero();
    }

    [System.Obsolete("Not recommend.")]
    public static void ScrollToTop(VerticalLayoutGroup verticalLayoutGroup)
    {
        if (verticalLayoutGroup != null) verticalLayoutGroup.GetComponent<RectTransform>().AnchoredPosToZero();
    }

    [System.Obsolete("Not recommend.")]
    public static void ScrollToTop(HorizontalLayoutGroup horizontalLayoutGroup)
    {
        if (horizontalLayoutGroup != null) horizontalLayoutGroup.GetComponent<RectTransform>().AnchoredPosToZero();
    }

    public static void ScrollToTop(this ScrollRect scrollRect)
    {
        if (scrollRect != null) scrollRect.normalizedPosition = new Vector2(0, 1);
    }
    public static void ScrollToBottom(this ScrollRect scrollRect)
    {
        if (scrollRect != null) scrollRect.normalizedPosition = new Vector2(1, 0);
    }

    //Created by lyp 获取Camera的快照
    public static Texture2D RenderRTImage(Camera cam)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D tex2d = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        tex2d.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        tex2d.Apply();
        RenderTexture.active = currentRT;
        return tex2d;
    }

    //获取三维模型的中心点位置（非锚点位置）
    public static Vector3 Get3DMeshBoundsCenter(this Renderer renderer)
    {
        if (renderer != null) return renderer.bounds.center;
        else return Vector3.zero;
    }
    public static Vector3 Get3DMeshBoundsSize(this Renderer renderer)
    {
        if (renderer != null) return renderer.bounds.size;
        else return Vector3.zero;
    }
    public static Vector3 Get3DModelSize(GameObject gameObject)
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        if (mesh == null) return Vector3.zero;

        Vector3 meshSize = mesh.bounds.size; //模型网格的大小
        Vector3 scale = gameObject.transform.lossyScale; //放缩
        return new Vector3(meshSize.x * scale.x, meshSize.y * scale.y, meshSize.z * scale.z); //游戏中的实际大小
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    //设置材质的渲染模式
    public static void SetMaterialRenderingMode(Material material, int mode)
    {
        if (material.GetFloat("_Mode") == mode)
        {
            return;
        }
        switch (mode)
        {
            case 0: // Opaque
                material.SetFloat("_Mode", 0);
                material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case 1: // Cutout
                material.SetFloat("_Mode", 1);
                material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case 2: // Fade
                material.SetFloat("_Mode", 2);
                material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case 3: // Transparent
                material.SetFloat("_Mode", 3);
                material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }

    public static Vector3 ScreenPointToWorldPosition(Camera camera, Vector2 position)
    {
        return camera.ScreenToWorldPoint(new Vector3(position.x, camera.pixelHeight - position.y, camera.nearClipPlane));
    }

    /// <summary>
    /// 世界坐标转为UGUI坐标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector2 WorldToUGUI(Vector3 worldPos, Camera cam, Canvas canvas)
    {

        Vector2 screenPoint = cam.WorldToScreenPoint(worldPos);//世界坐标转换为屏幕坐标
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        screenPoint -= screenSize * 0.5f;//将屏幕坐标变换为以屏幕中心为原点
        Vector2 anchorPos = screenPoint / screenSize * (canvas.transform as RectTransform).sizeDelta;//缩放得到UGUI坐标
        return anchorPos;
    }

    public static Vector3 WorldToUI(Vector3 worldPos, Camera cam, Canvas canvas)
    {
        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        float resolutionX = scaler.referenceResolution.x;
        float resolutionY = scaler.referenceResolution.y;
        Vector3 viewportPos = cam.WorldToViewportPoint(worldPos);
        Vector3 uiPos = new Vector3(viewportPos.x * resolutionX - resolutionX * 0.5f, viewportPos.y * resolutionY - resolutionY * 0.5f, 0f);
        return uiPos;
    }

    public static Vector2 WorldToUIPoint(Vector3 worldPos, Camera cam, Canvas canvas)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            cam.WorldToScreenPoint(worldPos),
            cam,
            out pos);
        return pos;
    }

    //判断物体是否在相机前面
    public static bool IsInVision(Vector3 worldPos, Camera cam)
    {
        Transform camTransform = cam.transform;
        Vector3 dir = (worldPos - camTransform.position).normalized;
        float dot = Vector3.Dot(camTransform.forward, dir);
        if (dot <= 0)
        {
            return false;
        }
        else
        {
            Vector2 viewportPos = cam.WorldToViewportPoint(worldPos);
            if (viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static float Mapping(float val, float a, float b, float toA, float toB)
    {
        val = Mathf.Clamp(val, a, b);
        float progress = (val - a) / (b - a);
        return Mathf.Lerp(toA, toB, progress);
    }

    public static IEnumerator DelayInteractiveBtn(Button btn, float delay)
    {
        btn.interactable = false;
        yield return new WaitForSeconds(delay);
        btn.interactable = true;
    }

    public static string ToFullString(this System.DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }

    public static bool IsIndexValid(this IList list, int index)
    {
        if (list == null || list.IsEmpty())
            return false;
        else
            return index >= 0 && index < list.Count;
    }

    public static bool IsEmpty(this ICollection collection)
    {
        return collection.Count <= 0;
    }

    public static bool IsNotEmpty(this ICollection collection)
    {
        return collection.Count > 0;
    }

    public static bool IsEmpty(this Array array)
    {
        return array == null || array.Length <= 0;
    }

    public static bool IsNotEmpty(this Array array)
    {
        return array != null && array.Length > 0;
    }
}