using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfilePO : IPersistentObject
{
    /// <summary>
    /// 玩家昵称
    /// </summary>
    public string playerName = "玩家";

    /// <summary>
    /// 玩家等级
    /// </summary>
    public int playerLevel;

    public void Init()
    {
        playerLevel = 1;
    }
}
