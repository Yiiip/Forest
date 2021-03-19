using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCharacterAvator
{
    Animal = 0, //动物形态
    Human  = 1, //拟人形态
}

[System.Serializable]
public class CharacterPO : IPersistentObject
{
    /// <summary>
    /// sCharacterVO的m_id
    /// </summary>
    public string voId;

    /// <summary>
    /// 形态
    /// </summary>
    public eCharacterAvator avator;

    public void Init()
    {
    }
}
