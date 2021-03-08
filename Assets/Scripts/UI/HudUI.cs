using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    public Button btnSetting;
    public SettingUI settingUI;

    private void Start()
    {
        btnSetting.onClick.AddListener(OpenSettingUI);

        DebugFPS.Instance.IsAllow = SaveData.current.setting.showFPS;
    }

    public void OpenSettingUI()
    {
        settingUI.gameObject.SetActive(true);
    }
}
