using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfilePO : IPersistentObject
{
    /// <summary>
    /// 玩家昵称
    /// </summary>
    public string playerName;

    /// <summary>
    /// 玩家等级
    /// </summary>
    public int playerLevel;

    /// <summary>
    /// 金钱
    /// </summary>
    public int coin;

    public void Init()
    {
        playerName = "新玩家";
        playerLevel = 1;
        coin = 0;
    }
}
