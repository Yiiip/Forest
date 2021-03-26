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

    /// <summary>
    /// 山水本源
    /// </summary>
    public int water = 300;

    /// <summary>
    /// 游戏世界时间
    /// </summary>
    public float globalTimer;

    public void Init()
    {
        playerName = "新玩家";
        playerLevel = 1;
        coin = 0;
        water = 0;
        globalTimer = 0;
    }
}
