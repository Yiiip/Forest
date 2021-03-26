using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : BaseUI
{
    public Canvas canvas;
    public Camera cam;
    public RectTransform Hand;
    public Text HandText;

    private Transform handTarget;

    protected override bool canAutoHide { get => false; }

    public void ShowHand(Transform handTarget, Vector2 offset = default, string hint = null)
    {
        this.handTarget = handTarget;
        Hand.gameObject.SetActive(true);

        if (string.IsNullOrEmpty(hint))
        {
            HandText.gameObject.SetActive(false);
        }
        else
        {
            HandText.gameObject.SetActive(true);
            HandText.text = hint;
        }

        RefreshHandPos();
    }

    private void RefreshHandPos()
    {
        if (handTarget != null)
        {
            var uiPos = UIUtility.WorldToUGUI(handTarget.position, cam, canvas);
            Hand.anchoredPosition = uiPos;
        }
    }

    public void HideHand()
    {
        Hand.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (Hand.gameObject.activeSelf && handTarget != null)
        {
            RefreshHandPos();
        }
    }
}
