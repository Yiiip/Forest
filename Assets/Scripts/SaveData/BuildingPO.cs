using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWorkState
{
    None = 0,
    Working,
    ReadyToHavest,
}

[System.Serializable]
public class BuildingPO : IPersistentObject
{
    public int uniqueId;

    /// <summary>
    /// sBuildingVO的m_id
    /// </summary>
    public string staticDataId;

    public float positionX;

    public float positionY;

    public eWorkState workState;

    public float workTimer;


    // private static int tempBuildingId = -1;
    public static BuildingPO Generate(int uniqueId, string staticDataId, Vector3 position)
    {
        var ret = new BuildingPO
        {
            uniqueId = uniqueId,
            staticDataId = staticDataId,
            positionX = position.x,
            positionY = position.y,
        };
        ret.Init();
        return ret;
    }


    public void Init()
    {
    }
}
