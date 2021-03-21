﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : BaseUI
{
    public Text TextCoin;
    public Text TextWater;
    public Text TextGlobalTimer;
    public Button btnSetting;

    protected override bool canAutoHide { get => false; }

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
        TextCoin.text = $"{SaveData.current.playerProfile.coin}";
    }

    private void OnNewDay(int curDay)
    {
        TextGlobalTimer.text = $"第{curDay}天";
        if (curDay > 1)
        {
            SaveData.current.playerProfile.coin += 100;
        }
    }
}
