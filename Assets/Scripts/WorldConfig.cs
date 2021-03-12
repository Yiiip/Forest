using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "游戏世界配置", menuName = "ScriptableObject/游戏世界配置")]
public class WorldConfig : ScriptableObject
{
    [Header("多少秒表示一个游戏日")]
    public float secondsPerDay;
}
