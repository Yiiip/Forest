﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : BaseUI
{
    public Text TextCoin;
    public Text TextWater;
    public Text TextGlobalTimer;
    public Text TextTodayTimePercent;
    public Slider SliderGlobalTimer;
    public Button btnSetting;

    protected override bool canAutoHide { get => false; }

    private int uiCoin = 0;
    private int uiWater = 0;

    protected override void Start()
    {
        base.Start();
        btnSetting.onClick.AddListener(OpenSettingUI);

        DebugFPS.Instance.IsAllow = SaveData.current.setting.showFPS;

        GameManager.Instance.AddNewDayChangedListener(OnNewDay);
    }

    public void OpenSettingUI()
    {
        UIManager.Instance.Show(typeof(SettingUI));
    }

    protected override void Update()
    {
        base.Update();

        UpdateCoin();
        UpdateWater();
        UpdateDayTimer();
    }

    private void UpdateCoin()
    {
        if (uiCoin < SaveData.current.playerProfile.coin)
        {
            uiCoin = Mathf.Min(uiCoin + 5, SaveData.current.playerProfile.coin);
        }
        else if (uiCoin > SaveData.current.playerProfile.coin)
        {
            uiCoin = Mathf.Max(uiCoin - 5, SaveData.current.playerProfile.coin);
        }
        TextCoin.text = uiCoin.ToString();
    }

    private void UpdateWater()
    {
        if (uiWater < SaveData.current.playerProfile.water)
        {
            uiWater = Mathf.Min(uiWater + 5, SaveData.current.playerProfile.water);
        }
        else if (uiWater > SaveData.current.playerProfile.water)
        {
            uiWater = Mathf.Max(uiWater - 5, SaveData.current.playerProfile.water);
        }
        TextWater.text = uiWater.ToString();
    }

    private void UpdateDayTimer()
    {
        var percent = GameManager.Instance.World.GetTodayPercent();
        SliderGlobalTimer.value = percent;
        TextTodayTimePercent.text = $"（{Mathf.Ceil(percent * 100f)}%）";
    }

    private void OnNewDay(int curDay)
    {
        TextGlobalTimer.text = $"第{curDay}天";
    }
}
