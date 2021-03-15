using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class sDialogVO
{
    public string m_title;
    public string m_content;
    public eDialogSide m_side;
    public string m_targetId;
}

public enum eDialogSide
{
    Left = 0,
    Right,
    Middle,
}