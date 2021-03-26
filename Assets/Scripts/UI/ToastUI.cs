using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastUI : BaseUI
{
    public RectTransform ToastRect;
    public CanvasGroup ToastCanvasGroup;
    public Text TextToast;
    public float fadeInDuration = 0.3f;
    public float stayDuration = 1f;
    public float fadeOutDuration = 0.3f;

    protected override bool canAutoHide { get => false; }

    private string content;

    protected override void Start()
    {
        base.Start();
        ToastRect.gameObject.SetActive(false);
    }

    public static void Toast(string content)
    {
        UIManager.Instance.GetUI<ToastUI>().ShowToast(content);
    }

    public void ShowToast(string content)
    {
        this.content = content;
        StopCoroutine(nameof(DoToast));
        StartCoroutine(nameof(DoToast));
    }

    private IEnumerator DoToast()
    {
        ToastRect.gameObject.SetActive(true);
        ToastCanvasGroup.alpha = 0f;
        TextToast.text = content;
        float p = 0f;
        while (p < 1f)
        {
            ToastCanvasGroup.alpha = Mathf.Lerp(0f, 1f, p);
            p += 1f / fadeInDuration * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(stayDuration);
        p = 0f;
        while (p < 1f)
        {
            ToastCanvasGroup.alpha = Mathf.Lerp(1f, 0f, p);
            p += 1f / fadeOutDuration * Time.deltaTime;
            yield return null;
        }
        ToastRect.gameObject.SetActive(false);
    }
}
