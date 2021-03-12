using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(menuName = "StaticData/sBuildingVO", fileName = "sBuildingVO")]
[System.Serializable]
public class sBuildingVO
{
    public string m_id;
    public string m_name;
    public string m_desc;
    public eBuildingType m_buildingType;
    public string m_prefab;
    public int m_width;
    public int m_height;
    public int m_playerLevel;

}

public enum eBuildingType
{
    Undefine = 0,
    House,
}