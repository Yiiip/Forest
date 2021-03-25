using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StockBlock : MonoBehaviour
{
    [ObjectReference("Image")]
    Image image;
    public RectTransform rectTransform;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ObjectReferenceAttribute.GetReferences(this);
    }

    public void UpdateColor(bool higher)
    {
        if (higher) image.color = new Color(253 / 255f, 82 / 255f, 82 / 255f);
        else image.color = new Color(104 / 255f, 243 / 255f, 119 / 255f);
    }

    public void SetEndBlock()
    {
        image.color = new Color(70f / 255, 70f / 255, 70f / 255);
        rectTransform.anchoredPosition += new Vector2(0, 280);
    }
}