using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObject/SaveDataName")]
public class SaveDataName : ScriptableObject
{
    [Header("存档文件名称")]
    public List<string> saveNames;
    [Header("存档昵称")]
    public List<string> saveNicknames;
}
