using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Toggle toggleFPS;
    public Button btnSaveAndQuit;
    public Button btnClose;

    private void Start()
    {
        btnSaveAndQuit.onClick.AddListener(SaveAndQuit);
        btnClose.onClick.AddListener(OnBtnClose);

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

    public void OnBtnClose()
    {
        this.gameObject.SetActive(false);
    }
}
