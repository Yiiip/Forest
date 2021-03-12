using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "建筑表", menuName = "StaticData/建筑表")]
public class sBuildingSheet : ScriptableObject
{
    public List<sBuildingVO> buildings;
}
