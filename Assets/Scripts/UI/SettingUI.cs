using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
    public Text txtPlayerName;
    public Toggle toggleFPS;
    public Toggle autoSaveWhenQuit;
    public Button btnSaveAndQuit;
    public Button btnResetAndQuit;
    public Button btnClose;

    protected override void OnEnable()
    {
        base.OnEnable();
        btnSaveAndQuit.onClick.AddListener(SaveAndQuit);
        btnResetAndQuit.onClick.AddListener(ResetAndQuit);
        btnClose.onClick.AddListener(base.Hide);

        txtPlayerName.text = $"昵称：{SaveData.current.playerProfile.playerName}";

        if (toggleFPS != null)
        {
            toggleFPS.isOn = SaveData.current.setting.showFPS;
            toggleFPS.onValueChanged.AddListener(delegate(bool value)
            {
                SaveData.current.setting.showFPS = value;
                DebugFPS.Instance.IsAllow = SaveData.current.setting.showFPS;
            });
        }

        if (autoSaveWhenQuit != null)
        {
            autoSaveWhenQuit.isOn = SaveData.current.setting.autoSaveWhenQuit;
            autoSaveWhenQuit.onValueChanged.AddListener(delegate(bool value)
            {
                SaveData.current.setting.autoSaveWhenQuit = value;
                SaveManager.Instance.OnSaveCurrent();
            });
        }
    }

    public void SaveAndQuit()
    {
        SaveManager.Instance.OnSaveCurrent();
        SceneManager.LoadScene("Start");
    }

    public void ResetAndQuit()
    {
        SaveManager.Instance.DeleteCurrent();
        SceneManager.LoadScene("Start");
    }
}
