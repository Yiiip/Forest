using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class sCharacterVO
{
    [Header("ID")]
    public string m_id;
    [Header("昵称")]
    public string m_name;
    [Header("描述")]
    public string m_desc;
    [Header("动物类型")]
    public eAnimalType m_animalType;
    [Header("预制体")]
    public GameObject m_prefab;
    [Header("头像")]
    public Sprite m_headPhoto;
    [Header("变身需要的水数量")]
    public int m_water;
}

public enum eAnimalType
{
    Undefine = 0,
    Rabbit,
}
