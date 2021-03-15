using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "对话表", menuName = "StaticData/对话表")]
public class sDialogSheet : ScriptableObject
{
    public string m_sheetId;
    public List<sDialogVO> dialogVOs;
}
