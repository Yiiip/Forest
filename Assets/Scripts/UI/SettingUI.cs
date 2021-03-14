using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
    public Text txtPlayerName;
    public Toggle toggleFPS;
    public Button btnSaveAndQuit;
    public Button btnResetAndQuit;
    public Button btnClose;

    protected override void Start()
    {
        base.Start();
        btnSaveAndQuit.onClick.AddListener(SaveAndQuit);
        btnResetAndQuit.onClick.AddListener(ResetAndQuit);
        btnClose.onClick.AddListener(OnBtnClose);

        txtPlayerName.text = $"昵称：{SaveData.current.playerProfile.playerName}";

        toggleFPS.isOn = SaveData.current.setting.showFPS;
        toggleFPS.onValueChanged.AddListener(delegate(bool value)
        {
            SaveData.current.setting.showFPS = value;
            DebugFPS.Instance.IsAllow = SaveData.current.setting.showFPS;
        });
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

    public void OnBtnClose()
    {
        this.gameObject.SetActive(false);
    }
}
