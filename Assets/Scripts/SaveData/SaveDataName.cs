using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "存档配置", menuName="ScriptableObject/存档配置")]
public class SaveDataName : ScriptableObject
{
    [Header("存档文件名称")]
    public List<string> saveNames;
    [Header("存档昵称")]
    public List<string> saveNicknames;
}
