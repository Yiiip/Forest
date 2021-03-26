using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StockBlock : MonoBehaviour
{
    [ObjectReference("Image")]
    Image image;
    [ObjectReference("Image")]
    Animation animation;
    public RectTransform rectTransform;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ObjectReferenceAttribute.GetReferences(this);
        animation.Stop();
    }

    public void UpdateColor(bool higher, bool animate = false)
    {
        if (higher) image.color = new Color(253 / 255f, 82 / 255f, 82 / 255f);
        else image.color = new Color(104 / 255f, 243 / 255f, 119 / 255f);
        if (animate) animation.Play();
    }

    public void SetEndBlock()
    {
        image.color = new Color(70f / 255, 70f / 255, 70f / 255);
        rectTransform.anchoredPosition += new Vector2(0, 160);
        image.rectTransform.sizeDelta += new Vector2(0, 500);
    }
}