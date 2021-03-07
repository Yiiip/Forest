using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingPO
{
    /// <summary>
    /// 是否显示帧率
    /// </summary>
    public bool showFPS;

    /// <summary>
    /// 存档时间
    /// </summary>
    public List<long> saveTimestampList;
}
