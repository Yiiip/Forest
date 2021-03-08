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

    public void Init()
    {
    }
}
