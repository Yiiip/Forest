using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogUIIntent
{
    public sDialogSheet dialogSheet;
    public Action onFinish;
}

public class DialogUI : BaseUI
{
    public Image imgLeft, imgRight, imgMiddle;
    public Text txtTitle, txtContent;
    public GameObject iconNext;
    public Button btnNext;

    private bool canNext;
    private List<sDialogVO> dialogs;
    private Action onFinish;

    void Awake()
    {
        imgLeft.gameObject.SetActiveOptimize(false);
        imgRight.gameObject.SetActiveOptimize(false);
        imgMiddle.gameObject.SetActiveOptimize(false);
        txtTitle.text = String.Empty;
        txtContent.text = String.Empty;
        iconNext.SetActiveOptimize(false);
        btnNext.onClick.AddListener(delegate()
        {
            canNext = true;
        });
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        DialogUIIntent data = (DialogUIIntent) intent;
        dialogs = data.dialogSheet.dialogVOs;
        onFinish = data.onFinish;
        canNext = false;
        StartCoroutine(nameof(PlayDialogs));
    }

    IEnumerator PlayDialogs()
    {
        for (int i = 0; i < dialogs.Count; i++)
        // for (int i = 0; i <= 1; i++) //DEBUG
        {
            canNext = false;

            iconNext.SetActiveOptimize(false);
            btnNext.interactable = false;

            sDialogVO dialog = dialogs[i];
            sCharacterVO characterVO = StaticDataManager.Instance.GetCharacterVO(dialog.m_targetId);
            txtTitle.text = !string.IsNullOrEmpty(dialog.m_title) ? dialog.m_title : (characterVO != null ? characterVO.m_name??string.Empty : string.Empty);

            bool waitText = false;
            txtContent.TypeText(dialog.m_content, 0.015f, delegate()
            {
                waitText = true;
            });

            if (characterVO == null || characterVO.m_headPhoto == null)
            {
                imgLeft.gameObject.SetActiveOptimize(false);
                imgRight.gameObject.SetActiveOptimize(false);
                imgMiddle.gameObject.SetActiveOptimize(false);
            }
            else
            {
                switch (dialog.m_side)
                {
                    case eDialogSide.Left:
                        imgLeft.gameObject.SetActiveOptimize(true);
                        imgRight.gameObject.SetActiveOptimize(false);
                        imgMiddle.gameObject.SetActiveOptimize(false);
                        imgLeft.SetImage(characterVO.m_headPhoto);
                        break;
                    case eDialogSide.Right:
                        imgLeft.gameObject.SetActiveOptimize(false);
                        imgRight.gameObject.SetActiveOptimize(true);
                        imgMiddle.gameObject.SetActiveOptimize(false);
                        imgRight.SetImage(characterVO.m_headPhoto);
                        break;
                    case eDialogSide.Middle:
                        imgLeft.gameObject.SetActiveOptimize(false);
                        imgRight.gameObject.SetActiveOptimize(false);
                        imgMiddle.gameObject.SetActiveOptimize(true);
                        imgMiddle.SetImage(characterVO.m_headPhoto);
                        break;
                }
            }

            // SaveData.current.playerProfile.globalTimer = Mathf.Max(1, SaveData.current.playerProfile.globalTimer - 0.3f);

            yield return new WaitUntil(() => waitText);

            btnNext.interactable = true;
            iconNext.SetActiveOptimize(true);

            yield return new WaitUntil(() => canNext);
        }

        base.Hide();
        onFinish?.Invoke();
    }
}
