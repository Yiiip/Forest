using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingPO : IPersistentObject
{
    /// <summary>
    /// 是否显示帧率
    /// </summary>
    public bool showFPS;

    public void Init()
    {
    }
}
