using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class sBuildingVO
{
    public string m_id;
    public string m_name;
    public string m_desc;
    public eBuildingType m_buildingType;
    public GameObject m_prefab;
    public int m_width;
    public int m_height;
    public int m_playerLevel;
    public float m_workingDuration = 30f;
}

public enum eBuildingType
{
    Undefine = 0,
    WaterSource,
    House,
    AppleTree,
}