using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCharacterAvator
{
    Animal = 0, //动物形态
    Human = 1, //拟人形态
}

[System.Serializable]
public class CharacterPO : IPersistentObject
{
    public int uniqueId;

    /// <summary>
    /// sCharacterVO的m_id
    /// </summary>
    public string staticDataId;

    /// <summary>
    /// 形态
    /// </summary>
    public eCharacterAvator avator;

    public void Init()
    {
    }

#region Factory
    public static CharacterPO CreateRabbit()
    {
        var ret = new CharacterPO
        {
            uniqueId = 1,
            staticDataId = "Rabbit",
            avator = eCharacterAvator.Animal,
        };
        ret.Init();
        return ret;
    }

    public static CharacterPO CreateBear()
    {
        var ret = new CharacterPO
        {
            uniqueId = 2,
            staticDataId = "Bear",
            avator = eCharacterAvator.Animal,
        };
        ret.Init();
        return ret;
    }

    public static CharacterPO CreateMonkey()
    {
        var ret = new CharacterPO
        {
            uniqueId = 3,
            staticDataId = "Monkey",
            avator = eCharacterAvator.Animal,
        };
        ret.Init();
        return ret;
    }
#endregion
}