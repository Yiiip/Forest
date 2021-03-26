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
    private List<RectTransform> appleTreeBars = new List<RectTransform>();

    protected override bool canAutoHide { get => false; }

    protected override void Start()
    {
        base.Start();
        HideAppleTreeBillboard();
    }

    public void ShowAppleTreeBillboard(BuildingEntity target, Action onBtnClick = null)
    {
        AppleTreeBillboard.gameObject.SetActive(true);
        appleTreeTarget = target.transform;
        // AppleTreeBillboard.Find("Title")
        var tips = AppleTreeBillboard.Find("Tips").GetComponent<Text>();
        switch (target.buildingPO.workState)
        {
            case eWorkState.None: tips.text = "点击采摘"; break;
            case eWorkState.Working: tips.text = String.Empty; break;
            case eWorkState.ReadyToHavest: tips.text = "点击收获"; break;
        }

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

    private void RefreshAppleTreeBars()
    {
        if (appleTreeBars.Count == 0)
        {
            foreach (var building in GameManager.Instance.World.BuildingEntities)
            {
                if (building.staticDataId == "AppleTree")
                {
                    var go = GameObject.Instantiate(AppleTreeBar, AppleTreeBar.parent);
                    appleTreeBars.Add(go);
                }
            }
            appleTreeBars.ForEach(i => i.gameObject.SetActiveOptimize(false));
        }
        if (appleTreeBars.Count != 0)
        {
            for (int i = 0, t = 0; i < GameManager.Instance.World.BuildingEntities.Count; i++)
            {
                var building = GameManager.Instance.World.BuildingEntities[i];
                if (building.staticDataId == "AppleTree")
                {
                    if (building.buildingPO.workState == eWorkState.Working || building.buildingPO.workState == eWorkState.ReadyToHavest)
                    {
                        appleTreeBars[t].gameObject.SetActiveOptimize(true);
                        appleTreeBars[t].Find("Slider").GetComponent<Slider>().value = Mathf.Clamp01(building.buildingPO.workTimer / building.staticData.m_workingDuration);
                        var remain = Mathf.CeilToInt(building.staticData.m_workingDuration - building.buildingPO.workTimer);
                        remain = Mathf.Max(remain, 0);
                        appleTreeBars[t].Find("Slider/TextTime").GetComponent<Text>().text = $"成熟倒计时:{remain}秒";
                        appleTreeBars[t].anchoredPosition = UIUtility.WorldToUGUI(building.transform.position, cam, canvas) + new Vector2(0, 70);
                    }
                    else
                    {
                        appleTreeBars[t].gameObject.SetActiveOptimize(false);
                    }
                    t++;
                }
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        RefreshAppleTreeBillboard();
        RefreshAppleTreeBars();
    }
}
