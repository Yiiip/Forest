using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingPO : IPersistentObject
{
    /// <summary>
    /// 是否显示帧率
    /// </summary>
    public bool showFPS = true;

    /// <summary>
    /// 退出时自动保存
    /// </summary>
    public bool autoSaveWhenQuit = true;

    public void Init()
    {
        showFPS = true;
        autoSaveWhenQuit = true;
    }
}
