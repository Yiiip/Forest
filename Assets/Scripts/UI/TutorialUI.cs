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

    protected override bool canAutoHide { get => false; }

    public void ShowHand(Vector3 worldPos, Vector2 offset = default, string hint = null)
    {
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

        var uiPos = UIUtility.WorldToUGUI(worldPos, cam, canvas);
        Hand.anchoredPosition = uiPos;
    }

    public void HideHand()
    {
        Hand.gameObject.SetActive(false);
    }
}
