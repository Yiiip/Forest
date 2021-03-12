using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : BaseUI
{
    public Button btnSetting;
    public SettingUI settingUI;

    protected override void Start()
    {
        base.Start();
        btnSetting.onClick.AddListener(OpenSettingUI);

        DebugFPS.Instance.IsAllow = SaveData.current.setting.showFPS;
    }

    public void OpenSettingUI()
    {
        settingUI.gameObject.SetActive(true);
    }
}
