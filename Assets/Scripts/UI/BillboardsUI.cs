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
    public RectTransform AppleTreeBar;

    private Transform appleTreeTarget;

    protected override bool canAutoHide { get => false; }

    protected override void Start()
    {
        base.Start();
        HideAppleTreeBillboard();
    }

    public void ShowAppleTreeBillboard(Transform target, Action onBtnClick = null)
    {
        AppleTreeBillboard.gameObject.SetActive(true);
        appleTreeTarget = target;

        // var btn = AppleTreeBillboard.Find("Button").GetComponent<Button>();
        // btn.onClick.RemoveAllListeners();
        // btn.onClick.AddListener(() =>
        // {
        //     onBtnClick?.Invoke();
        // });

        RefreshAppleTreeBillboard();
    }

    public void HideAppleTreeBillboard()
    {
        appleTreeTarget = null;
        AppleTreeBillboard.gameObject.SetActive(false);
    }

    private void RefreshAppleTreeBillboard()
    {
        if (appleTreeTarget != null && AppleTreeBillboard.gameObject.activeSelf)
        {
            var uiPos = UIUtility.WorldToUGUI(appleTreeTarget.position, cam, canvas);
            AppleTreeBillboard.anchoredPosition = uiPos;
        }
    }

    protected override void Update()
    {
        base.Update();
        RefreshAppleTreeBillboard();
    }
}
