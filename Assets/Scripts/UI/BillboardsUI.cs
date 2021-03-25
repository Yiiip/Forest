using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillboardsUI : BaseUI
{
    public Canvas canvas;
    public Camera cam;
    public RectTransform AppleTreeBillboard;

    protected override bool canAutoHide { get => false; }

    protected override void Start()
    {
        base.Start();
        HideAppleTreeBillboard();
    }

    public void ShowAppleTreeBillboard(Transform target, Action onBtnClick)
    {
        AppleTreeBillboard.gameObject.SetActive(true);
        var uiPos = UIUtility.WorldToUGUI(target.position, cam, canvas);
        AppleTreeBillboard.anchoredPosition = uiPos;

        var btn = AppleTreeBillboard.Find("Button").GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            onBtnClick?.Invoke();
        });
    }

    public void HideAppleTreeBillboard()
    {
        AppleTreeBillboard.gameObject.SetActive(false);
    }
}
